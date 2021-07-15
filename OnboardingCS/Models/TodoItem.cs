using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.Models
{
    public class TodoItem
    {

        public int TodoId { get; set; } // #TODO harusnya apa ya selain int?
        public string TodoName { get; set; }
        public bool TodoIsDone { get; set; }
        /*public static IEnumerable<TodoItem> getTodoItems()
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem {todoId = 1, todoActivity = "Masak", todoIsDone = false },
                new TodoItem {todoId = 2, todoActivity = "Masak", todoIsDone = false },
                new TodoItem {todoId = 3, todoActivity = "Masak", todoIsDone = false },
                new TodoItem {todoId = 4, todoActivity = "Masak", todoIsDone = false },
            };
        }*/
    }
}
