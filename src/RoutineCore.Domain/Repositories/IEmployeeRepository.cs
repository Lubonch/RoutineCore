using System.Threading.Tasks;
using RoutineCore.Domain.Entities;

namespace RoutineCore.Domain.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> GetByEmailAsync(string email);
        Task<Employee?> GetByCodeAsync(string code);
    }
}
