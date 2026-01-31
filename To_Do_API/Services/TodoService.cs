using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using To_Do_API.Exceptions;
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
            _logger.LogInformation("Tüm to-do mesajları getiriliyor.");
            return await _context.ToDoItem.Select(x => MapToDto(x)).ToListAsync();  
        }

        
        public async Task<TodoItemDTO> GetByIdAsync(long id)
        {
            TodoItem todoItem = await _context.ToDoItem.FindAsync(id);
            _logger.LogInformation("Todo getiriliyor. To-do Id : {todoId}", id);

            if (todoItem == null)
            {
                throw new NotFoundException("Girilen Id ile eşleşen bir todo yok. {To-Do Id}");
            }


            _logger.LogInformation("Todo başarıyla getirildi. To-do Id: {todoId}", id);
            return MapToDto(todoItem);
        }

        public async Task<TodoItemDTO> CreateAsync(CreateTodoItemRequest todoRequest, CancellationToken ct)
        {
            _logger.LogInformation("To-do oluşturuluyor.");

            TodoItem todoItem = new TodoItem
            {
                Name = todoRequest.Name,
                IsComplete = false
            };

            await _context.ToDoItem.AddAsync(todoItem, ct);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("To-Do oluşturuldu. {TodoId}", todoItem.Id);

            return MapToDto(todoItem);
        }

        public async Task<TodoItemDTO> UpdateAsync(long id, UpdateTodoItemRequest updateTodoItemRequest, CancellationToken ct)
        {
            _logger.LogInformation("Belirtilen to-do yeniden düzenleniyor. To-do Id : {id}", id);
            TodoItem todoItem = await _context.ToDoItem.FindAsync(id);

            if (todoItem == null)
            {
                _logger.LogInformation("To-Do Item is empty. {todoId}", id);
                throw new NotFoundException(message: $"Girilen Id ile eşleşen bir todo yok. To-Do Id : {id}");
            }


            todoItem.Name = updateTodoItemRequest.Name;
            todoItem.IsComplete = updateTodoItemRequest.IsComplete;


            _context.ToDoItem.Update(todoItem);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("To-Do Item Updated. {TodoId}", todoItem.Id);


            return MapToDto(todoItem);

        }

        public async Task DeleteAsync(long id)
        {
            _logger.LogInformation("Silme işlemi başlatıldı. To-Do Id : {todoId}", id);
            TodoItem todoItemById = await _context.ToDoItem.FindAsync(id);

            if (todoItemById == null)
            {
                _logger.LogInformation("Verilen id de bir todo yok. {todoId}", id);
                throw new NotFoundException("Verilen id de bir todo yok. Silme işlemi başarısız.");
            }

            _context.Remove(todoItemById);

            await _context.SaveChangesAsync();
            _logger.LogInformation("Silme işlemi başarıyla tamamlandı. {todoId}", id);
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
