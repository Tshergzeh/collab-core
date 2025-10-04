using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CollabCore.Application.Services;
using CollabCore.Contracts.Requests;

namespace CollabCore.Api.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/members")]
    [Authorize]
    public class ProjectMembersController : ControllerBase
    {
        private readonly ProjectMemberAppService _projectMemberAppService;
        private readonly AuthorizationService _authorizationService;

        public ProjectMembersController(ProjectMemberAppService projectMemberAppService,
            AuthorizationService authorizationService)
        {
            _projectMemberAppService = projectMemberAppService;
            _authorizationService = authorizationService;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddProjectMember(
            Guid projectId,
            Guid userId,
            string role = "Contributor")
        {
            var ownerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (ownerIdClaim == null) return Unauthorized("User ID not found in token.");
            var ownerId = Guid.Parse(ownerIdClaim.Value);

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
            var ownerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (ownerIdClaim == null) return Unauthorized("User ID not found in token.");
            var ownerId = Guid.Parse(ownerIdClaim.Value);

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