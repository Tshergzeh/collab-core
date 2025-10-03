using CollabCore.Core.Entities;

namespace CollabCore.Core.Interfaces
{
    public interface IAssignmentsRepository
    {
        Task AddAsync(Assignment assignment);
        Task<bool> ExistsAsync(Guid taskId, Guid userId);
    }
}