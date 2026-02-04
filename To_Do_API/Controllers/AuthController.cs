using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using To_Do_API.Models;
using To_Do_API.Models.ToDoDTO.User.Auth;
using To_Do_API.Services;

namespace To_Do_API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto registerRequest)
        {
            AuthResponseDto request = await _authService.RegisterAsync(registerRequest);

            // return CreatedAtAction(nameof(GetUserById), new {id = request.UserId }, request);

            return Ok(request);
        }


        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto loginRequest)
        {
            AuthResponseDto request = await _authService.LoginAsync(loginRequest);


            return Ok(request);
        }


    }
}
