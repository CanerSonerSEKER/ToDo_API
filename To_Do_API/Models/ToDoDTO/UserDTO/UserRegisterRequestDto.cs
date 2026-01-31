using System.ComponentModel.DataAnnotations;

namespace To_Do_API.Models.ToDoDTO.UserDTO
{
    public class UserRegisterRequestDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public DateTime CreateAt { get; set; }

    }
}
