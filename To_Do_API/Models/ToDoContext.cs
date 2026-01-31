using Microsoft.EntityFrameworkCore;
using To_Do_API.Models.ToDoDTO;

namespace To_Do_API.Models
{
    public class ToDoContext : DbContext
    {

        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {
                
        }

        
        public DbSet<TodoItem> ToDoItem { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;




    }
}
