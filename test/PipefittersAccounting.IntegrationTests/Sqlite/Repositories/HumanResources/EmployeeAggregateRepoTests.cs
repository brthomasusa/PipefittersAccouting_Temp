#pragma warning disable CS8625

using System;
using System.Threading.Tasks;
using TestSupport.EfHelpers;
using Xunit;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.HumanResources;
using PipefittersAccounting.SharedKernel.CommonValueObjects;


namespace PipefittersAccounting.IntegrationTests.Sqlite.Repositories.HumanResources
{
    public class EmployeeAggregateRepoTests
    {
        [Fact]
        public async Task ShouldInsert_ExternalAgentAndEmployee()
        {
            // DbContextOptionsDisposable<AppDbContext> options1 = SqliteInMemory.CreateOptions<AppDbContext>();
            var options = SqliteInMemory.CreateOptions<AppDbContext>();
            using var context = new AppDbContext(options);

            AppUnitOfWork uow = new AppUnitOfWork(context);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);

            context.Database.EnsureCreated();
            context.SeedDatabase();

            Guid agentId = Guid.NewGuid();
            Employee employee = new Employee
            (
                EmployeeAgent.Create(EntityGuidID.Create(agentId)),
                EntityGuidID.Create(new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E")),
                PersonName.Create("Dough", "Jonnie", "J"),
                SocialSecurityNumber.Create("123789901"),
                PhoneNumber.Create("817-987-1234"),
                Address.Create("123 Main Plaza", null, "Fort Worth", "TX", "78965"),
                MaritalStatus.Create("M"),
                TaxExemption.Create(5),
                PayRate.Create(40M),
                StartDate.Create(new DateTime(1998, 12, 2)),
                true,
                true
            );

            await repo.AddAsync(employee);
            await uow.Commit();

            var employeeResult = await repo.Exists(employee.Id);

            Assert.True(employeeResult);
        }
    }
}