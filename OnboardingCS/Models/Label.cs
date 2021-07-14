using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.Models
{
    public class Label
    {
        [Required]
        public int labelId { get; set; }
        [Required]
        public string labelName { get; set; }
        public IEnumerable<TodoItem> todos { get; set; }
    }
}
