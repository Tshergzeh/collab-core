using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CollabCore.Application.Services;

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
        public async Task<IActionResult> AddProjectMember(Guid projectId, Guid userId)
        {
            var ownerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (ownerIdClaim == null) return Unauthorized("User ID not found in token.");
            var ownerId = Guid.Parse(ownerIdClaim.Value);

            if (!await _authorizationService.IsProjectOwnerAsync(projectId, ownerId))
                return Forbid();

            await _projectMemberAppService.AddProjectMemberAsync(projectId, userId);

            return CreatedAtAction(
                nameof(GetProjectMembers),
                new { projectId },
                new { projectId, userId }
            );
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