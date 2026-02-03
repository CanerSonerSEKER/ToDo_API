namespace To_Do_API.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete {  get; set; }

        public long UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
