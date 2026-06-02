using FluentNHibernate.Mapping;
using RoutineCore.Domain.Entities;

namespace RoutineCore.Infrastructure.Mappings
{
    public class PunchMap : ClassMap<Punch>
    {
        public PunchMap()
        {
            Table("punches");
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.EmployeeId).Column("employee_id").Not.Nullable();
            Map(x => x.PunchTime).Column("punch_time").Not.Nullable();
            Map(x => x.Direction).Column("direction").Not.Nullable();
            Map(x => x.DeviceCode).Column("device_code").Not.Nullable();
            Map(x => x.Processed).Column("processed").Not.Nullable();
        }
    }
}
