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

        //private readonly ToDoContext _context;

        //public ToDoController(ToDoContext context)
        //{
        //    _context = context;
        //}

        public ToDoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // GET All 
        [HttpGet]
        [Route("GetTodos")]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodos()
        {
            return await _todoService.GetAllAsync();
        }

        // GET BY ID
        [HttpGet]
        [Route("GetTodosById/{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodosById(long id)
        {
            return await _todoService.GetByIdAsync(id);
        }

        // ADD / POST 
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<TodoItemDTO>> Create([FromBody]CreateTodoItemRequest createTodoRequest, CancellationToken ct)
        {
            //TodoItem todoItem = new TodoItem()
            //{
            //    Name = todoItemDTO.Name,
            //    IsComplete = todoItemDTO.IsComplete
            //};

            //_context.ToDoItem.Add(todoItem);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetToDoItemById), new { id = todoItem.Id }, ItemToDTO(todoItem));
            
            // BU HATAYI ALIYORUM EN SON ONA BAKIYORDUM....

            var result = await _todoService.CreateAsync(createTodoRequest, ct);
            return CreatedAtAction(nameof(GetTodosById),new { id = result.Id }, result);

        }


        // UPDATE / PUT
        [HttpPut]
        [Route("Update/{id}")]
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


        //private bool TodoItemExist(long? id)
        //{
        
        //    return _context.ToDoItem.Any(i => i.Id == id);
        //}

        //private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
        //    new TodoItemDTO
        //    {
        //        Id = todoItem.Id,
        //        Name = todoItem.Name,
        //        IsComplete = todoItem.IsComplete,
        //    };
    }
}
