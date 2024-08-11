using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Todo;
using api.Models;

namespace api.Mappers
{
    public static class TodoMappers
    {
        public static TodoDto ToTodoDto(this Todo todoModel)
        {
            return new TodoDto
            {
                TodoId = todoModel.TodoId,
                Title = todoModel.Title,
                IsComplete = todoModel.IsComplete
            };
        }

        public static Todo ToTodoFromCreateDto(this CreateTodoRequestDto todoDto)
        {
            return new Todo
            {
                Title = todoDto.Title,
                IsComplete = todoDto.IsComplete,
            };
        }
    }
}

    