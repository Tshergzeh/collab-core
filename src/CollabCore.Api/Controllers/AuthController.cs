
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerOperation(
            Summary = "Register"
        )]
        public async Task<ActionResult<AuthResponse>> Register(
            [FromBody] RegisterDto request)
        {
            var response = await _authAppService.Register(request);

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Login"
        )]
        public async Task<ActionResult<AuthResponse>> Login(
            [FromBody] LoginDto request)
        {
            var response = await _authAppService.Login(request);
            if (!response.Success) return Unauthorized(response);
            return Ok(response);
        }

        [HttpPost("refresh")]
        [SwaggerOperation(
            Summary = "Refresh token"
        )]
        public async Task<ActionResult<AuthResponse>> Refresh(
            [FromBody] RefreshTokenRequest request)
        {
            var response = await _authAppService.Refresh(request);
            if (!response.Success) return Unauthorized(response);
            return Ok(response);
        }
    }
}