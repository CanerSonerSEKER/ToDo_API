using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using To_Do_API.Models;
using To_Do_API.Models.ToDoDTO;

namespace To_Do_API.Services
{
    public class TodoService : ITodoService 
    {
        private readonly ILogger<TodoService> _logger;
        private readonly ToDoContext _context;

        public TodoService(ILogger<TodoService> logger, ToDoContext context)
        {
            _logger = logger;
            _context = context;
        }


        public async Task<ActionResult<TodoItemDTO>> CreateAsync(TodoItemDTO todoItem)
        {
            TodoItem todoDto = new TodoItem
            {
                Id = GenerateId(),
                Name = todoItem.Name,
                IsComplete = false
            };

            await _context.ToDoItem.AddAsync(todoDto);
            _logger.LogInformation($"To-Do created. {todoDto.Id}", todoDto.Id);

            return MapToDto(todoDto);
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<IEnumerable<TodoItemDTO>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<TodoItemDTO>> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<TodoItemDTO>> UpdateAsync(long id, TodoItemDTO todoItemDto)
        {
            throw new NotImplementedException();
        }


        // Independent Methods

        private long GenerateId()
        {
            var todoItem = _context.ToDoItem.ToList();

            if (todoItem.Any())
            {
                return todoItem.Max(t => t.Id) + 1;
            }
            else
            {
                return 1;
            }
        }

        private TodoItemDTO MapToDto(TodoItem todoItem)
        {
            var mapTodoDto = new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete,
            };

            return mapTodoDto;
        }



    }
}
