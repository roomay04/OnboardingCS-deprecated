using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnboardingCS.DTO;
using OnboardingCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// This code is generated with Controller + CRUD Template


namespace OnboardingCS.Controllers
{
    public class TodoItemsController : BaseController
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;

        public TodoItemsController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET: api/<TodosController>
        /// <summary>
        /// Get All TodoItem.
        /// </summary>
        /// <returns>All item of TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If no items created</response>  
        /// 
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<TodoItemDTO>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetAll()
        {
            List<TodoItemDTO> result = await _unitOfWork.TodoItemRepository.GetAll().ProjectTo<TodoItemDTO>(_mapper.ConfigurationProvider).ToListAsync();
            if (result.Count > 0)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        // GET: api/<TodosController>
        /// <summary>
        /// Get a TodoItem.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Selected todo item with given Id</returns>
        /// <response code="201">Returns selected todo item</response>
        /// <response code="404">If no items with selected Id</response>
        // GET api/<TodosController>/5
        [HttpGet("{id}", Name = "TodoDetailLink")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Label), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoItem>> Get([FromRoute] Guid id)
        {
            TodoItem item = await _unitOfWork.TodoItemRepository.GetByIdAsync(id);
            if (item != null)
            {
                return new OkObjectResult(item);
            }
            return new NotFoundResult();
        }

        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "TodoName": "Item1",
        ///        "TodoIsDone": true
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
        public async Task<ActionResult<IEnumerable<TodoItem>>> Post([FromBody] TodoItemDTO todoItemDTO)
        {
            var todoItem = _mapper.Map<TodoItem>(todoItemDTO);
            TodoItem newTodo = await _unitOfWork.TodoItemRepository.AddAsync(todoItem);
            await _unitOfWork.SaveAsync();

            return CreatedAtRoute("TodoDetailLink", new { id = todoItem.TodoId }, newTodo);
        }

        // PUT api/<TodosController>/5
        /// <summary>
        /// Update a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isDone": true
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly updated TodoItem</returns>
        /// <response code="201">Returns the updated created item</response>
        /// <response code="400">If the item not found</response>  
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<TodoItem>> Put(Guid id, [FromBody] TodoItemDTO todoItemDTO)
        {
            var todoItem = await _unitOfWork.TodoItemRepository.GetSingleAsync(curr => curr.TodoId == todoItemDTO.TodoId);

            // kalau ga ketemu dan langsung dipanggil akan return error 500 System.NullReferenceException: Object reference not set to an instance of an object.
            // jadi baiknya di handle
            if (todoItem != null) { 
                todoItem.TodoName = todoItemDTO.TodoName;
                todoItem.TodoIsDone = todoItemDTO.TodoIsDone;

                _unitOfWork.TodoItemRepository.Edit(todoItem); 
                await _unitOfWork.SaveAsync();

                return CreatedAtRoute("TodoDetailLink", new { id = todoItem.TodoId }, todoItem); //TODO: kalau post benernya pake 201 atau 200 ya
            }
            return BadRequest();

            //TodoItem todoItem2 = _mapper.Map<TodoItem>(todoItemDTO);
            //todoItem2.TodoId = todoItem.TodoId;
            //_unitOfWork.TodoItemRepository.Edit(todoItem);
        }

        // PUT api/<TodosController>/5
        /// <summary>
        /// Update a TodoItem with another approach.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isDone": true
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly updated TodoItem</returns>
        /// <response code="201">Returns the updated created item</response>
        /// <response code="400">If the item not found</response>  
        //[HttpPut("/edit2/{id}")] -> ini ngebuat dia jadi /edit2/{id}
        [HttpPut("edit2/{id}")] // -> ini ngebuat jadi /api/[controller]/edit2/{id} -> tetep ngikutin behaviour parent
        [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<TodoItem>> Put2(Guid id, [FromBody] TodoItem todoItemDTO)
        {
            // kalau ga ketemu errornya apa? 
            bool isExists = await _unitOfWork.TodoItemRepository.IsExist(curr => curr.TodoId == todoItemDTO.TodoId);
            //todoItem.TodoName = todoItemDTO.TodoName;
            //todoItem.TodoIsDone = todoItemDTO.TodoIsDone;
            //_unitOfWork.TodoItemRepository.Edit(todoItem);

            if (isExists)
            {
                TodoItem todoItem2 = _mapper.Map<TodoItem>(todoItemDTO);
                todoItem2.TodoId = id;
                _unitOfWork.TodoItemRepository.Edit(todoItem2);
                await _unitOfWork.SaveAsync();
                return CreatedAtRoute("TodoDetailLink", new { id = todoItem2.TodoId }, todoItem2);
            }
            return BadRequest();

        }

        // DELETE api/<TodosController>/5
        /// <summary>
        /// Delete a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /TodoItems/{id}
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>All recent todo items</returns>
        /// <response code="200">Returns all recent todo items</response>
        /// <response code="404">If the item is null</response>  
        [ProducesResponseType(typeof(List<TodoItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [HttpDelete("{id}")]
        public ActionResult<IEnumerable<TodoItem>> Delete(Guid id)
        {
            //if (_unitOfWork.TodoItemRepository.IsExist(selectedTodo ))
            _unitOfWork.TodoItemRepository.Delete(selectedTodo => selectedTodo.TodoId == id);
            return new OkResult();
        }

        ////// use path "/id" to prevent conflict with GetAll()
        ////// GET api/<TodosController>/id?id={id}
        /////// <summary>
        /////// Get a TodoItem using param (just test).
        /////// </summary>
        /////// <param name="id"></param>
        /////// <returns>Selected todo item with given Id</returns>
        /////// <response code="201">Returns selected todo item</response>
        /////// <response code="404">If no items with selected Id</response>
        ////[HttpGet("id")]
        ////public ActionResult<TodoItem> GetFromParam([FromQuery] int id)
        ////{
        ////    Console.WriteLine("test");
        ////    Console.WriteLine(Request.Path);
        ////    TodoItem item = _todoItems.FirstOrDefault(item => item.TodoId == id);
        ////    if (item != null)
        ////    {
        ////        return new OkObjectResult(item);
        ////    }
        ////    return new BadRequestObjectResult(id);
        ////}
    }
}
