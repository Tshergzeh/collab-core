using CollabCore.Core.Entities;
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

        public Task AddProjectMemberAsync(Guid projectId, Guid userId) =>
            _projectMemberRepository.AddProjectMemberAsync(projectId, userId);
        public Task RemoveProjectMemberAsync(Guid projectId, Guid userId) =>
            _projectMemberRepository.RemoveProjectMemberAsync(projectId, userId);
        public Task<IEnumerable<User>> GetProjectMembersAsync(Guid projectId) =>
            _projectMemberRepository.GetProjectMembersAsync(projectId);
    }
}