using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CollabCore.Infrastructure.Data;
using CollabCore.Contracts.Requests;
using CollabCore.Contracts.Responses;
using CollabCore.Core.Entities;
using CollabCore.Application.Services;
using CollabCore.Infrastructure.Services;

namespace CollabCore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectAppService _projectAppService;
        private readonly CurrentUserService _currentUser;

        public ProjectsController(
            ProjectAppService projectAppService,
            CurrentUserService currentUser)
        {
            _projectAppService = projectAppService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects([FromQuery] QueryParameters query)
        {
            var (items, total) = await _projectAppService.GetProjects(query);

            var response = new {
                Items = items,
                TotalItems = total,
                Page = query.Page,
                PageSize = query.PageSize
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(Guid id)
        {
            var project = await _projectAppService.GetProject(id);
            return project == null ? NotFound() : Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectCreateDto dto)
        {
            var userId = _currentUser.GetCurrentUserId();

            var project = await _projectAppService.CreateProject(dto, userId);
            
            return CreatedAtAction(nameof(GetProject), 
                new { id = project.Id},
                project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, ProjectUpdateDto dto)
        {
            var updated = await _projectAppService.UpdateProject(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var deleted = await _projectAppService.DeleteProject(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}