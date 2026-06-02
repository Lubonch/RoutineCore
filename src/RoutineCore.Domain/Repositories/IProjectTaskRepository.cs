using System.Threading.Tasks;
using RoutineCore.Domain.Entities;

namespace RoutineCore.Domain.Repositories
{
    public interface IProjectTaskRepository : IRepository<ProjectTask>
    {
        Task<ProjectTask?> GetByCodeAsync(string projectCode);
    }
}
