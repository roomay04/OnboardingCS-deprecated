using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.Models
{
    public class HanOnboardingSkdDbContext : DbContext
    {
        public HanOnboardingSkdDbContext(DbContextOptions<HanOnboardingSkdDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<VisibilityTicket>().HasIndex(a => new { a.TicketTypeId, a.CallTypeId, a.SubCallTypeId }).IsUnique(true);
        }

        public DbSet<Label> Labels { get; set; }
        //public DbSet<Author> Authors { get; set; }
    }
}
