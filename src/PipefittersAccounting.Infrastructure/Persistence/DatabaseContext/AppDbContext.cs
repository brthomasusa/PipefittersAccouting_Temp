#pragma warning disable CS8618

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.FinancierAggregate;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Core.Shared;

namespace PipefittersAccounting.Infrastructure.Persistence.DatabaseContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<ExternalAgent> ExternalAgents { get; set; }
        public DbSet<AgentType> ExternalAgentTypes { get; set; }
        public DbSet<EconomicEvent> EconomicEvents { get; set; }
        public DbSet<EventType> EconomicEventTypes { get; set; }
        public DbSet<DomainUser> DomainUsers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Financier> Financiers { get; set; }
        public DbSet<LoanAgreement> LoanAgreements { get; set; }
        public DbSet<LoanInstallment> LoanInstallments { get; set; }
        public DbSet<CashAccount> CashAccounts { get; set; }
        public DbSet<CashTransaction> CashTransactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
