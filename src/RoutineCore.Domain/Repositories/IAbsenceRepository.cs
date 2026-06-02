using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoutineCore.Domain.Entities;

namespace RoutineCore.Domain.Repositories
{
    public interface IAbsenceRepository : IRepository<Absence>
    {
        Task<IEnumerable<Absence>> GetByEmployeeAsync(Guid employeeId);
        Task<IEnumerable<Absence>> GetPendingApprovalsAsync();
    }
}
