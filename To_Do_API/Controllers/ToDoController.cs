using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using To_Do_API.Models;

namespace To_Do_API.Controllers
{
    [ApiController]
    public class ToDoController : Controller
    {
        private readonly ToDoContext _context;

        public ToDoController(ToDoContext context)
        {
            _context = context;

            if (!_context.ToDoItem.Any())
            {
                _context.ToDoItem.Add(new TodoItem
                {
                    Name = "Eve gel",
                    IsComplete = true,
                });
                _context.SaveChanges();
            }
        }

        // GET All 
        [HttpGet("GetToDoItems")]
        public async Task<IEnumerable<TodoItem>> GetToDoItems()
        {

            List<TodoItem> items = await _context.ToDoItem.ToListAsync();

            return items;
        }

        //// GET BY ID 
        //[HttpGet("{id}")]
        //public ActionResult<TodoItem> GetToDoItemsById(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var items = _context.ToDoItem.Find(id);

        //    if (items == null)
        //    {
        //        return NotFound();
        //    }

        //    return items;
        //}

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetToDoItemById(long? id)
        {
            if (id == null)
                return NotFound();

            TodoItem item = await _context.ToDoItem.FindAsync(id);

            if (item == null)
                return BadRequest();

            return item;
        }


        // ADD / POST 
        [HttpPost("CreateAnItem")]
        public async Task<ActionResult<TodoItem>> CreateAnItem(TodoItem toDoItem)
        {
            _context.ToDoItem.Add(toDoItem);
            await _context.SaveChangesAsync();

            // Once veri alınıyor sonrasında yeni bir id atanarak girilen item ekleniyor. Anladığım budur.
            return CreatedAtAction(nameof(GetToDoItems), new { id = toDoItem.Id }, toDoItem);
        }


        // UPDATE / PUT
        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItem>> UpdateAnItem(long? id, [FromBody] TodoItem toDoItem)
        {
            if (toDoItem == null || toDoItem.Id != id)
                return BadRequest();

            TodoItem item = await _context.ToDoItem.FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
                return NotFound();

            item.Name = toDoItem.Name;
            item.IsComplete = toDoItem.IsComplete;

            _context.ToDoItem.Update(item);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }


        // DELETE 
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteAnItem(long? id, TodoItem toDoItem)
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

    }
}
