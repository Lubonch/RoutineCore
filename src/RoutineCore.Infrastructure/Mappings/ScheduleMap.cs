using FluentNHibernate.Mapping;
using RoutineCore.Domain.Entities;

namespace RoutineCore.Infrastructure.Mappings
{
    public class ScheduleMap : ClassMap<Schedule>
    {
        public ScheduleMap()
        {
            Table("schedules");
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.EmployeeId).Column("employee_id").Not.Nullable();
            Map(x => x.StartTime).Column("start_time").Not.Nullable();
            Map(x => x.EndTime).Column("end_time").Not.Nullable();
            Map(x => x.ProjectTaskId).Column("project_task_id").Not.Nullable();
            Map(x => x.Status).Column("status").Not.Nullable();
        }
    }
}
