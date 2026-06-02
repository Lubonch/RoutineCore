using FluentNHibernate.Mapping;
using RoutineCore.Domain.Entities;

namespace RoutineCore.Infrastructure.Mappings
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("employees");
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.Name).Column("name").Not.Nullable();
            Map(x => x.Email).Column("email").Not.Nullable();
            Map(x => x.Role).Column("role").Not.Nullable();
            Map(x => x.EmployeeCode).Column("employee_code").Not.Nullable();
            Map(x => x.IsActive).Column("is_active").Not.Nullable();
        }
    }
}
