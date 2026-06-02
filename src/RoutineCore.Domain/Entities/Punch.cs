using System;

namespace RoutineCore.Domain.Entities
{
    public class Punch
    {
        public virtual Guid Id { get; set; }
        public virtual Guid EmployeeId { get; set; }
        public virtual DateTime PunchTime { get; set; }
        public virtual string Direction { get; set; } = "In"; // In, Out
        public virtual string DeviceCode { get; set; } = "Web";
        public virtual bool Processed { get; set; } = false;
    }
}
