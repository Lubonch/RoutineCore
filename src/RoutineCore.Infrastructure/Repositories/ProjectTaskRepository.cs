using System.Threading.Tasks;
using NHibernate;
using RoutineCore.Domain.Entities;
using RoutineCore.Domain.Repositories;

namespace RoutineCore.Infrastructure.Repositories
{
    public class ProjectTaskRepository : NHibernateRepository<ProjectTask>, IProjectTaskRepository
    {
        public ProjectTaskRepository(ISession session) : base(session) { }

        public async Task<ProjectTask?> GetByCodeAsync(string projectCode)
        {
            return await Session.QueryOver<ProjectTask>()
                .Where(x => x.ProjectCode == projectCode)
                .SingleOrDefaultAsync();
        }
    }
}
