using FluentNHibernate.Mapping;
using RoutineCore.Domain.Entities;

namespace RoutineCore.Infrastructure.Mappings
{
    public class ProjectTaskMap : ClassMap<ProjectTask>
    {
        public ProjectTaskMap()
        {
            Table("project_tasks");
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.Description).Column("description").Not.Nullable();
            Map(x => x.ProjectCode).Column("project_code").Not.Nullable();
            Map(x => x.IsActive).Column("is_active").Not.Nullable();
        }
    }
}
