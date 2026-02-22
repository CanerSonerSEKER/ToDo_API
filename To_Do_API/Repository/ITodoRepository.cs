using To_Do_API.Models.ToDoDTO;

namespace To_Do_API.Repository
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItemDTO>> GetAllAsync(long userId);
        Task<TodoItemDTO> GetByIdAsync(long id, long userId);
        Task<TodoItemDTO> CreateAsync(CreateTodoItemRequest createRequest, long userId, CancellationToken ct);
        Task<TodoItemDTO> UpdateAsync(long id, long userId, UpdateTodoItemRequest updateRequest, CancellationToken ct);
        Task DeleteAsync(long id, long userId);

    }
}
