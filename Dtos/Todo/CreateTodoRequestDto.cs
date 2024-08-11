using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Todo
{
    public class CreateTodoRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Title must be minimum 3 characters")]
        [MaxLength(140, ErrorMessage = "Title cannot be over 140 characters")]
        public string Title { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
    }
}