using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.Models
{
    public class TodoItem
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TodoId { get; set; } 
        [Required]
        public string TodoName { get; set; }
        [Required]
        public bool TodoIsDone { get; set; }
        public DateTime DueDate { get; set; }
    }
}
