#pragma warning disable CS8618

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;

namespace PipefittersAccounting.Infrastructure.Persistence.DatabaseContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}