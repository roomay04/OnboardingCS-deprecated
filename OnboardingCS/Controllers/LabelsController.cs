using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnboardingCS.DTO;
using OnboardingCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace OnboardingCS.Controllers
{
    // public class LabelsController : Controller
    // Don't use Controller class, use ControllerBase instead
    // Controller derives from ControllerBase and adds support for views, so it's for handling web pages, not web API requests.
    public class LabelsController : BaseController 
    {
        /*public IActionResult Index()
        {
            return View();
        }*/
        private static IEnumerable<Label> _labels = new List<Label>();

        [HttpPost] //attributes to specify the supported HTTP action verb
        [ProducesResponseType(StatusCodes.Status201Created)] // any known HTTP status codes that could be returned
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // specify any known HTTP status codes that could be returned
        public ActionResult<Label> Create(LabelRequest labelRequest)
        {
            TodoItem newTodo = new TodoItem() { todoId = labelRequest.todos.todoId, todoName = labelRequest.todos.todoName, todoIsDone = labelRequest.todos.isDone };
            Label selectedLabel = _labels.FirstOrDefault(currLabel => currLabel.labelName == labelRequest.labelName);
            if (selectedLabel != null)
            {
                TodoItem selectedTodo = selectedLabel.todos.FirstOrDefault(currTodo => currTodo.todoId == labelRequest.todos.todoId);
                if (selectedTodo != null)
                {
                    selectedTodo = newTodo;
                }
                else
                {
                    selectedLabel.todos = selectedLabel.todos.Append(newTodo); //TODO: bener gini ga ya untuk assignnya
                }                
            }
            else
            {
                selectedLabel = new Label() { labelId = 1, labelName = labelRequest.labelName, todos = new List<TodoItem>() { newTodo } };
                _labels = _labels.Append(selectedLabel);
            }
            return new CreatedAtRouteResult("LabelLink",new { id = selectedLabel.labelId }, selectedLabel); //#TODO ini gimana sih kok dia ga ngarah ke yg bener
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Label>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Label>> Get()
        {
            if (_labels.Count() > 0)
            {
                return new OkObjectResult(_labels);
            }
            return new BadRequestObjectResult("No Labels");
        }

        [HttpGet("{id}", Name = "LabelLink")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Label), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        /*[ProducesErrorResponseType(]*/ //ini apa sih?
        public ActionResult<Label> Get(int id)
        {
            Label label = _labels.FirstOrDefault(label => label.labelId == id);
            if (label != null)
            {
                return new OkObjectResult(label);
            }
            return new BadRequestObjectResult(id);
        }

    }
}
