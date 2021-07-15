using OnboardingCS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.DTO
{
    public class TodoItemDTO
    {
        public Guid TodoId { get; set; }
        [Required]
        public string TodoName { get; set; }
        public bool TodoIsDone { get; set; }
        public DateTime DueDate { get; set; }
        public Guid LabelId { get; set; }
        //jadi muncul karena ada ini dan ada mapping dari model juga
        public LabelDTO Label { get; set; } //pas pake LabelDTO, dia langsung dapet nested 2x, tapi pas pake Label aja, cuma sekali aja
        public TodoItemDTO()
        {
            TodoIsDone = false;
        }
    }
}
