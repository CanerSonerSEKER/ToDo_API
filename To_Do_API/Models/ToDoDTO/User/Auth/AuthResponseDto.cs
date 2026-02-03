namespace To_Do_API.Models.ToDoDTO.User.Auth
{
    public class AuthResponseDto
    {
        public long UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Token {get; set; } = string.Empty;
    }
}
