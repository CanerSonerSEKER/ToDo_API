using System.ComponentModel.DataAnnotations;

namespace To_Do_API.Models.ToDoDTO.UserDTO
{
    public class UserLoginRequestDto
    {

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
