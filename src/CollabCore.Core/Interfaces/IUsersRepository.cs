using CollabCore.Core.Entities;

namespace CollabCore.Core.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<Project>> GetOwnedProjectsAsync(Guid id);
    }
}