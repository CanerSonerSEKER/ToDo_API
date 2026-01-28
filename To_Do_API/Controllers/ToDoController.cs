using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using To_Do_API.Models;
using To_Do_API.Models.ToDoDTO;
using To_Do_API.Services;

namespace To_Do_API.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class ToDoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public ToDoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // GET All 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodos()
        {
            return Ok(await _todoService.GetAllAsync());
        }

        // GET BY ID
        [HttpGet("{id:long}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodosById(long id)
        {
            TodoItemDTO todo = await _todoService.GetByIdAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        // ADD / POST 
        [HttpPost()]
        public async Task<ActionResult<TodoItemDTO>> Create([FromBody] CreateTodoItemRequest createTodoRequest, CancellationToken ct)
        {
            TodoItemDTO result = await _todoService.CreateAsync(createTodoRequest, ct);
            return CreatedAtAction(nameof(GetTodosById), new { id = result.Id }, result);
        }


        // UPDATE / PUT
        [HttpPut]
        public async Task<ActionResult<TodoItemDTO>> Update(long id, UpdateTodoItemRequest updateTodoRequest, CancellationToken ct)
        {
            return await _todoService.UpdateAsync(id, updateTodoRequest, ct);
        }


        // Delete 
        [HttpDelete]
        [Route(("Delete/{id}"))]
        public async Task Delete(long id)
        {
            await _todoService.DeleteAsync(id);
        }
    }
}
