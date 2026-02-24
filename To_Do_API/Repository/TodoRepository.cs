using Microsoft.EntityFrameworkCore;
using To_Do_API.Models;

namespace To_Do_API.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ToDoContext _context;

        public TodoRepository(ToDoContext context)
        {
            _context = context;            
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync(long userId)
        {
            return await _context.ToDoItems
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(long id, long userId)
        {
            return await _context.ToDoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<TodoItem> CreateAsync(TodoItem todoItem, CancellationToken ct)
        {
            await _context.AddAsync(todoItem, ct);
            await _context.SaveChangesAsync(ct);

            return todoItem;
        }

        public async Task<TodoItem> UpdateAsync(TodoItem todoItem, CancellationToken ct)
        {
            _context.ToDoItems.Update(todoItem);

            await _context.SaveChangesAsync(ct);

            return todoItem;
        }

        public async Task DeleteAsync(TodoItem todoItem)
        {
            _context.Remove(todoItem);

            await _context.SaveChangesAsync();
        }

        
    }
}
