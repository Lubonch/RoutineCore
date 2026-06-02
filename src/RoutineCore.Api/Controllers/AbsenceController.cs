using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoutineCore.Application.Dtos;
using RoutineCore.Application.Services.Interfaces;

namespace RoutineCore.Api.Controllers
{
    [ApiController]
    [Route("api/v1/absences")]
    [Authorize]
    public class AbsenceController : ControllerBase
    {
        private readonly IAbsenceService _absenceService;

        public AbsenceController(IAbsenceService absenceService)
        {
            _absenceService = absenceService;
        }

        [HttpPost]
        public async Task<IActionResult> RequestAbsence([FromBody] AbsenceDto dto)
        {
            var result = await _absenceService.RequestAbsenceAsync(dto);
            return Ok(result);
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var approverEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "admin@routinecore.com";
            try
            {
                await _absenceService.ApproveAbsenceAsync(id, approverEmail);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetPending()
        {
            var list = await _absenceService.GetPendingApprovalsAsync();
            return Ok(list);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployee(Guid employeeId)
        {
            var list = await _absenceService.GetByEmployeeAsync(employeeId);
            return Ok(list);
        }
    }
}
//
