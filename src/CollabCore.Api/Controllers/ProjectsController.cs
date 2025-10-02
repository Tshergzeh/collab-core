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
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectResponse>>> GetProjects([FromQuery] QueryParameters query)
        {
            var projects = _context.Projects.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                projects = projects.Where(p => p.Name.Contains(query.Search)
                    || (p.Description != null && p.Description.Contains(query.Search)));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                projects = query.SortBy.ToLower() switch
                {
                    "name" => query.SortDesc
                        ? projects.OrderByDescending(p => p.Name)
                        : projects.OrderBy(p => p.Name),
                    "createdat" => query.SortDesc
                        ? projects.OrderByDescending(p => p.CreatedAt)
                        : projects.OrderBy(p => p.CreatedAt),
                    _ => projects.OrderBy(p => p.CreatedAt)
                };
            }
            else
            {
                projects = projects.OrderBy(p => p.CreatedAt);
            }

            var totalItems = await projects.CountAsync();
            var items = await projects
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(p => new ProjectResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    OwnerId = p.OwnerId,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();

            var response = new
            {
                Items = items,
                TotalItems = totalItems,
                Page = query.Page,
                PageSize = query.PageSize
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectResponse>> GetProject(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            return Ok(new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                OwnerId = project.OwnerId,
                CreatedAt = project.CreatedAt,
                ModifiedAt = project.ModifiedAt
            });
        }

        [HttpPost]
        public async Task<ActionResult<ProjectResponse>> CreateProject(ProjectCreateDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("User ID not found in token.");

            var userId = Guid.Parse(userIdClaim.Value);

            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                OwnerId = userId
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                OwnerId = project.OwnerId,
                CreatedAt = project.CreatedAt,
                ModifiedAt = project.ModifiedAt
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, ProjectUpdateDto dto)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                OwnerId = project.OwnerId,
                CreatedAt = project.CreatedAt,
                ModifiedAt = project.ModifiedAt
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}