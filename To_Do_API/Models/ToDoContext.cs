using Microsoft.EntityFrameworkCore;

namespace To_Do_API.Models
{
    public class ToDoContext : DbContext
    {

        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {
                
        }

        
        public DbSet<TodoItem> ToDoItem { get; set; } = null!;



    }
}
