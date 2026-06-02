using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate;
using RoutineCore.Domain.Entities;
using RoutineCore.Domain.Repositories;

namespace RoutineCore.Infrastructure.Repositories
{
    public class ScheduleRepository : NHibernateRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(ISession session) : base(session) { }

        public async Task<IEnumerable<Schedule>> GetByEmployeeAsync(Guid employeeId)
        {
            return await Session.QueryOver<Schedule>()
                .Where(x => x.EmployeeId == employeeId)
                .ListAsync();
        }

        public async Task<IEnumerable<Schedule>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return await Session.QueryOver<Schedule>()
                .Where(x => x.StartTime >= start && x.EndTime <= end)
                .ListAsync();
        }
    }
}
