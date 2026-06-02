using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoutineCore.Application.Dtos;
using RoutineCore.Application.Services.Interfaces;

namespace RoutineCore.Api.Controllers
{
    [ApiController]
    [Route("api/v1/tasks")]
    [Authorize]
    public class ProjectTaskController : ControllerBase
    {
        private readonly IProjectTaskService _projectTaskService;

        public ProjectTaskController(IProjectTaskService projectTaskService)
        {
            _projectTaskService = projectTaskService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([FromBody] ProjectTaskDto dto)
        {
            var result = await _projectTaskService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _projectTaskService.GetAllAsync();
            return Ok(list);
        }
    }
}
