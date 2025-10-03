using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetOwnedProjects(Guid id)
        {
            var projects = await _usersAppService.GetOwnedProjects(id);
            return Ok(projects);
        }

        [HttpGet("{id}/assignments")]
        public async Task<IActionResult> GetAssignments(Guid id)
        {
            var assignments = await _usersAppService.GetAssignments(id);
            return Ok(assignments);
        }

        [HttpGet("{id}/project-memberships")]
        public async Task<IActionResult> GetProjectMemberships(Guid id)
        {
            var projectMemberships = await _usersAppService.GetProjectMemberships(id);
            return Ok(projectMemberships);
        }
    }
}