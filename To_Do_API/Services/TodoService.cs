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

        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetAllAsync()
        {
            return await _context.ToDoItem.Select(x => MapToDto(x)).ToListAsync(); // Yani önce dto yu getirip sonrasında listeliyoruz
        }

        
        public async Task<ActionResult<TodoItemDTO>> GetByIdAsync(long id)
        {
            TodoItem todoItem = await _context.ToDoItem.FirstOrDefaultAsync(x => x.Id == id);

            if (todoItem == null)
            {
                _logger.LogInformation("To-Do Item is empty {todoId}", todoItem.Id);
            }


            return MapToDto(todoItem);
        }

        public async Task<ActionResult<TodoItemDTO>> CreateAsync(CreateTodoItemRequest todoRequest, CancellationToken ct)
        {
            TodoItem todoItem = new TodoItem
            {
                Name = todoRequest.Name,
                IsComplete = false
            };

            await _context.ToDoItem.AddAsync(todoItem, ct);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("To-Do created. {TodoId}", todoItem.Id);

            //return new TodoItemDTO
            //{
            //    Id = todoItem.Id,
            //    Name = todoItem.Name,
            //    IsComplete = todoItem.IsComplete
            //};

            return MapToDto(todoItem);
        }



        public async Task DeleteAsync(long id)
        {
            TodoItem todoItemById = await _context.ToDoItem.FindAsync(id);

            _context.Remove(todoItemById);

            await _context.SaveChangesAsync();

        }

        public async Task<ActionResult<TodoItemDTO>> UpdateAsync(long id, UpdateTodoItemRequest updateTodoItemRequest, CancellationToken ct)
        {
            TodoItem todoItem = await _context.ToDoItem.FirstOrDefaultAsync(x => x.Id == id);

            if (todoItem == null)
            {
                _logger.LogInformation("Empty To-Do {TodoId}", todoItem.Id);
            }


            TodoItem updateTodo = new TodoItem
            {
                Name = updateTodoItemRequest.Name,
                IsComplete = updateTodoItemRequest.IsComplete
            };

            _context.ToDoItem.Update(updateTodo);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("To-Do Item Updated. {TodoId}", updateTodo.Id);


            return MapToDto(updateTodo);

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
