using CollabCore.Core.Interfaces;

namespace CollabCore.Application.Services
{
    public class AuthorizationService
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectRepository _projectRepository;

        public AuthorizationService(IProjectMemberRepository projectMemberRepository,
            IProjectRepository projectRepository)
        {
            _projectMemberRepository = projectMemberRepository;
            _projectRepository = projectRepository;
        }

        public Task<bool> IsProjectMemberAsync(Guid projectId, Guid userId)
        {
            return _projectMemberRepository.IsProjectMemberAsync(projectId, userId);
        }

        public Task<bool> IsProjectOwnerAsync(Guid projectId, Guid userId)
        {
            return _projectRepository.IsProjectOwnerAsync(projectId, userId);
        }
    }
}