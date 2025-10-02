using CollabCore.Core.Entities;
using CollabCore.Core.Common;

namespace CollabCore.Core.Interfaces 
{
    public interface ITaskRepository 
    {
        Task<IEnumerable<TaskItem>> GetPagedAsync(TaskQuery query);
        Task<TaskItem?> GetByIdAsync(Guid projectId, Guid taskId);
        Task<int> CountAsync(TaskQuery query);
        Task<TaskItem> AddAsync(TaskItem task);
        Task UpdateAsync(TaskItem task);
        Task DeleteAsync(TaskItem task);
    }
}