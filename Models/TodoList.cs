using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.obj.Models;

namespace api.Models
{
    public class TodoList
    {
        public int Id { get; set; }
        public List<Todo> Todos { get; set; } = new List<Todo>();
        public int? UserId { get; set; }
    }
}