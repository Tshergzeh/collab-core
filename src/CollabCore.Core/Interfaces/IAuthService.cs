using CollabCore.Contracts.Requests;
using CollabCore.Contracts.Responses;

namespace CollabCore.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> Register(UserDto request);
        Task<AuthResponse> Login(UserDto request);
        Task<AuthResponse> Refresh(RefreshTokenRequest request);
    }
}
