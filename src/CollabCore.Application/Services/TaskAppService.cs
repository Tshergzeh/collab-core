using CollabCore.Contracts.Requests;
using CollabCore.Contracts.Responses;
using CollabCore.Core.Entities;
using CollabCore.Core.Interfaces;
using CollabCore.Core.Common;

namespace CollabCore.Application.Services
{
    public class TaskAppService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskAppService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<(IEnumerable<TaskResponse> Items, int TotalItems)> GetTasks(
            Guid projectId, 
            QueryParameters query,
            string? status,
            string? priority)
        {
            var domainQuery = new TaskQuery
            {
                ProjectId = projectId,
                Search = query.Search,
                Status = status,
                Priority = priority,
                SortBy = query.SortBy,
                SortDesc = query.SortDesc,
                Page = query.Page,
                PageSize = query.PageSize
            };

            var items = await _taskRepository.GetPagedAsync(domainQuery);
            var total = await _taskRepository.CountAsync(domainQuery);
            
            return (items.Select(task => new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                DueDate = task.DueDate,
                ProjectId = task.ProjectId,
                CreatedBy = task.CreatedBy,
                CreatedAt = task.CreatedAt
            }), total);
        }

        public async Task<TaskResponse?> GetTask(Guid projectId, Guid taskId)
        {
            var task = await _taskRepository.GetByIdAsync(projectId, taskId);
            return task == null ? null : new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                DueDate = task.DueDate,
                ProjectId = task.ProjectId,
                CreatedBy = task.CreatedBy,
                CreatedAt = task.CreatedAt
            };
        }

        public async Task<TaskResponse> CreateTask(
            TaskCreateDto dto, 
            Guid projectId, 
            Guid userId)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                ProjectId = projectId,
                CreatedBy = userId
            };

            await _taskRepository.AddAsync(task);

            return new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                DueDate = task.DueDate,
                ProjectId = task.ProjectId,
                CreatedBy = task.CreatedBy,
            };
        }

        public async Task<TaskResponse?> UpdateTask(
            Guid projectId, 
            Guid taskId, 
            TaskUpdateDto dto)
        {
            var task = await _taskRepository.GetByIdAsync(projectId, taskId);
            if (task == null) return null;

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = dto.Status;
            task.Priority = dto.Priority;
            task.DueDate = dto.DueDate;
            task.ModifiedAt = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(task);

            return new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                DueDate = task.DueDate,
                ProjectId = task.ProjectId,
                CreatedBy = task.CreatedBy
            };
        }

        public async Task<bool> DeleteTask(Guid projectId, Guid taskId)
        {
            var task = await _taskRepository.GetByIdAsync(projectId, taskId);
            if (task == null) return false;

            await _taskRepository.DeleteAsync(task);
            return true;
        }
    }
}