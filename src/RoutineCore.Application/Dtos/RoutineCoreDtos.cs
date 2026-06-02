using System;

namespace RoutineCore.Application.Dtos
{
    public record EmployeeDto(
        Guid Id, 
        string Name, 
        string Email, 
        string Role, 
        string EmployeeCode, 
        bool IsActive
    );

    public record PunchDto(
        Guid Id, 
        Guid EmployeeId, 
        DateTime PunchTime, 
        string Direction, 
        string DeviceCode, 
        bool Processed
    );

    public record ScheduleDto(
        Guid Id, 
        Guid EmployeeId, 
        DateTime StartTime, 
        DateTime EndTime, 
        Guid ProjectTaskId, 
        string Status
    );

    public record AbsenceDto(
        Guid Id, 
        Guid EmployeeId, 
        DateTime StartDate, 
        DateTime EndDate, 
        string Reason, 
        bool Authorized, 
        string? ApprovedBy
    );

    public record ProjectTaskDto(
        Guid Id, 
        string Description, 
        string ProjectCode, 
        bool IsActive
    );

    public record LoginRequest(
        string Email, 
        string EmployeeCode
    );

    public record AuthResponse(
        string Token, 
        EmployeeDto Employee
    );
}
