using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CollabCore.Application.Services;
using CollabCore.Contracts.Requests;
using CollabCore.Infrastructure.Services;

namespace CollabCore.Api.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/members")]
    [Authorize]
    public class ProjectMembersController : ControllerBase
    {
        private readonly ProjectMemberAppService _projectMemberAppService;
        private readonly AuthorizationService _authorizationService;
        private readonly CurrentUserService _currentUser;

        public ProjectMembersController(
            ProjectMemberAppService projectMemberAppService,
            AuthorizationService authorizationService,
            CurrentUserService currentUser)
        {
            _projectMemberAppService = projectMemberAppService;
            _authorizationService = authorizationService;
            _currentUser = currentUser;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddProjectMember(
            Guid projectId,
            Guid userId,
            string role = "Contributor")
        {
            var ownerId = _currentUser.GetCurrentUserId();

            if (!await _authorizationService.IsProjectOwnerAsync(projectId, ownerId))
                return Forbid();

            await _projectMemberAppService.AddProjectMemberAsync(projectId, userId, role);

            return CreatedAtAction(
                nameof(GetProjectMembers),
                new { projectId },
                new { projectId, userId }
            );
        }

        [HttpPut("{userId}/role")]
        public async Task<IActionResult> UpdateMemberRole(
            Guid projectId,
            Guid userId,
            [FromBody] UpdateProjectMemberRoleRequest request)
        {
            if (userId != request.UserId) return BadRequest("UserId mismatch");

            try
            {
                var result = await _projectMemberAppService.UpdateMemberRoleAsync(projectId, request);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> RemoveProjectMember(Guid projectId, Guid userId)
        {
            var ownerId = _currentUser.GetCurrentUserId();

            if (!await _authorizationService.IsProjectOwnerAsync(projectId, ownerId))
                return Forbid();
                
            await _projectMemberAppService.RemoveProjectMemberAsync(projectId, userId);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectMembers(Guid projectId)
        {
            var members = await _projectMemberAppService.GetProjectMembersAsync(projectId);
            return Ok(members);
        }
    }
}