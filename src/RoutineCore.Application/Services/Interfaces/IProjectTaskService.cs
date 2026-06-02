using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoutineCore.Application.Dtos;

namespace RoutineCore.Application.Services.Interfaces
{
    public interface IProjectTaskService
    {
        Task<ProjectTaskDto> CreateAsync(ProjectTaskDto taskDto);
        Task<IEnumerable<ProjectTaskDto>> GetAllAsync();
    }
}
