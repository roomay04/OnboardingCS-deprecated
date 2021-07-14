using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.Model
{
    public class Label
    {
        public int labelId { get; set; }
        public string labelName { get; set; }
        public IEnumerable<TodoItem> todos { get; set; }
    }
}
