using System.ComponentModel;

namespace To_Do_API.Models.ToDoDTO
{
    public class TodoItemDTO
    {
        public long Id { get; set; }

        [Description("Kullanıcı gizli tutmak istediği todo itemını boş bırakabilmeli")]
        public string? Name { get; set; }
        public bool IsComplete { get; set; }

    }
}
