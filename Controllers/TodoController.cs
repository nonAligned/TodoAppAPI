using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Todo;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }
            
            var todos = await _todoRepo.GetAllAsync(query, userId);

            var todoDto = todos.Select(t => t.ToTodoDto());

            return Ok(todoDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var todoModel = todoDto.ToTodoFromCreateDto();

            todoModel.UserId = userId;
            todoModel.CreatedAt = DateTime.UtcNow;

            await _todoRepo.CreateAsync(todoModel);
            return CreatedAtAction(nameof(GetById), new { id = todoModel.TodoId }, todoModel.ToTodoDto());
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTodoRequestDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todoModel = await _todoRepo.UpdateAsync(id, updateDto);

            if(todoModel == null)
            {
                return NotFound();
            }

            return Ok(todoModel.ToTodoDto());
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todoModel = await _todoRepo.DeleteAsync(id);

            if(todoModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}