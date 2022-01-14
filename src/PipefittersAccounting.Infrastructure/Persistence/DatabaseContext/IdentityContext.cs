using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.Infrastructure.Identity;

namespace PipefittersAccounting.Infrastructure.Persistence.DatabaseContext
{
    public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // For Guid Primary Key
            builder.Entity<ApplicationUser>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<ApplicationRole>().Property(p => p.Id).ValueGeneratedOnAdd();
        }
    }
}