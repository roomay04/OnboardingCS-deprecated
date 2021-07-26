using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OnboardingCS.DTO;
using OnboardingCS.Models;

namespace OnboardingCS.Interface
{
    public interface ITodoItemService
    {
        public Task SendTodoItemToEventHub(TodoItemDTO TodoItem, IConfiguration _config);
    }
}
