namespace To_Do_API.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt{ get; set; }


        // Navigation property
        public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();

    }
}
