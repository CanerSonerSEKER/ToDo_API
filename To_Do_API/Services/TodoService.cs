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

        public async Task<IEnumerable<TodoItemDTO>> GetAllAsync(long userId)
        {
            _logger.LogInformation("Tüm to-do mesajları getiriliyor. UserId : {userId}", userId);
            return await _context.ToDoItems
                .Where(t => t.UserId == userId)
                .Select(x => MapToDto(x))
                .ToListAsync();  
        }

        
        public async Task<TodoItemDTO> GetByIdAsync(long id, long userId)
        {
            _logger.LogInformation("Todo getiriliyor. To-do Id : {todoId}, UserId : {userId}", id, userId);
            
            TodoItem? todoItem = await _context.ToDoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);


            if (todoItem == null)
            {
                throw new NotFoundException($"To-Do bulunamadı veya size ait değil. Todo Id : {id}");
            }


            _logger.LogInformation("Todo başarıyla getirildi. To-do Id: {todoId}, UserId : {userId}", id, userId);
            return MapToDto(todoItem);
        }

        public async Task<TodoItemDTO> CreateAsync(CreateTodoItemRequest todoRequest, long userId, CancellationToken ct)
        {
            _logger.LogInformation("To-do oluşturuluyor. UserId : {userId}", userId);

            TodoItem todoItem = new TodoItem
            {
                UserId = userId,
                Name = todoRequest.Name,
                IsComplete = false
            };

            await _context.ToDoItems.AddAsync(todoItem, ct);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("To-Do oluşturuldu. {TodoId}", todoItem.Id);

            return MapToDto(todoItem);
        }

        public async Task<TodoItemDTO> UpdateAsync(long id, UpdateTodoItemRequest updateTodoItemRequest, long userId, CancellationToken ct)
        {
            _logger.LogInformation("Belirtilen to-do yeniden düzenleniyor. To-do Id : {id}, UserId : {userId}", id, userId);
            TodoItem? todoItem = await _context.ToDoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todoItem == null)
            {
                _logger.LogWarning("To-Do Item is empty. {todoId}, UserId: {userId}", id, userId);
                throw new NotFoundException(message: $"Girilen Id ile eşleşen bir todo yok. To-Do Id : {id}");
            }

            todoItem.Name = updateTodoItemRequest.Name;
            todoItem.IsComplete = updateTodoItemRequest.IsComplete;

            _context.ToDoItems.Update(todoItem);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("To-Do Item Updated. {TodoId}", todoItem.Id);

            return MapToDto(todoItem);
        }

        public async Task DeleteAsync(long id , long userId)
        {
            _logger.LogInformation("Silme işlemi başlatıldı. To-Do Id : {todoId}, UserId: {userId}", id, userId);

            TodoItem todoItemById = await _context.ToDoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todoItemById == null)
            {
                _logger.LogWarning("Verilen id de bir todo yok. {todoId}, UserId : {userId}", id, userId);
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
