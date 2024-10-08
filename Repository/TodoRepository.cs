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

        public async Task<List<Todo>> GetAllAsync(QueryObject query, string userId)
        {
            
            var todos = _context.Todos.Where(t => t.UserId == userId).AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.Title))
            {
                todos = todos.Where(t => t.Title.Contains(query.Title));
            }

            if(query.Filter != null)
            {
                if (query.Filter.Equals("Completed", StringComparison.OrdinalIgnoreCase))
                {
                    todos = todos.Where(t => t.IsComplete == true);
                }
                else if (query.Filter.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                {
                    todos = todos.Where(t => t.IsComplete == false);
                }
            }

            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase))
                {
                    todos = query.IsDescending ? todos.OrderByDescending(t => t.CreatedAt) : todos.OrderBy(t => t.CreatedAt);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await todos.Skip(skipNumber).Take(query.PageSize).ToListAsync();
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