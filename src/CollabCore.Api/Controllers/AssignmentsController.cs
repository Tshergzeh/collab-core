using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using CollabCore.Application.Services;
using CollabCore.Contracts.Requests;

namespace CollabCore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssignmentsController : ControllerBase
    {
        private readonly AssignmentsAppService _assignmentsAppService;

        public AssignmentsController(AssignmentsAppService assignmentsAppService)
        {
            _assignmentsAppService = assignmentsAppService;
        }

        [HttpPost("assign")]
        [SwaggerOperation(
            Summary = "Assign tasks to users",
            Description = "Allows project owners and PMs to assign tasks to project members"
        )]
        public async Task<IActionResult> AssignTask(
            [FromBody] AssignTaskRequest request)
        {
            try
            {
                await _assignmentsAppService.AssignTaskAsync(request);
                return Ok(new { message = "Task assigned successfully" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}