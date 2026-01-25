using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using To_Do_API.Models;
using To_Do_API.Models.ToDoDTO;

namespace To_Do_API.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItemDTO>> GetAllAsync();
        Task<TodoItemDTO> GetByIdAsync(long id);
        Task<TodoItemDTO> CreateAsync(CreateTodoItemRequest todoRequest, CancellationToken ct);
        Task<TodoItemDTO> UpdateAsync(long id, TodoItemDTO todoItemDto);
        Task DeleteAsync(long id);


    }
}
