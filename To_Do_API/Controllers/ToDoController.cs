using Microsoft.AspNetCore.Mvc;
using To_Do_API.Models.ToDoDTO;
using To_Do_API.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using To_Do_API.Exceptions;

namespace To_Do_API.Controllers
{
    [ApiController]
    [Route("api/todos")]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public ToDoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        private long GetUserId()
        {
            string userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                throw new UnauthorizeException("İstenen Id ile user bulunamadı.");
            }

            return long.Parse(userIdClaim);
        }


        // GET All 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodos()
        {
            long userId = GetUserId();
            return Ok(await _todoService.GetAllAsync(userId));
        }

        // GET BY ID
        [HttpGet("{id:long}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodosById(long id)
        {
            long userId = GetUserId();
            TodoItemDTO todo = await _todoService.GetByIdAsync(id, userId);

            return Ok(todo);
        }

        // ADD / POST 
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> Create([FromBody] CreateTodoItemRequest createTodoRequest, CancellationToken ct)
        {
            long userId = GetUserId();
            TodoItemDTO result = await _todoService.CreateAsync(createTodoRequest, userId, ct);
            return CreatedAtAction(nameof(GetTodosById), new { id = result.Id }, result);
        }


        // UPDATE / PUT
        [HttpPut("{id:long}")]
        public async Task<ActionResult<TodoItemDTO>> Update(long id, UpdateTodoItemRequest updateTodoRequest, CancellationToken ct)
        {
            long userId = GetUserId();
            TodoItemDTO result = await _todoService.UpdateAsync(id, updateTodoRequest, userId, ct);

            return Ok(result);
        }


        // Delete 
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            long userId = GetUserId();
            await _todoService.DeleteAsync(id, userId);
            return NoContent();
        }
    }
}
