using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.DTO
{
    public class LabelDTO
    {
        public Guid LabelId { get; set; }
        public string LabelName { get; set; }
        //public ICollection<TodoItemDTO> Todos { get; set; }
    }

    public class LabelWithTodosDTO : LabelDTO
    {
        //[JsonIgnore]  -> untuk ngeignore ketika parsing data aja
        // benarkah? handle error ketika pakai include di todo maupun di label-> System.Text.Json.JsonException: A possible object cycle was detected which is not supported. This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32.
        public ICollection<TodoItemDTO> Todos { get; set; }  // ga keambil datanya meskipun udah diJsonIgnore dan di include
        // diatas bikin error LabelId ga ada di TodoItem -> soalnya dia ngecek di Todo ada apa nggak atau mappingannya
    }
}
