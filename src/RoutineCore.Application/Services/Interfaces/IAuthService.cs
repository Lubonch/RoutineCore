using System.Threading.Tasks;
using RoutineCore.Application.Dtos;

namespace RoutineCore.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse?> AuthenticateAsync(LoginRequest loginRequest);
    }
}
