using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CollabCore.Infrastructure.Data;
using CollabCore.Contracts.Requests;
using CollabCore.Contracts.Responses;
using CollabCore.Core.Entities;
using CollabCore.Application.Services;

namespace CollabCore.Api.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        public readonly TaskAppService _taskAppService;

        public TasksController(TaskAppService taskAppService)
        {
            _taskAppService = taskAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(
            Guid projectId,
            [FromQuery] QueryParameters query,
            [FromQuery] string? status,
            [FromQuery] string? priority)
        {
            var (items, total) = await _taskAppService.GetTasks(
                projectId, 
                query, 
                status, 
                priority);

            var response = new {
                Items = items,
                TotalItems = total,
                Page = query.Page,
                PageSize = query.PageSize
            };

            return Ok(response);
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTask(Guid projectId, Guid taskId)
        {
            var task = await _taskAppService.GetTask(projectId, taskId);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(Guid projectId, TaskCreateDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("User ID not found in token.");
            var userId = Guid.Parse(userIdClaim.Value);

            var task = await _taskAppService.CreateTask(dto, projectId, userId);
            
            return CreatedAtAction(
                nameof(GetTask), 
                new { projectId = projectId, id = task.Id},
                task);
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(Guid projectId, Guid taskId, TaskUpdateDto dto)
        {
            var updated = await _taskAppService.UpdateTask(projectId, taskId, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid projectId, Guid taskId)
        {
            var deleted = await _taskAppService.DeleteTask(projectId, taskId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}