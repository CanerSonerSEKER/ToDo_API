using To_Do_API.Models.ToDoDTO;

namespace To_Do_API.Repository
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItemDTO>> GetAllAsync(long id);



    }
}
