using CollabCore.Contracts.Requests;
using CollabCore.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
}