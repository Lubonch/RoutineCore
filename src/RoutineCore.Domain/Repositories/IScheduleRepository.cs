using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoutineCore.Domain.Entities;

namespace RoutineCore.Domain.Repositories
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<IEnumerable<Schedule>> GetByEmployeeAsync(Guid employeeId);
        Task<IEnumerable<Schedule>> GetByDateRangeAsync(DateTime start, DateTime end);
    }
}
