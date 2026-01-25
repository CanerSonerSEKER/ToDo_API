using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using To_Do_API.Models;
using To_Do_API.Models.ToDoDTO;

namespace To_Do_API.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class ToDoController : Controller
    {
        private readonly ToDoContext _context;

        public ToDoController(ToDoContext context)
        {
            _context = context;
        }

        // GET All 
        [HttpGet("GetToDoItems")]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetToDoItems()
        {
            // Msc Learn
            //List<TodoItemDTO> items = await _context.TodoItemDTO.ToListAsync();

            //return items;

            // Model icerisinden aktarilarak elde edilen dtolu cikti
            return await _context.ToDoItem.Select(x => ItemToDTO(x)).ToListAsync();
        }

        // GET BY ID
        [HttpGet("GetToDoItemById/{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetToDoItemById(long id)
        {
            TodoItem todoItem = await _context.ToDoItem.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(todoItem);
        }


        // ADD / POST 
        [HttpPost("CreateAnItem")]
        public async Task<ActionResult<TodoItemDTO>> CreateAnItem(TodoItemDTO todoItemDTO)
        {
            TodoItem todoItem = new TodoItem()
            {
                Name = todoItemDTO.Name,
                IsComplete = todoItemDTO.IsComplete
            };

            _context.ToDoItem.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetToDoItemById), new { id = todoItem.Id }, ItemToDTO(todoItem));
        }


        // UPDATE / PUT
        [HttpPut("UpdateAnItem/{id}")]
        public async Task<ActionResult<TodoItemDTO>> UpdateAnItem(long? id, TodoItemDTO todoItemDTO)
        {
            if (todoItemDTO == null || todoItemDTO.Id != id)
                return BadRequest();

            TodoItem todoItem = await _context.ToDoItem.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExist(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, "Concurrency Error");
                }

            }
            return new NoContentResult();
        }


        // Delete 
        [HttpDelete("DeleteAnItem/{id}")]
        public async Task<ActionResult<TodoItemDTO>> DeleteAnItem(long? id)
        {
            if(id == null)
                return BadRequest();

            TodoItem item = await _context.ToDoItem.FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
                return NotFound();

            _context.ToDoItem.Remove(item);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }


        private bool TodoItemExist(long? id)
        {
        
            return _context.ToDoItem.Any(i => i.Id == id);
        }

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete,
            };
    }
}
