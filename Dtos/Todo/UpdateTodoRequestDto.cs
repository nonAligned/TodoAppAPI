using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Todo
{
    public class UpdateTodoRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
    }
}