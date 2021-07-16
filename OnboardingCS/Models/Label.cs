using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnboardingCS.Models
{
    public class Label
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid LabelId { get; set; }
        [Required]
        public string LabelName { get; set; }
        //[JsonIgnore] // handle error ketika pakai include di todo maupun di label-> System.Text.Json.JsonException: A possible object cycle was detected which is not supported. This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32.
        public ICollection<TodoItem> Todos { get; set; }  // ga keambil datanya meskipun udah diJsonIgnore dan di include
        // diatas bikin error LabelId ga ada di TodoItem -> soalnya dia ngecek di Todo ada apa nggak atau mappingannya
        [NotMapped]
        public bool IsArchive { get; set; }

    }
}
