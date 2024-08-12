using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class User : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
    }
}