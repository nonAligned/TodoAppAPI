using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Todo;
using api.Interfaces;
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
        private readonly ITodoRepository _todoRepo;
        public TodoController(ApplicationDBContext context, ITodoRepository todoRepo)
        {
            _todoRepo = todoRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _todoRepo.GetAllAsync();

            var todoDto = todos.Select(t => t.ToTodoDto());

            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var todo = await _todoRepo.GetByIdAsync(id);

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
            //UserId only for testing
            todoModel.UserId = Guid.Parse("7daeebe8-8eee-4dd4-a9e5-f6293c8fa768");
            await _todoRepo.CreateAsync(todoModel);
            return CreatedAtAction(nameof(GetById), new { id = todoModel.TodoId }, todoModel.ToTodoDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTodoRequestDto updateDto)
        {
            var todoModel = await _todoRepo.UpdateAsync(id, updateDto);

            if(todoModel == null)
            {
                return NotFound();
            }

            return Ok(todoModel.ToTodoDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var todoModel = await _todoRepo.DeleteAsync(id);

            if(todoModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}