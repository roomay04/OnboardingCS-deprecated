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
        [ProducesResponseType(typeof(IEnumerable<Label>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<LabelDTO>>> GetAll()
        {
            var result = await _unitOfWork.LabelRepository.GetAll().Include(x => x.Todos).ProjectTo<LabelDTO>(_mapper.ConfigurationProvider).ToListAsync();
            //var labelDTO = _mapper.Map<LabelDTO>(result);
            //var result2 = await _unitOfWork.LabelRepository.GetAll().ToListAsync();
            return new OkObjectResult(result);
            //TODO kalau dto error
            //AutoMapper.AutoMapperMappingException: Missing type map configuration or unsupported mapping.
            //Mapping types:
            //Object->LabelDTO
            //return new OkObjectResult(result);
        }

        [HttpGet("{id}", Name = "LabelLink")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Label), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(string))] //Kalau error, returnnya apa objectnya bakal kayak apa
        public async Task<ActionResult<Label>> Get(Guid id)
        {
            Label label = await _unitOfWork.LabelRepository.GetSingleAsync(label => label.LabelId == id);
            if (label != null)
            {
                return new OkObjectResult(label);
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

    }
}
