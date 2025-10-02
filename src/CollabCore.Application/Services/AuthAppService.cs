using CollabCore.Contracts.Requests;
using CollabCore.Contracts.Responses;
using CollabCore.Core.Models;
using CollabCore.Core.Interfaces;

namespace CollabCore.Application.Services
{
    public class AuthAppService
    {
        private readonly IAuthService _authService;

        public AuthAppService(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResponse> Register(RegisterDto dto)
        {
            var result = await _authService.Register(new UserRegistration
            {
                Username = dto.Username,
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.Role
            });

            return new AuthResponse
            {
                Success = result.Success,
                Token = result.Token ?? string.Empty,
                RefreshToken = result.RefreshToken ?? string.Empty,
                Message = result.Message ?? string.Empty
            };
        }

        public async Task<AuthResponse> Login(LoginDto dto)
        {
            var result = await _authService.Login(new UserLogin
            {
                Username = dto.Username,
                Password = dto.Password
            });

            return new AuthResponse
            {
                Success = result.Success,
                Token = result.Token ?? string.Empty,
                RefreshToken = result.RefreshToken ?? string.Empty,
                Message = result.Message ?? string.Empty
            };
        }

        public async Task<AuthResponse> Refresh(RefreshTokenRequest dto)
        {
            var result = await _authService.Refresh(dto.RefreshToken);

            return new AuthResponse
            {
                Success = result.Success,
                Token = result.Token ?? string.Empty,
                RefreshToken = result.RefreshToken ?? string.Empty,
                Message = result.Message ?? string.Empty
            };
        }
    }
}