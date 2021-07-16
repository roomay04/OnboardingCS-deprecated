using AutoMapper;
using OnboardingCS.DTO;
using OnboardingCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<LabelDTO, Label>().ReverseMap();
            //CreateMap<Label, LabelDTO>();
            CreateMap<TodoItem, TodoItemDTO>().ReverseMap();
            //CreateMap<TodoItemDTO, TodoItem>();
        }
    }
}
