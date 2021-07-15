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
        public int TodoId { get; set; } // #TODO harusnya apa ya selain int?
        public string TodoName { get; set; }
        public bool TodoIsDone { get; set; }
        public DateTime DueDate { get; set; }
    }
}
