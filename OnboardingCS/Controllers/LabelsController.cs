using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnboardingCS.DTO;
using OnboardingCS.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using OnboardingCS.Repositories;
using OnboardingCS.Interface;

namespace OnboardingCS.Controllers
{
    public class LabelsController : BaseController 
    {
        /*public IActionResult Index()
        {
            return View();
        }*/
        //private static IEnumerable<Label> _labels = new List<Label>();
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private ILabelService _labelService;

        public LabelsController(IUnitOfWork unitOfWork, IMapper mapper, ILabelService labelService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _labelService = labelService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LabelDTO>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<LabelDTO>>> GetAll()
        {
            var result = await _labelService.GetAll();
            return new OkObjectResult(result);
            //ERROR kalau dto 
            //var labelDTO = _mapper.Map<LabelDTO>(result);
            //var result2 = await _unitOfWork.LabelRepository.GetAll().ToListAsync();
            //AutoMapper.AutoMapperMappingException: Missing type map configuration or unsupported mapping.
            //Mapping types:
            //Object->LabelDTO
            //SOLVED: pakai .ProjectTo karena kalau .Map di _mapper itu single entity
            //return new OkObjectResult(result);
        }

        [HttpGet("Todos")]
        [ProducesResponseType(typeof(IEnumerable<LabelWithTodosDTO>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<LabelWithTodosDTO>>> GetAllWithTodos()
        {
            var result2 = _unitOfWork.LabelRepository.GetAll().Include(x => x.Todos); //include todos on fetch
            var result = await result2.ProjectTo<LabelWithTodosDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return new OkObjectResult(result);
        }

        [HttpGet("{id}", Name = "LabelLink")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(LabelDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(string))] //Kalau error, returnnya apa objectnya bakal kayak apa
        public async Task<ActionResult<LabelDTO>> Get(Guid id)
        {
            Label label = await _unitOfWork.LabelRepository.GetSingleAsync(label => label.LabelId == id);
            if (label != null)
            {
                return new OkObjectResult(_mapper.Map<LabelDTO>(label));
            }
            return new BadRequestObjectResult(id);
        }

        [HttpGet("{id}/Todos", Name = "LabelWithTodosLink")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(LabelWithTodosDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(string))] //Kalau error, returnnya apa objectnya bakal kayak apa
        public async Task<ActionResult<LabelDTO>> GetWithTodos(Guid id)
        {
            Label label = await _unitOfWork.LabelRepository.GetAll().Include( x => x.Todos).FirstOrDefaultAsync(label => label.LabelId == id);
            if (label != null)
            {
                return new OkObjectResult(_mapper.Map<LabelWithTodosDTO>(label));
            }
            return new BadRequestObjectResult(id);
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> CreateAsync([FromBody] LabelDTO labelDTO)
        {
            var isExist = _unitOfWork.LabelRepository.GetAll().Where(x => x.LabelName == labelDTO.LabelName).Any();
            if (!isExist)
            {
                var book = _mapper.Map<Label>(labelDTO);
                await _unitOfWork.LabelRepository.AddAsync(book);
                await _unitOfWork.SaveAsync();
                return new OkResult();
            }
            return new BadRequestResult();
        }

        [HttpPost]
        [Route("Todos")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> CreateWithTodosAsync([FromBody] LabelWithTodosDTO labelWithTodosDTO)
        {
            bool isLabelExist = _unitOfWork.LabelRepository.GetAll().Where(x => x.LabelName == labelWithTodosDTO.LabelName).Any();
            int countTodoIsExist = labelWithTodosDTO.Todos.Select(todo => _unitOfWork.TodoItemRepository.IsExist(todoInDB => todo.TodoId == todoInDB.TodoId)).Where( isExists => isExists ).Count();
            if (!isLabelExist && countTodoIsExist == 0)
            {
                var label = _mapper.Map<Label>(labelWithTodosDTO);
                await _unitOfWork.LabelRepository.AddAsync(label);
                await _unitOfWork.SaveAsync();
                return new OkResult();
            }
            return new BadRequestResult();
        }


        // Get api/<LabelsController>/{id}/Todos
        /// <summary>
        /// Get a Label with TodoItems with Redis.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Labels/5/Todo/Redis
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A Labels with TodoItem for the given Id</returns>
        /// <response code="201">Returns the Labels with TodoItem</response>
        /// <response code="400">If the item not found</response>  
        [HttpGet]
        [Route("{id}/Todos/Redis")]
        [ProducesResponseType(typeof(LabelWithTodosDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> GetWithTodoRedisAsync(Guid id)
        {
            LabelWithTodosDTO? label = await _labelService.GetLabelWithTodosRedis(id);
            if (label == null)
            {
                return BadRequest();
            }
            return new OkObjectResult(label);
        }

        // DELETE api/<LabelsController>/5
        /// <summary>
        /// Delete a Label.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /Labels/{id}
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>All recent todo items</returns>
        /// <response code="200">Returns all recent todo items</response>
        /// <response code="400">If the item is with given id is not found</response>  
        [ProducesResponseType(typeof(List<TodoItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            //if (_unitOfWork.TodoItemRepository.IsExist(selectedTodo ))
            bool isDeleted = await _labelService.DeleteLabel(id);
            if (isDeleted){
                return new OkObjectResult(isDeleted);
            }
            return new BadRequestObjectResult(isDeleted);
        }
    }
}
