using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoutineCore.Application.Dtos;

namespace RoutineCore.Application.Services.Interfaces
{
    public interface IPunchService
    {
        Task<PunchDto> RegisterPunchAsync(Guid employeeId, string direction, string deviceCode);
        Task<IEnumerable<PunchDto>> GetUnprocessedAsync();
        Task ProcessPunchesAsync();
    }
}
