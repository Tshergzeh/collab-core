using CollabCore.Core.Entities;

namespace CollabCore.Core.Interfaces
{
    public interface IAssignmentsRepository
    {
        Task AddAsync(Assignment assignment);
    }
}