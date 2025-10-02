using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CollabCore.Infrastructure.Data;
using CollabCore.Contracts.Requests;
using CollabCore.Contracts.Responses;
using CollabCore.Core.Entities;

namespace CollabCore.Api.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        public readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponse>>> GetTasks(
            Guid projectId,
            [FromQuery] QueryParameters query,
            [FromQuery] string? status,
            [FromQuery] string? priority)
        {
            var tasks = _context.Tasks
                .Where(t => t.ProjectId == projectId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                tasks = tasks.Where(task =>
                    task.Title.Contains(query.Search) ||
                    (task.Description != null && task.Description.Contains(query.Search))
                );
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                tasks = tasks.Where(task => task.Status == status);
            }

            if (!string.IsNullOrWhiteSpace(priority))
            {
                tasks = tasks.Where(task => task.Priority == priority);
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                tasks = query.SortBy.ToLower() switch
                {
                    "title" => query.SortDesc
                        ? tasks.OrderByDescending(task => task.Title)
                        : tasks.OrderBy(task => task.Title),
                    "duedate" => query.SortDesc
                        ? tasks.OrderByDescending(task => task.DueDate)
                        : tasks.OrderBy(task => task.DueDate),
                    "createdat" => query.SortDesc
                        ? tasks.OrderByDescending(task => task.CreatedAt)
                        : tasks.OrderBy(task => task.CreatedAt),
                    _ => tasks.OrderBy(task => task.CreatedAt)
                };
            }
            else
            {
                tasks = tasks.OrderBy(task => task.CreatedBy);
            }

            var totalItems = await tasks.CountAsync();
            var items = await tasks
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(t => new TaskResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Priority = t.Priority,
                    DueDate = t.DueDate,
                    ProjectId = t.ProjectId,
                    CreatedBy = t.CreatedBy
                })
                .ToListAsync();


            return Ok(new
            {
                TotalItems = totalItems,
                Page = query.Page,
                PageSize = query.PageSize,
                Items = items
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponse>> GetTask(Guid projectId, Guid id)
        {
            var task = await _context.Tasks
                .Where(t => t.ProjectId == projectId && t.Id == id)
                .FirstOrDefaultAsync();

            if (task == null) return NotFound();

            return Ok(new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                DueDate = task.DueDate,
                ProjectId = task.ProjectId,
                CreatedBy = task.CreatedBy
            });
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponse>> CreateTask(Guid projectId, TaskCreateDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("User ID not found in token.");

            var userId = Guid.Parse(userIdClaim.Value);

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

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var response = new TaskResponse
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

            return CreatedAtAction(nameof(GetTask), new { projectId = projectId, id = task.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid projectId, Guid id, TaskUpdateDto dto)
        {
            var task = await _context.Tasks
                .Where(t => t.ProjectId == projectId && t.Id == id)
                .FirstOrDefaultAsync();

            if (task == null) return NotFound();

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = dto.Status;
            task.Priority = dto.Priority;
            task.DueDate = dto.DueDate;
            task.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var response = new TaskResponse
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

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid projectId, Guid id)
        {
            var task = await _context.Tasks
                .Where(t => t.ProjectId == projectId && t.Id == id)
                .FirstOrDefaultAsync();

            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}