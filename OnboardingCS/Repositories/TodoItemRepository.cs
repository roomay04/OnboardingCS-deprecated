using Microsoft.EntityFrameworkCore;
using OnboardingCS.Interface;
using OnboardingCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.Repository
{
    public class TodoItemRepository : BaseRepository<TodoItem>, ITodoItemRepository
    {
        public TodoItemRepository(DbContext dbContext) : base(dbContext) { }
    }
}
