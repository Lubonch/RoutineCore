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
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<ScheduleDto> CreateAsync(ScheduleDto dto)
        {
            var entity = new Schedule
            {
                EmployeeId = dto.EmployeeId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                ProjectTaskId = dto.ProjectTaskId,
                Status = dto.Status
            };

            await _scheduleRepository.SaveAsync(entity);

            return new ScheduleDto(
                entity.Id,
                entity.EmployeeId,
                entity.StartTime,
                entity.EndTime,
                entity.ProjectTaskId,
                entity.Status
            );
        }

        public async Task<IEnumerable<ScheduleDto>> GetByEmployeeAsync(Guid employeeId)
        {
            var schedules = await _scheduleRepository.GetByEmployeeAsync(employeeId);
            return schedules.Select(x => new ScheduleDto(
                x.Id,
                x.EmployeeId,
                x.StartTime,
                x.EndTime,
                x.ProjectTaskId,
                x.Status
            ));
        }

        public async Task<IEnumerable<ScheduleDto>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            var schedules = await _scheduleRepository.GetByDateRangeAsync(start, end);
            return schedules.Select(x => new ScheduleDto(
                x.Id,
                x.EmployeeId,
                x.StartTime,
                x.EndTime,
                x.ProjectTaskId,
                x.Status
            ));
        }
    }
}
//
