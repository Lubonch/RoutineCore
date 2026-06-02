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
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly IProjectTaskRepository _projectTaskRepository;

        public ProjectTaskService(IProjectTaskRepository projectTaskRepository)
        {
            _projectTaskRepository = projectTaskRepository;
        }

        public async Task<ProjectTaskDto> CreateAsync(ProjectTaskDto dto)
        {
            var entity = new ProjectTask
            {
                Description = dto.Description,
                ProjectCode = dto.ProjectCode,
                IsActive = dto.IsActive
            };

            await _projectTaskRepository.SaveAsync(entity);

            return new ProjectTaskDto(
                entity.Id,
                entity.Description,
                entity.ProjectCode,
                entity.IsActive
            );
        }

        public async Task<IEnumerable<ProjectTaskDto>> GetAllAsync()
        {
            var list = await _projectTaskRepository.GetAllAsync();
            return list.Select(x => new ProjectTaskDto(
                x.Id,
                x.Description,
                x.ProjectCode,
                x.IsActive
            ));
        }
    }
}
