using CollabCore.Core.Entities;
using CollabCore.Core.Common;

namespace CollabCore.Core.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(Guid id);
        Task<IEnumerable<Project>> GetPagedAsync(ProjectQuery query);
        Task<int> CountAsync(ProjectQuery query);
        Task<Project> AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
    }
}