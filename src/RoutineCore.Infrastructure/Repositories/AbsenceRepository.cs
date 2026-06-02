using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate;
using RoutineCore.Domain.Entities;
using RoutineCore.Domain.Repositories;

namespace RoutineCore.Infrastructure.Repositories
{
    public class AbsenceRepository : NHibernateRepository<Absence>, IAbsenceRepository
    {
        public AbsenceRepository(ISession session) : base(session) { }

        public async Task<IEnumerable<Absence>> GetByEmployeeAsync(Guid employeeId)
        {
            return await Session.QueryOver<Absence>()
                .Where(x => x.EmployeeId == employeeId)
                .ListAsync();
        }

        public async Task<IEnumerable<Absence>> GetPendingApprovalsAsync()
        {
            return await Session.QueryOver<Absence>()
                .Where(x => !x.Authorized)
                .ListAsync();
        }
    }
}
