using CollabCore.Contracts.Requests;
using CollabCore.Contracts.Responses;

namespace CollabCore.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> Register(RegisterDto request);
        Task<AuthResponse> Login(LoginDto request);
        Task<AuthResponse> Refresh(RefreshTokenRequest request);
    }
}
