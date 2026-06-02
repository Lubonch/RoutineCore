using RoutineCore.Application.Dtos;

namespace RoutineCore.Application.Services.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(EmployeeDto employee);
    }
}
