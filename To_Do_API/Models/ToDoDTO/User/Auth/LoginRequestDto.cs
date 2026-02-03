using System.ComponentModel.DataAnnotations;

namespace To_Do_API.Models.ToDoDTO.User.Auth
{
    public class LoginRequestDto
    {

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
