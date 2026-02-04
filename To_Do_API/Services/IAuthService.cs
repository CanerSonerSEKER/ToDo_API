using To_Do_API.Models;
using To_Do_API.Models.ToDoDTO.User.Auth;

namespace To_Do_API.Services
{
    public interface IAuthService
    {

        Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequest);

        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequest);


    }
}
