using System;
using System.Threading.Tasks;
using RoutineCore.Application.Dtos;
using RoutineCore.Application.Services.Interfaces;
using RoutineCore.Domain.Repositories;

namespace RoutineCore.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IEmployeeRepository employeeRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _employeeRepository = employeeRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponse?> AuthenticateAsync(LoginRequest loginRequest)
        {
            var employee = await _employeeRepository.GetByEmailAsync(loginRequest.Email);
            if (employee == null || employee.EmployeeCode != loginRequest.EmployeeCode || !employee.IsActive)
            {
                return null;
            }

            var dto = new EmployeeDto(
                employee.Id,
                employee.Name,
                employee.Email,
                employee.Role,
                employee.EmployeeCode,
                employee.IsActive
            );

            var token = _jwtTokenGenerator.GenerateToken(dto);
            return new AuthResponse(token, dto);
        }
    }
}
//
