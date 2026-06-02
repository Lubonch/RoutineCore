using System.Threading.Tasks;
using NHibernate;
using RoutineCore.Domain.Entities;
using RoutineCore.Domain.Repositories;

namespace RoutineCore.Infrastructure.Repositories
{
    public class EmployeeRepository : NHibernateRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ISession session) : base(session) { }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await Session.QueryOver<Employee>()
                .Where(x => x.Email == email)
                .SingleOrDefaultAsync();
        }

        public async Task<Employee?> GetByCodeAsync(string code)
        {
            return await Session.QueryOver<Employee>()
                .Where(x => x.EmployeeCode == code)
                .SingleOrDefaultAsync();
        }
    }
}
