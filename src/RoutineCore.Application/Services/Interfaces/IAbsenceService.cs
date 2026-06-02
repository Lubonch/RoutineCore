using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoutineCore.Application.Dtos;

namespace RoutineCore.Application.Services.Interfaces
{
    public interface IAbsenceService
    {
        Task<AbsenceDto> RequestAbsenceAsync(AbsenceDto absenceDto);
        Task ApproveAbsenceAsync(Guid id, string approverEmail);
        Task<IEnumerable<AbsenceDto>> GetPendingApprovalsAsync();
        Task<IEnumerable<AbsenceDto>> GetByEmployeeAsync(Guid employeeId);
    }
}
