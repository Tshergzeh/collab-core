
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CollabCore.Contracts.Requests;
using CollabCore.Contracts.Responses;
using CollabCore.Application.Services;

namespace CollabCore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthAppService _authAppService;

        public AuthController(AuthAppService authAppService)
        {
            _authAppService = authAppService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(
            [FromBody] RegisterDto request)
        {
            var response = await _authAppService.Register(request);

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(
            [FromBody] LoginDto request)
        {
            var response = await _authAppService.Login(request);
            if (!response.Success) return Unauthorized(response);
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponse>> Refresh(
            [FromBody] RefreshTokenRequest request)
        {
            var response = await _authAppService.Refresh(request);
            if (!response.Success) return Unauthorized(response);
            return Ok(response);
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetMe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                UserId = userId,
                Username = username,
                Role = role
            });
        }
    }
}