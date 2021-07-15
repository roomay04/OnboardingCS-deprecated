using Microsoft.EntityFrameworkCore;
using OnboardingCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.Repository
{
    public class LabelRepository : BaseRepository<Label>
    {
        public LabelRepository(DbContext dbContext) : base(dbContext) { }
    }
}
