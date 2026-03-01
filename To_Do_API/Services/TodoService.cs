using To_Do_API.Exceptions;
using To_Do_API.Models;
using To_Do_API.Models.ToDoDTO;
using To_Do_API.Repository;

namespace To_Do_API.Services
{
    public class TodoService : ITodoService
    {
        private readonly ILogger<TodoService> _logger;
        private readonly ITodoRepository _todoRepository;

        public TodoService(ILogger<TodoService> logger, ITodoRepository repository)
        {
            _logger = logger;
            _todoRepository = repository;
        }

        public async Task<IEnumerable<TodoItemDTO>> GetAllAsync(long userId)
        {
            _logger.LogInformation("Tüm to-do mesajları getiriliyor. UserId : {userId}", userId);

            IEnumerable<TodoItem> todoItems = await _todoRepository.GetAllAsync(userId);

            return todoItems.Select(t => MapToDto(t));
        }

        
        public async Task<TodoItemDTO> GetByIdAsync(long id, long userId)
        {
            _logger.LogInformation("Todo getiriliyor. To-do Id : {todoId}, UserId : {userId}", id, userId);

            TodoItem? todoItem = await _todoRepository.GetByIdAsync(id, userId);

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

            await _todoRepository.CreateAsync(todoItem, ct);

            _logger.LogInformation("To-Do oluşturuldu. {TodoId}", todoItem.Id);

            return MapToDto(todoItem);
        }

        public async Task<TodoItemDTO> UpdateAsync(long id, UpdateTodoItemRequest updateTodoItemRequest, long userId, CancellationToken ct)
        {
            _logger.LogInformation("Belirtilen to-do yeniden düzenleniyor. To-do Id : {id}, UserId : {userId}", id, userId);
            TodoItem? todoItem = await _todoRepository.GetByIdAsync(id, userId);

            if (todoItem == null)
            {
                _logger.LogWarning("To-Do Item is empty. {todoId}, UserId: {userId}", id, userId);
                throw new NotFoundException(message: $"Girilen Id ile eşleşen bir todo yok. To-Do Id : {id}");
            }

            todoItem.Name = updateTodoItemRequest.Name;
            todoItem.IsComplete = updateTodoItemRequest.IsComplete;

            await _todoRepository.UpdateAsync(todoItem, ct);

            _logger.LogInformation("To-Do Item Updated. {TodoId}", todoItem.Id);

            return MapToDto(todoItem);
        }

        public async Task DeleteAsync(long id , long userId)
        {
            _logger.LogInformation("Silme işlemi başlatıldı. To-Do Id : {todoId}, UserId: {userId}", id, userId);

            TodoItem? todoItemById = await _todoRepository.GetByIdAsync(id, userId);

            if (todoItemById == null)
            {
                _logger.LogWarning("Verilen id de bir todo yok. {todoId}, UserId : {userId}", id, userId);
                throw new NotFoundException("Verilen id de bir todo yok. Silme işlemi başarısız.");
            }

            await _todoRepository.DeleteAsync(todoItemById);

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
