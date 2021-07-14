using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.DTO
{
    public class LabelRequest
    {
        public string labelName { get; set; }
        public TodoItemRequest todos { get; set; }
    }
}
