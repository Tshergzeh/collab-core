using CollabCore.Infrastructure.Data;
using CollabCore.Core.Entities;
using CollabCore.Core.Interfaces;
using CollabCore.Core.Common;
using Microsoft.EntityFrameworkCore;

namespace CollabCore.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;
        
        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetPagedAsync (TaskQuery query)
        {
            var tasks = _context.Tasks.Where(task =>
                task.ProjectId == query.ProjectId);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                tasks = tasks.Where(task => task.Title.Contains(query.Search)
                    || (task.Description != null && 
                        task.Description.Contains(query.Search)));
            }

            if (!string.IsNullOrWhiteSpace(query.Status))
                tasks = tasks.Where(task => task.Status == query.Status);

            if (!string.IsNullOrWhiteSpace(query.Priority))
                tasks = tasks.Where(task => task.Priority == query.Priority);

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                tasks = query.SortBy.ToLower() switch
                {
                    "title" => query.SortDesc
                        ? tasks.OrderByDescending(task => task.Title)
                        : tasks.OrderBy(task => task.Title),
                    "duedate" => query.SortDesc
                        ? tasks.OrderByDescending(task => task.Title)
                        : tasks.OrderBy(task => task.Title),
                    "createdat" => query.SortDesc
                        ? tasks.OrderByDescending(task => task.CreatedAt)
                        : tasks.OrderBy(task => task.CreatedAt),
                    _ => tasks.OrderBy(task => task.CreatedAt)
                };
            }
            else
            {
                tasks = tasks.OrderBy(task => task.CreatedAt);
            }

            return await tasks
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(Guid projectId, Guid taskId) =>
            await _context.Tasks.FirstOrDefaultAsync(task => 
                task.ProjectId == projectId &&
                task.Id == taskId);

        public async Task<int> CountAsync(TaskQuery query)
        {
            var tasks = _context.Tasks.Where(task => 
                task.ProjectId == query.ProjectId);

            if (!string.IsNullOrWhiteSpace(query.Status))
                tasks = tasks.Where(task => task.Status == query.Status);

            if (!string.IsNullOrWhiteSpace(query.Priority))
                tasks = tasks.Where(task => task.Priority == query.Priority);

            return await tasks.CountAsync();
        }

        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}