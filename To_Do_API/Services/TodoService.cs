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
            return await _todoRepository.GetAllAsync(userId);  
        }

        
        public async Task<TodoItemDTO> GetByIdAsync(long id, long userId)
        {
            TodoItemDTO todoItemDTO = await _todoRepository.GetByIdAsync(id, userId);

            return todoItemDTO;
        }

        public async Task<TodoItemDTO> CreateAsync(CreateTodoItemRequest todoRequest, long userId, CancellationToken ct)
        {
            TodoItemDTO todoItemDTO = await _todoRepository.CreateAsync(todoRequest, userId, ct);

            return todoItemDTO;
        }

        public async Task<TodoItemDTO> UpdateAsync(long id, UpdateTodoItemRequest updateTodoItemRequest, long userId, CancellationToken ct)
        {
            TodoItemDTO todoItemDTO = await _todoRepository.UpdateAsync(id, userId, updateTodoItemRequest, ct);

            return todoItemDTO;
        }

        public async Task DeleteAsync(long id , long userId)
        {
            await _todoRepository.DeleteAsync(id, userId);
        }
    }
}
