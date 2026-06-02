using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoutineCore.Application.Dtos;

namespace RoutineCore.Application.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<ScheduleDto> CreateAsync(ScheduleDto scheduleDto);
        Task<IEnumerable<ScheduleDto>> GetByEmployeeAsync(Guid employeeId);
        Task<IEnumerable<ScheduleDto>> GetByDateRangeAsync(DateTime start, DateTime end);
    }
}
