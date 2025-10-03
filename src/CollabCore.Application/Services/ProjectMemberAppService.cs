using CollabCore.Contracts.Responses;
using CollabCore.Core.Interfaces;

namespace CollabCore.Application.Services
{
    public class ProjectMemberAppService
    {
        private readonly IProjectMemberRepository _projectMemberRepository;

        public ProjectMemberAppService(IProjectMemberRepository projectMemberRepository)
        {
            _projectMemberRepository = projectMemberRepository;
        }

        public Task AddProjectMemberAsync(Guid projectId, Guid userId, string role) =>
            _projectMemberRepository.AddProjectMemberAsync(projectId, userId, role);
        public Task RemoveProjectMemberAsync(Guid projectId, Guid userId) =>
            _projectMemberRepository.RemoveProjectMemberAsync(projectId, userId);
        public Task<IEnumerable<ProjectMemberResponse>> GetProjectMembersAsync(Guid projectId) =>
            _projectMemberRepository.GetProjectMembersAsync(projectId);
    }
}