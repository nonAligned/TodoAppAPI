using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Todo;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public TodoController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _context.Todos.ToListAsync();

            var todoDto = todos.Select(t => t.ToTodoDto());

            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var todo = await _context.Todos.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo.ToTodoDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoRequestDto todoDto)
        {
            var todoModel = todoDto.ToTodoFromCreateDto();
            await _context.Todos.AddAsync(todoModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = todoModel.TodoId }, todoModel.ToTodoDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTodoRequestDto updateDto)
        {
            var todoModel = await _context.Todos.FirstOrDefaultAsync(x => x.TodoId == id);

            if(todoModel == null)
            {
                return NotFound();
            }

            todoModel.Title = updateDto.Title;
            todoModel.IsComplete = updateDto.IsComplete;

            await _context.SaveChangesAsync();

            return Ok(todoModel.ToTodoDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var todoModel = await _context.Todos.FirstOrDefaultAsync(x => x.TodoId == id);

            if(todoModel == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todoModel);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}