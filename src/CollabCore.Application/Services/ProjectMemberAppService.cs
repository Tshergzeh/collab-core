using CollabCore.Contracts.Requests;
using CollabCore.Core.Interfaces;

namespace CollabCore.Application.Services
{
    public class ProjectMemberAppService
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentUserService _currentUser;

        public ProjectMemberAppService(
            IProjectMemberRepository projectMemberRepository,
            IProjectRepository projectRepository,
            ICurrentUserService currentUser)
        {
            _projectMemberRepository = projectMemberRepository;
            _projectRepository = projectRepository;
            _currentUser = currentUser;
        }

        public Task AddProjectMemberAsync(Guid projectId, Guid userId, string role) =>
            _projectMemberRepository.AddProjectMemberAsync(projectId, userId, role);
        public Task RemoveProjectMemberAsync(Guid projectId, Guid userId) =>
            _projectMemberRepository.RemoveProjectMemberAsync(projectId, userId);
        public Task<IEnumerable<ProjectMemberResponse>> GetProjectMembersAsync(Guid projectId) =>
            _projectMemberRepository.GetProjectMembersAsync(projectId);
        public async Task<ProjectMemberResponse> UpdateMemberRoleAsync(
            Guid projectId,
            UpdateProjectMemberRoleRequest request)
        {
            var currentUserId = _currentUser.GetCurrentUserId();

            var isOwner = await _projectRepository.IsProjectOwnerAsync(projectId, currentUserId);
            var IsProjectManager = await _projectMemberRepository.IsProjectManagerAsync(
                projectId,
                currentUserId);

            if (!isOwner && !IsProjectManager) throw new UnauthorizedAccessException(
                "Only project owners and project managers can update members' roles");

            await _projectMemberRepository.UpdateMemberRoleAsync(
                projectId,
                request.UserId,
                request.Role);

            var members = await _projectMemberRepository.GetProjectMembersAsync(projectId);
            return members.First(member => member.Id == request.UserId);
        }
    }
}