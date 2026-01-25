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

        public async Task<IEnumerable<TodoItemDTO>> GetAllAsync()
        {
            return await _context.ToDoItem.Select(x => MapToDto(x)).ToListAsync(); // Yani önce dto yu getirip sonrasında listeliyoruz
        }

        public Task<TodoItemDTO> GetByIdAsync(long id)
        {

        }

        public async Task<TodoItemDTO> CreateAsync(CreateTodoItemRequest todoRequest, CancellationToken ct)
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



        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        

        

        public Task<TodoItemDTO> UpdateAsync(long id, TodoItemDTO todoItemDto)
        {
            throw new NotImplementedException();
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
