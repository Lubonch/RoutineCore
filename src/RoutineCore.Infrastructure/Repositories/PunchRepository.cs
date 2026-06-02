using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate;
using RoutineCore.Domain.Entities;
using RoutineCore.Domain.Repositories;

namespace RoutineCore.Infrastructure.Repositories
{
    public class PunchRepository : NHibernateRepository<Punch>, IPunchRepository
    {
        public PunchRepository(ISession session) : base(session) { }

        public async Task<IEnumerable<Punch>> GetUnprocessedPunchesAsync()
        {
            return await Session.QueryOver<Punch>()
                .Where(x => !x.Processed)
                .ListAsync();
        }

        public async Task<IEnumerable<Punch>> GetByEmployeeAsync(Guid employeeId)
        {
            return await Session.QueryOver<Punch>()
                .Where(x => x.EmployeeId == employeeId)
                .ListAsync();
        }
    }
}
