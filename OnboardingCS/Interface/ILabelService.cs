using OnboardingCS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.Interface
{
    public interface ILabelService
    {
        public Task<LabelWithTodosDTO?> GetLabelWithTodosRedis(Guid id);
        public Task<bool> DeleteLabel(Guid id);
        public Task<List<LabelDTO>?> GetAll();
        public Task<LabelWithTodosDTO?> CreateLabelWithTodos(LabelWithTodosDTO labelWithTodosDTO);
    }
}
