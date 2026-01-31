namespace To_Do_API.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public long UserId { get; set; } 
        public string? Name { get; set; }
        public bool IsComplete {  get; set; }
        public string? Secret {  get; set; }
        public int? Age { get; set; }


    }
}
