using To_Do_API.Models.ToDoDTO;

namespace To_Do_API.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItemDTO>> GetAllAsync(long userId);
        Task<TodoItemDTO> GetByIdAsync(long id, long userId);
        Task<TodoItemDTO> CreateAsync(CreateTodoItemRequest todoRequest, long userId,  CancellationToken ct);
        Task<TodoItemDTO> UpdateAsync(long id, UpdateTodoItemRequest updateTodoRequest, long userId, CancellationToken ct);
        Task DeleteAsync(long id, long userId);

    }
}
