using System;

namespace RoutineCore.Domain.Contracts
{
    public record PunchRegisteredEvent(
        Guid PunchId,
        Guid EmployeeId,
        DateTime PunchTime,
        string Direction,
        string DeviceCode
    );

    public record AbsenceApprovedEvent(
        Guid AbsenceId,
        Guid EmployeeId,
        DateTime StartDate,
        DateTime EndDate,
        string Reason,
        string ApprovedBy
    );
}
