namespace To_Do_API.Models.ToDoDTO.User.Auth
{
    public class AuthResponseDto
    {
        public string UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Token {get; set; } = string.Empty;
    }
}
