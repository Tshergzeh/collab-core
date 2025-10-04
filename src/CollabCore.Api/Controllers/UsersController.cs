using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using CollabCore.Infrastructure.Data;
using CollabCore.Contracts.Responses;
using CollabCore.Application.Services;

namespace CollabCore.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UsersAppService _usersAppService;

        public UsersController(UsersAppService usersAppService)
        {
            _usersAppService = usersAppService;
        }

        [HttpGet("{id}/owned-projects")]
        [SwaggerOperation(
            Summary = "Get all projects owned by a user"
        )]
        public async Task<IActionResult> GetOwnedProjects(Guid id)
        {
            var projects = await _usersAppService.GetOwnedProjects(id);
            return Ok(projects);
        }

        [HttpGet("{id}/assignments")]
        [SwaggerOperation(
            Summary = "Get all tasks assigned to a user"
        )]
        public async Task<IActionResult> GetAssignments(Guid id)
        {
            var assignments = await _usersAppService.GetAssignments(id);
            return Ok(assignments);
        }

        [HttpGet("{id}/project-memberships")]
        [SwaggerOperation(
            Summary = "Get all projects a user is assigned to"
        )]
        public async Task<IActionResult> GetProjectMemberships(Guid id)
        {
            var projectMemberships = await _usersAppService.GetProjectMemberships(id);
            return Ok(projectMemberships);
        }
    }
}