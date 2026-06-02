using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoutineCore.Application.Dtos;
using RoutineCore.Application.Services.Interfaces;

namespace RoutineCore.Api.Controllers
{
    [ApiController]
    [Route("api/v1/schedules")]
    [Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ScheduleDto dto)
        {
            var result = await _scheduleService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployee(Guid employeeId)
        {
            var list = await _scheduleService.GetByEmployeeAsync(employeeId);
            return Ok(list);
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetByRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var list = await _scheduleService.GetByDateRangeAsync(start, end);
            return Ok(list);
        }
    }
}
//
