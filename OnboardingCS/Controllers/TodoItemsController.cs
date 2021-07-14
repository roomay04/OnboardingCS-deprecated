using Microsoft.AspNetCore.Mvc;
using OnboardingCS.DTO;
using OnboardingCS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// This code is generated with Controller + CRUD Template


namespace OnboardingCS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private static IEnumerable<TodoItem> _todoItems = new List<TodoItem>(){
                new TodoItem {todoId = 1, todoName = "Masak1", todoIsDone = false },
                new TodoItem {todoId = 2, todoName = "Masak2", todoIsDone = false },
                new TodoItem {todoId = 3, todoName = "Masak3", todoIsDone = false },
                new TodoItem {todoId = 4, todoName = "Masak4", todoIsDone = false },
            };

        // GET: api/<TodosController>
        [HttpGet]
        public IEnumerable<TodoItem> Get()
        {
            return _todoItems;
        }

        // GET api/<TodosController>/5
        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get(int id)
        {
            TodoItem item = _todoItems.FirstOrDefault(item => item.todoId == id);
            if (item != null)
            {
                return new OkObjectResult(item);
            }
            return new BadRequestObjectResult(id);
        }

        // POST api/<TodosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TodosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TodosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
