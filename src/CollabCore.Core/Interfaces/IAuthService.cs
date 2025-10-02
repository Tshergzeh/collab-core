using CollabCore.Core.Models;

namespace CollabCore.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> Register(UserRegistration registration);
        Task<AuthResult> Login(UserLogin login);
        Task<AuthResult> Refresh(string refreshToken);
    }
}
