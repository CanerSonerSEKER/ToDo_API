using Microsoft.EntityFrameworkCore;
using To_Do_API.Exceptions;
using To_Do_API.Models;
using To_Do_API.Models.ToDoDTO;

namespace To_Do_API.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ToDoContext _context;
        private readonly ILogger _logger;

        public TodoRepository(ToDoContext context, ILogger logger)
        {
            _logger = logger;
            _context = context;            
        }

        public async Task<IEnumerable<TodoItemDTO>> GetAllAsync(long userId)
        {
            _logger.LogInformation("Tüm To-Do mesajları getiriliyor. User Id : {UserId}", userId);

            return await _context.ToDoItems.Where(u => u.UserId == userId).Select(u => MapTodoDTO(u)).ToListAsync();
        }

        public async Task<TodoItemDTO> GetByIdAsync(long id, long userId)
        {
            _logger.LogInformation("To-do getiriliyor. TodoId : {todoId} , UserId : {UserId}", id, userId);

            TodoItem? todoItem = await _context.ToDoItems
                .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

            if (todoItem == null)
            {
                throw new NotFoundException($"Kullanıcıya ait todo bulunamadı veya Kullanıcı bulunamadı. Todo Id : {id}");
            }

            _logger.LogInformation("Todo başarıyla getirildi. TodoId : {todoId} , UserId : {userId}", id, userId);

            return MapTodoDTO(todoItem);
        }

        public async Task<TodoItemDTO> CreateAsync(CreateTodoItemRequest createRequest, long userId, CancellationToken ct)
        {
            _logger.LogInformation("Todo oluşturma işlemi başlatılıyor. UserId : {userId}", userId);

            TodoItem todoItem = new TodoItem
            {
                UserId = userId,
                Name = createRequest.Name,
                IsComplete =  false
            };

            await _context.AddAsync(todoItem,ct);
            _logger.LogInformation("Todo eklendi. UserId : {userId}", userId);

            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Todo Kaydedildi. UserId :{userId}", userId);

            TodoItemDTO todoItemDTO = MapTodoDTO(todoItem);

            return todoItemDTO;
        }

        public async Task<TodoItemDTO> UpdateAsync(long id, long userId, UpdateTodoItemRequest updateRequest, CancellationToken ct)
        {
            _logger.LogInformation("Todo Item güncelleme işlemi başlatılıyor. TodoId : {todoId}, UserId : {userId}", id, userId);

            TodoItem? todoItem = await _context.ToDoItems
                .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

            if (todoItem == null)
            {
                _logger.LogWarning("Todo Item boş durumda. TodoId : {todoId}, UserId : {userId}", id, userId);
                throw new NotFoundException(message: $"Kullanıcıya ait todo bulunamadı veya kullanıcı bulunamadı.TodoId :  {id}");
            }

            todoItem.Name = updateRequest.Name;
            todoItem.IsComplete = updateRequest.IsComplete;

            _context.ToDoItems.Update(todoItem);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Todolar başarıyla güncellendi. TodoId : {todoId} ", todoItem.Id);

            return MapTodoDTO(todoItem);
        }

        public async Task DeleteAsync(long id, long userId)
        {
            _logger.LogInformation("Silme işlemi başlatıldı. To-Do Id : {todoId}, UserId: {userId}", id, userId);

            TodoItem? todoItemById = await _context.ToDoItems
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

        private TodoItemDTO MapTodoDTO(TodoItem todoItem)
        {
            TodoItemDTO todoItemDTO = new TodoItemDTO
            {
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };

            return todoItemDTO;
        }

    }
}
