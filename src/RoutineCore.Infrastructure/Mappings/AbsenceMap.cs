using FluentNHibernate.Mapping;
using RoutineCore.Domain.Entities;

namespace RoutineCore.Infrastructure.Mappings
{
    public class AbsenceMap : ClassMap<Absence>
    {
        public AbsenceMap()
        {
            Table("absences");
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.EmployeeId).Column("employee_id").Not.Nullable();
            Map(x => x.StartDate).Column("start_date").Not.Nullable();
            Map(x => x.EndDate).Column("end_date").Not.Nullable();
            Map(x => x.Reason).Column("reason").Not.Nullable();
            Map(x => x.Authorized).Column("authorized").Not.Nullable();
            Map(x => x.ApprovedBy).Column("approved_by").Nullable();
        }
    }
}
