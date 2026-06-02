using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoutineCore.Application.Services.Interfaces;

namespace RoutineCore.Api.Controllers
{
    [ApiController]
    [Route("api/v1/punches")]
    [Authorize]
    public class PunchController : ControllerBase
    {
        private readonly IPunchService _punchService;

        public PunchController(IPunchService punchService)
        {
            _punchService = punchService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterPunchRequest request)
        {
            var result = await _punchService.RegisterPunchAsync(request.EmployeeId, request.Direction, request.DeviceCode);
            return Ok(result);
        }

        [HttpGet("unprocessed")]
        public async Task<IActionResult> GetUnprocessed()
        {
            var list = await _punchService.GetUnprocessedAsync();
            return Ok(list);
        }

        [HttpPost("process")]
        public async Task<IActionResult> Process()
        {
            await _punchService.ProcessPunchesAsync();
            return Ok(new { message = "Punches processed successfully." });
        }
    }

    public record RegisterPunchRequest(Guid EmployeeId, string Direction, string DeviceCode);
}
