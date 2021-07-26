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

namespace OnboardingCS.Controllers
{
    public class LabelsController : BaseController 
    {
        /*public IActionResult Index()
        {
            return View();
        }*/
        //private static IEnumerable<Label> _labels = new List<Label>();
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;

        public LabelsController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LabelDTO>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<LabelDTO>>> GetAll()
        {
            var result2 = _unitOfWork.LabelRepository.GetAll(); //doesn't include todo on fecth, todos will set as null
            var result = await result2.ProjectTo<LabelDTO>(_mapper.ConfigurationProvider).ToListAsync();
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

    }
}
