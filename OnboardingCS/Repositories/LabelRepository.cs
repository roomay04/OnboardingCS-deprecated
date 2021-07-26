using Microsoft.EntityFrameworkCore;
using OnboardingCS.Interface;
using OnboardingCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.Repository
{
    public class LabelRepository : BaseRepository<Label>, ILabelRepository
    {
        public LabelRepository(DbContext dbContext) : base(dbContext) { }
    }
}
