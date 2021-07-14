using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.Model
{
    public class TodoItem
    {
        public int todoId { get; set; } // #TODO harusnya apa ya selain int?
        public string todoName { get; set; }
        public bool todoIsDone { get; set; }

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
