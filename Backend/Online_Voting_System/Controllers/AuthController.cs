using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Voting_System.Application.Services;
using Online_Voting_System.Domain.Dtos;

namespace Online_Voting_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.Register(dto, "User");
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.Login(dto);
            if (!token.Success)
            {
                return BadRequest(token);
            }
            return Ok(token);
        }
    }
}
