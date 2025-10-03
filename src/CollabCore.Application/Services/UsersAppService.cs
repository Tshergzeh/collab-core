using CollabCore.Contracts.Responses;
using CollabCore.Core.Interfaces;

namespace CollabCore.Application.Services
{
    public class UsersAppService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersAppService(IUsersRepository usersrepository)
        {
            _usersRepository = usersrepository;
        }

        public async Task<IEnumerable<ProjectResponse>> GetOwnedProjects(Guid id)
        {
            var projects = await _usersRepository.GetOwnedProjectsAsync(id);

            return projects.Select(project => new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                OwnerId = project.OwnerId,
                CreatedAt = project.CreatedAt,
                ModifiedAt = project.ModifiedAt
            });
        }
    }
}