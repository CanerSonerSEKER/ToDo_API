using System.ComponentModel.DataAnnotations;

namespace To_Do_API.Models.ToDoDTO.UserDTO
{
    public class UserRegisterRequestDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        public string Address { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public DateOnly Birth { get; set; }



    }
}
