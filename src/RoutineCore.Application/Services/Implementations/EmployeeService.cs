using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoutineCore.Application.Dtos;
using RoutineCore.Application.Services.Interfaces;
using RoutineCore.Domain.Entities;
using RoutineCore.Domain.Repositories;

namespace RoutineCore.Application.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeDto?> GetByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return null;

            return new EmployeeDto(
                employee.Id,
                employee.Name,
                employee.Email,
                employee.Role,
                employee.EmployeeCode,
                employee.IsActive
            );
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return employees.Select(employee => new EmployeeDto(
                employee.Id,
                employee.Name,
                employee.Email,
                employee.Role,
                employee.EmployeeCode,
                employee.IsActive
            ));
        }

        public async Task<EmployeeDto> CreateAsync(EmployeeDto dto)
        {
            var entity = new Employee
            {
                Name = dto.Name,
                Email = dto.Email,
                Role = dto.Role,
                EmployeeCode = dto.EmployeeCode,
                IsActive = dto.IsActive
            };

            await _employeeRepository.SaveAsync(entity);

            return new EmployeeDto(
                entity.Id,
                entity.Name,
                entity.Email,
                entity.Role,
                entity.EmployeeCode,
                entity.IsActive
            );
        }

        public async Task UpdateAsync(Guid id, EmployeeDto dto)
        {
            var entity = await _employeeRepository.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Employee {id} not found");

            entity.Name = dto.Name;
            entity.Email = dto.Email;
            entity.Role = dto.Role;
            entity.EmployeeCode = dto.EmployeeCode;
            entity.IsActive = dto.IsActive;

            await _employeeRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _employeeRepository.GetByIdAsync(id);
            if (entity != null)
            {
                await _employeeRepository.DeleteAsync(entity);
            }
        }
    }
}
