using System;

namespace RoutineCore.Domain.Entities
{
    public class Absence
    {
        public virtual Guid Id { get; set; }
        public virtual Guid EmployeeId { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual string Reason { get; set; } = string.Empty;
        public virtual bool Authorized { get; set; } = false;
        public virtual string? ApprovedBy { get; set; }
    }
}
