using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Todo;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDBContext _context;
        public TodoRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Todo> CreateAsync(Todo todoModel)
        {
            await _context.Todos.AddAsync(todoModel);
            await _context.SaveChangesAsync();
            return todoModel;
        }

        public async Task<Todo?> DeleteAsync(Guid id)
        {
            var todoModel = await _context.Todos.FirstOrDefaultAsync(x => x.TodoId == id);

            if(todoModel == null)
            {
                return null;
            }

            _context.Todos.Remove(todoModel);
            await _context.SaveChangesAsync();

            return todoModel;
        }

        public async Task<List<Todo>> GetAllAsync(QueryObject query)
        {
            var todos = _context.Todos.AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.Title))
            {
                todos = todos.Where(t => t.Title.Contains(query.Title));
            }

            if(query.IsComplete != null)
            {
                todos = todos.Where(t => t.IsComplete == query.IsComplete);
            }

            return await todos.ToListAsync();
        }

        public async Task<Todo?> GetByIdAsync(Guid id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task<Todo?> UpdateAsync(Guid id, UpdateTodoRequestDto todoDto)
        {
            var existingTodo = await _context.Todos.FirstOrDefaultAsync(x => x.TodoId == id);

            if (existingTodo == null)
            {
                return null;
            }

            existingTodo.Title = todoDto.Title;
            existingTodo.IsComplete = todoDto.IsComplete;

            await _context.SaveChangesAsync();

            return existingTodo;
        }
    }
}