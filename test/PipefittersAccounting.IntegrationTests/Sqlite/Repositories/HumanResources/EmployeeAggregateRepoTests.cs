#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8625

using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Core.Interfaces.HumanResources;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.HumanResources;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
namespace PipefittersAccounting.IntegrationTests.Sqlite.Repositories.HumanResources
{
    public class EmployeeAggregateRepoTests
    {
        [Fact]
        public async Task RepositoryAdd_ExternalAgentAndEmployee()
        {
            SqliteDbContextFactory factory = new();
            AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            IEmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);

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

            var employeeResult = await context.Employees.FindAsync(agentId);

            Assert.Equal("Dough", employeeResult.EmployeeName.LastName);
        }

        [Fact]
        public async Task RepositoryGetByID_Employee()
        {
            SqliteDbContextFactory factory = new();
            AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            IEmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);

            Guid agentId = new Guid("0cf9de54-c2ca-417e-827c-a5b87be2d788");

            Employee employee = await repo.GetByIdAsync(agentId);
            Employee result = await context.Employees.FindAsync(agentId);

            Assert.Equal(employee.EmployeeName, result.EmployeeName);
        }

        [Fact]
        public async Task RepositoryExists_Employee_true()
        {
            SqliteDbContextFactory factory = new();
            AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            IEmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);

            Guid agentId = new Guid("0cf9de54-c2ca-417e-827c-a5b87be2d788");

            bool result = await repo.Exists(agentId);
            Assert.True(result);

            Employee employee = await context.Employees.FindAsync(agentId);
            Assert.NotNull(employee);
        }

        [Fact]
        public async Task RepositoryExists_Employee_false()
        {
            SqliteDbContextFactory factory = new();
            AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            IEmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);

            Guid agentId = new Guid("1234de54-c2ca-417e-827c-a5b87be2d788");

            bool result = await repo.Exists(agentId);
            Assert.False(result);

            Employee employee = await context.Employees.FindAsync(agentId);
            Assert.Null(employee);
        }

        [Fact]
        public async Task RepositoryUpdate_Employee()
        {
            SqliteDbContextFactory factory = new();
            AppDbContext context = factory.CreateInMemoryContext();

            IEmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);

            Guid agentId = new Guid("0cf9de54-c2ca-417e-827c-a5b87be2d788");

            Employee employee = await context.Employees.FindAsync(agentId);
            Assert.Equal("Brown", employee.EmployeeName.LastName);
            Assert.Equal("Jamie", employee.EmployeeName.FirstName);

            PersonName newName = PersonName.Create("Garbor", "ZarZar", "Z");
            employee.UpdateEmployeeName(newName);

            repo.Update(employee);

            Employee result = await context.Employees.FindAsync(agentId);
            Assert.Equal("Garbor", result.EmployeeName.LastName);
            Assert.Equal("ZarZar", result.EmployeeName.FirstName);
        }

        [Fact]
        public async Task RepositoryDelete_Employee()
        {
            SqliteDbContextFactory factory = new();
            AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            IEmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);

            Guid agentId = new Guid("0cf9de54-c2ca-417e-827c-a5b87be2d788");

            Employee employee = await context.Employees.FindAsync(agentId);
            Assert.NotNull(employee);
            ExternalAgent agent = await context.ExternalAgents.FindAsync(agentId);
            Assert.NotNull(agent);

            repo.Delete(employee);
            await uow.Commit();

            employee = await context.Employees.FindAsync(agentId);
            Assert.Null(employee);
            agent = await context.ExternalAgents.FindAsync(agentId);
            Assert.Null(agent);
        }

        [Fact]
        public async Task CheckForDuplicateEmployeeName_ExistingName_ReturnsNonEmptyGuid()
        {
            SqliteDbContextFactory factory = new();
            AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            IEmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);

            Guid agentId = new Guid("0cf9de54-c2ca-417e-827c-a5b87be2d788");
            OperationResult<Guid> result = await repo.CheckForDuplicateEmployeeName("Brown", "Jamie", "J");

            Assert.Equal(agentId, result.Result);
        }

        [Fact]
        public async Task CheckForDuplicateEmployeeName_CaseInsensitive_ReturnsNonEmptyGuid()
        {
            SqliteDbContextFactory factory = new();
            AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            IEmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);

            Guid agentId = new Guid("0cf9de54-c2ca-417e-827c-a5b87be2d788");
            OperationResult<Guid> result = await repo.CheckForDuplicateEmployeeName("BROWN", "JamiE", "J");

            Assert.Equal(agentId, result.Result);
        }

        [Fact]
        public async Task CheckForDuplicateEmployeeName_NonExistingName_ReturnsEmptyGuid()
        {
            SqliteDbContextFactory factory = new();
            AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            IEmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);

            OperationResult<Guid> result = await repo.CheckForDuplicateEmployeeName("Browne", "Jamie", "J");

            Assert.True(result.Result.Equals(default));
        }

        [Fact]
        public async Task CheckForDuplicateSSN_NonExistingSSN_ReturnsEmptyGuid()
        {
            SqliteDbContextFactory factory = new();
            AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            IEmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);

            OperationResult<Guid> result = await repo.CheckForDuplicateSSN("723452567");

            Assert.True(result.Result.Equals(default));
        }

        [Fact]
        public async Task CheckForDuplicateSSN_ExistingSSN_ReturnsNonEmptyGuid()
        {
            SqliteDbContextFactory factory = new();
            AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            IEmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);

            Guid agentId = new Guid("0cf9de54-c2ca-417e-827c-a5b87be2d788");
            OperationResult<Guid> result = await repo.CheckForDuplicateSSN("123700009");

            Assert.Equal(agentId, result.Result);
        }
    }
}