using System;

namespace RoutineCore.Domain.Entities
{
    public class Employee
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; } = string.Empty;
        public virtual string Email { get; set; } = string.Empty;
        public virtual string Role { get; set; } = "Operator";
        public virtual string EmployeeCode { get; set; } = string.Empty;
        public virtual bool IsActive { get; set; } = true;
    }
}
