
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CollabCore.Contracts.Requests;
using CollabCore.Core.Entities;
using CollabCore.Core.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto request) =>
        Ok(await _authService.Register(request));

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserDto request) =>
        Ok(await _authService.Login(request));

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request) =>
        Ok(await _authService.Refresh(request));

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.FindFirstValue(ClaimTypes.Name);
        var role = User.FindFirstValue(ClaimTypes.Role);

        return Ok(new
        {
            Message = "This is a protected endpoint.",
            UserId = userId,
            Username = username,
            Role = role
        });
    }
}