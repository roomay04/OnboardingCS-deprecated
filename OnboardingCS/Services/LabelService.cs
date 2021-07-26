using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnboardingCS.DTO;
using OnboardingCS.Interface;
using OnboardingCS.Models;
using System;
using System.Collections.Generic;
using AutoMapper.QueryableExtensions;
using System.Linq;
using System.Threading.Tasks;
using static OnboardingCS.Services.RedisService;

namespace OnboardingCS.Services
{
    public class LabelService : ILabelService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IConfiguration _config;
        private readonly ILogger<LabelService> _logger; //TODO kenapa dia readonly ya?
        private IRedisService _redis;

        public LabelService(ILogger<LabelService> logger, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IRedisService redisService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _config = configuration;
            _redis = redisService;
        }
        public async Task<LabelWithTodosDTO?> GetLabelWithTodosRedis(Guid id)
        {
            LabelWithTodosDTO? labelWithTodos = await _redis.GetStringAsync<LabelWithTodosDTO>(RedisKeys.GetLabelWithTodosById(id));
            if (labelWithTodos == null)
            {
                List<Label> labelList = await _unitOfWork.LabelRepository.GetAll().Where(label => label.LabelId == id).Include(x => x.Todos).ToListAsync();
                Label label = await _unitOfWork.LabelRepository.GetAll().Where(label => label.LabelId == id).Include(x => x.Todos).FirstOrDefaultAsync(); //Tadi sempet ga bisa, ternyata karena belom import EF
                List<Label> label2List = await _unitOfWork.LabelRepository.GetAll().Include(x => x.Todos).Where(label => label.LabelId == id).ToListAsync(); //TODO Check ngaruh speednya ga
                Label label2 = await _unitOfWork.LabelRepository.GetAll().Include(x => x.Todos).Where(label => label.LabelId == id).SingleOrDefaultAsync(); //TODO Check ngaruh speednya ga
                labelWithTodos = _mapper.Map<LabelWithTodosDTO>(label);

                await _redis.SaveStringAsync<LabelWithTodosDTO>(RedisKeys.GetLabelWithTodosById(id), labelWithTodos);
            }

            return labelWithTodos;
        }

        //TODO buat yg getAll pakai redis

        public async Task<bool> DeleteLabel(Guid id){
            if (_unitOfWork.LabelRepository.IsExist( curr => curr.LabelId == id)) // Kalau ga dihandle ga error, tapi gasesuai expect user aja ga sih?
            { 
                _unitOfWork.TodoItemRepository.Delete(selectedTodo => selectedTodo.TodoId == id);
                await _redis.DeleteStringAsync(RedisKeys.GetLabelWithTodosById(id));
                return true;
            }
            return false;
        }

        public async Task<List<LabelDTO>> GetAll(){
            var result2 = _unitOfWork.LabelRepository.GetAll(); //doesn't include todo on fecth, todos will set as null
            var result = await result2.ProjectTo<LabelDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return result;
        }
    }
}
