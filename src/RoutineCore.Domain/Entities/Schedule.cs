using System;

namespace RoutineCore.Domain.Entities
{
    public class Schedule
    {
        public virtual Guid Id { get; set; }
        public virtual Guid EmployeeId { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime EndTime { get; set; }
        public virtual Guid ProjectTaskId { get; set; }
        public virtual string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled
    }
}
