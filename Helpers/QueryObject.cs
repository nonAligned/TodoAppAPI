using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class QueryObject
    {
        public string? Title { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
        public string? Filter { get; set; } = null;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 100;
    }
}