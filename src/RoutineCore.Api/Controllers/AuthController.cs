using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoutineCore.Application.Dtos;
using RoutineCore.Application.Services.Interfaces;

namespace RoutineCore.Api.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.AuthenticateAsync(request);
            if (response == null)
            {
                return Unauthorized(new { message = "Invalid email or employee code." });
            }

            return Ok(response);
        }
    }
}
