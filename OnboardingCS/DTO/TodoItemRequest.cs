using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.DTO
{
    public class TodoItemRequest
    {
        public int todoId { get; set; }
        public string todoName { get; set; }
        public bool isDone { get; set; }
    }
}
