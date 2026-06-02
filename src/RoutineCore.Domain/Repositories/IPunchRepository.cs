using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoutineCore.Domain.Entities;

namespace RoutineCore.Domain.Repositories
{
    public interface IPunchRepository : IRepository<Punch>
    {
        Task<IEnumerable<Punch>> GetUnprocessedPunchesAsync();
        Task<IEnumerable<Punch>> GetByEmployeeAsync(Guid employeeId);
    }
}
