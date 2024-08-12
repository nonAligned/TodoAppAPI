using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Todo;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetAllAsync(QueryObject query, string userId);
        Task<Todo?> GetByIdAsync(Guid id);
        Task<Todo> CreateAsync(Todo todoModel);
        Task<Todo?> UpdateAsync(Guid id, UpdateTodoRequestDto todoDto);
        Task<Todo?> DeleteAsync(Guid id);
    }
}