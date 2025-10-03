using CollabCore.Core.Interfaces;

namespace CollabCore.Application.Services
{
    public class AuthorizationService
    {
        private readonly IProjectMemberRepository _projectMemberRepository;

        public AuthorizationService(IProjectMemberRepository projectMemberRepository)
        {
            _projectMemberRepository = projectMemberRepository;
        }

        public Task<bool> IsProjectMemberAsync(Guid projectId, Guid userId)
        {
            return _projectMemberRepository.IsProjectMemberAsync(projectId, userId);
        }
    }
}