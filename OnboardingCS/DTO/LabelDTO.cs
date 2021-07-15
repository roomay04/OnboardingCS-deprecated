using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.DTO
{
    public class LabelDTO
    {
        public string LabelName { get; set; }
        public TodoItemDTO Todos { get; set; }
    }
}
