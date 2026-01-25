using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using To_Do_API.Models;
using To_Do_API.Models.ToDoDTO;

namespace To_Do_API.Services
{
    public interface ITodoService
    {
        Task<ActionResult<IEnumerable<TodoItemDTO>>> GetAllAsync();
        Task<ActionResult<TodoItemDTO>> GetByIdAsync(long id);
        Task<ActionResult<TodoItemDTO>> CreateAsync(TodoItemDTO todoItemDto);
        Task<ActionResult<TodoItemDTO>> UpdateAsync(long id, TodoItemDTO todoItemDto);
        Task DeleteAsync(long id);


    }
}
