using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoutineCore.Application.Dtos;

namespace RoutineCore.Application.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto> CreateAsync(EmployeeDto employeeDto);
        Task UpdateAsync(Guid id, EmployeeDto employeeDto);
        Task DeleteAsync(Guid id);
    }
}
