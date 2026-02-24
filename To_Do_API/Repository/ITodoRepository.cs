using To_Do_API.Models;

namespace To_Do_API.Repository
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> GetAllAsync(long userId);
        Task<TodoItem?> GetByIdAsync(long id, long userId);
        Task<TodoItem> CreateAsync(TodoItem todoItem, CancellationToken ct);
        Task<TodoItem> UpdateAsync(TodoItem todoItem, CancellationToken ct);
        Task DeleteAsync(TodoItem todoItem);
    }
}
