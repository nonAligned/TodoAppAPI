using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Todo
    {
        public Guid TodoId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
        public DateTime CreatedAt { get; set; }
        public User? User { get; set; }
    }
}