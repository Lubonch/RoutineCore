using System;

namespace RoutineCore.Domain.Entities
{
    public class ProjectTask
    {
        public virtual Guid Id { get; set; }
        public virtual string Description { get; set; } = string.Empty;
        public virtual string ProjectCode { get; set; } = string.Empty;
        public virtual bool IsActive { get; set; } = true;
    }
}
