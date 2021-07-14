using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnboardingCS.DTO;
using OnboardingCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// This code is generated with Controller + CRUD Template


namespace OnboardingCS.Controllers
{
    public class TodoItemsController : BaseController
    {
        private static IEnumerable<TodoItem> _todoItems = new List<TodoItem>(){
                new TodoItem {todoId = 1, todoName = "Masak1", todoIsDone = false },
                new TodoItem {todoId = 2, todoName = "Masak2", todoIsDone = false },
                new TodoItem {todoId = 3, todoName = "Masak3", todoIsDone = false },
                new TodoItem {todoId = 4, todoName = "Masak4", todoIsDone = false },
            };

        // GET: api/<TodosController>

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _todoItems;
        }

        // GET api/<TodosController>/5
        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get([FromRoute] int id)
        {
            Console.WriteLine("test");
            Console.WriteLine(Request.Path);
            TodoItem item = _todoItems.FirstOrDefault(item => item.todoId == id);
            if (item != null)
            {
                return new OkObjectResult(item);
            }
            return new BadRequestObjectResult(id);
        }

        //ERROR: this code is conflict with GetAll()
        /*// GET api/<TodosController>/5
        [HttpGet("{id}/async")]
        public ActionResult<TodoItem> Get(int id)
        {
            Console.WriteLine("test");
            Console.WriteLine(Request.Path);
            TodoItem item = _todoItems.FirstOrDefault(item => item.todoId == id);
            if (item != null)
            {
                return new OkObjectResult(item);
            }
            return new BadRequestObjectResult(id);
        }*/

        //ERROR: this code is conflict with GetAll()
        // GET api/<TodosController>/5
        [HttpGet("id")]
        public ActionResult<TodoItem> GetFromParam([FromQuery] int id)
        {
            Console.WriteLine("test");
            Console.WriteLine(Request.Path);
            TodoItem item = _todoItems.FirstOrDefault(item => item.todoId == id);
            if (item != null)
            {
                return new OkObjectResult(item);
            }
            return new BadRequestObjectResult(id);
        }

        //#TODO via param
        // GET api/<TodosController>/5
        /*[HttpGet]
        public ActionResult<TodoItem> GetViaParam([FromQuery] int id)
        {
            TodoItem item = _todoItems.FirstOrDefault(item => item.todoId == id);
            if (item != null)
            {
                return new OkObjectResult(item);
            }
            return new BadRequestObjectResult(id);
        }*/

        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>  
        [ProducesResponseType(StatusCodes.Status201Created)] // any known HTTP status codes that could be returned
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // specify any known HTTP status codes that could be returned
        [HttpPost]
        public void Post([FromBody] TodoItem todoItem)
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
