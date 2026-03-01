using Microsoft.EntityFrameworkCore;
using To_Do_API.Models;

namespace To_Do_API.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ToDoContext _context;
        private readonly ILogger<TodoRepository> _logger;


        public TodoRepository(ToDoContext context, ILogger<TodoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync(long userId)
        {
            _logger.LogInformation("Tüm To-do'lar getiriliyor. User Id : {userId}", userId);

            return await _context.ToDoItems
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(long id, long userId)
        {
            _logger.LogInformation("Belirtilen kullanıcının todoları getiriliyor. Id : {id}, UserId : {userId}", id, userId);

            return await _context.ToDoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<TodoItem> CreateAsync(TodoItem todoItem, CancellationToken ct)
        {
            _logger.LogInformation("Todo oluşturma işlemi başlatılıyor. ");

            await _context.AddAsync(todoItem, ct);
            await _context.SaveChangesAsync(ct);

            return todoItem;
        }

        public async Task<TodoItem> UpdateAsync(TodoItem todoItem, CancellationToken ct)
        {
            _logger.LogInformation("To-do güncelleme işlemi başlatılıyor.");

            _context.ToDoItems.Update(todoItem);
            await _context.SaveChangesAsync(ct);

            return todoItem;
        }

        public async Task DeleteAsync(TodoItem todoItem)
        {
            _logger.LogInformation("Todo silme işlemi başlatılıyor.");
            _context.Remove(todoItem);

            await _context.SaveChangesAsync();
            _logger.LogInformation("Todo silme işlemi gerçekleştirildi.");

        }

        
    }
}
