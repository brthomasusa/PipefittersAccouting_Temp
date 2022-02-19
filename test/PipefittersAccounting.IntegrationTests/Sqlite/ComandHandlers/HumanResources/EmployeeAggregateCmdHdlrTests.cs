#pragma warning disable CS8600
#pragma warning disable CS8602

using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.HumanResources;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.HumanResources;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.IntegrationTests.Sqlite.ComandHandlers.HumanResources
{
    public class EmployeeAggregateCmdHdlrTests
    {
        [Fact]
        public async Task CmdHdlr_Add_Employee()
        {
            using SqliteDbContextFactory factory = new();
            using AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);
            IEmployeeAggregateCommandHandlerService cmdHdlr = new EmployeeCommandHandlerServiceSqliteInMemory(repo, uow);

            CreateEmployeeInfo model = GetCreateEmployeeInfo();

            OperationResult<bool> result = await cmdHdlr.CreateEmployeeInfo(model);
            Assert.True(result.Success);

            var newEmployee = await context.Employees.FindAsync(model.Id);

            Assert.NotNull(newEmployee);
        }

        [Fact]
        public async Task CmdHdlr_Add_Employee_Duplicate_ShouldFail()
        {
            using SqliteDbContextFactory factory = new();
            using AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);
            IEmployeeAggregateCommandHandlerService cmdHdlr = new EmployeeCommandHandlerServiceSqliteInMemory(repo, uow);

            CreateEmployeeInfo model = GetCreateEmployeeInfo();
            model.Id = new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E");

            OperationResult<bool> result = await cmdHdlr.CreateEmployeeInfo(model);
            Assert.False(result.Success);
            Assert.NotNull(result.Exception);
            Assert.IsType<InvalidOperationException>(result.Exception);
            Assert.Equal("Can not create this employee, they already exists!", result.Exception.Message);
        }

        [Fact]
        public async Task CmdHdlr_Edit_Employee()
        {
            using SqliteDbContextFactory factory = new();
            using AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);
            IEmployeeAggregateCommandHandlerService cmdHdlr = new EmployeeCommandHandlerServiceSqliteInMemory(repo, uow);

            EditEmployeeInfo model = GetEditEmployeeInfo();

            OperationResult<bool> result = await cmdHdlr.EditEmployeeInfo(model);
            Assert.True(result.Success);

            var updatedEmployee = await context.Employees.FindAsync(model.Id);
            Assert.Equal(model.SupervisorId, updatedEmployee.SupervisorId);
            Assert.Equal(PersonName.Create(model.LastName, model.FirstName, model.MiddleInitial), updatedEmployee.EmployeeName);
            Assert.Equal(PhoneNumber.Create(PhoneNumber.Create(model.Telephone)), updatedEmployee.EmployeeTelephone);
            Assert.Equal(Address.Create(model.AddressLine1, model.AddressLine2, model.City, model.StateCode, model.Zipcode), updatedEmployee.EmployeeAddress);
            Assert.Equal(SocialSecurityNumber.Create(model.SSN), updatedEmployee.SSN);
            Assert.Equal(MaritalStatus.Create(model.MaritalStatus), updatedEmployee.MaritalStatus);
            Assert.Equal(TaxExemption.Create(model.Exemptions), updatedEmployee.TaxExemptions);
            Assert.Equal(PayRate.Create(model.PayRate), updatedEmployee.EmployeePayRate);
            Assert.Equal(StartDate.Create(model.StartDate), updatedEmployee.EmploymentDate);
            Assert.Equal(Status.Create(model.IsActive), updatedEmployee.IsActive);
            Assert.Equal(model.IsSupervisor, updatedEmployee.IsSupervisor);
        }

        [Fact]
        public async Task CmdHdlr_Edit_Employee_BadID_ShouldFail()
        {
            using SqliteDbContextFactory factory = new();
            using AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);
            IEmployeeAggregateCommandHandlerService cmdHdlr = new EmployeeCommandHandlerServiceSqliteInMemory(repo, uow);

            EditEmployeeInfo model = GetEditEmployeeInfo();
            model.Id = Guid.NewGuid();

            OperationResult<bool> result = await cmdHdlr.EditEmployeeInfo(model);

            Assert.False(result.Success);
            Assert.NotNull(result.Exception);
            Assert.IsType<ArgumentException>(result.Exception);

        }

        [Fact]
        public async Task CmdHdlr_Edit_Employee_BadInputData_ShouldFail()
        {
            using SqliteDbContextFactory factory = new();
            using AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);
            IEmployeeAggregateCommandHandlerService cmdHdlr = new EmployeeCommandHandlerServiceSqliteInMemory(repo, uow);

            EditEmployeeInfo model = GetEditEmployeeInfo();
            model.Telephone = "2144-897-99";

            OperationResult<bool> result = await cmdHdlr.EditEmployeeInfo(model);

            Assert.False(result.Success);
            Assert.NotNull(result.Exception);
        }

        [Fact]
        public async Task CmdHdlr_Delete_Employee()
        {
            using SqliteDbContextFactory factory = new();
            using AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);
            IEmployeeAggregateCommandHandlerService cmdHdlr = new EmployeeCommandHandlerServiceSqliteInMemory(repo, uow);

            Guid agentId = new Guid("e6b86ea3-6479-48a2-b8d4-54bd6cbbdbc5");
            Employee employee = await context.Employees.FindAsync(agentId);
            Assert.NotNull(employee);

            DeleteEmployeeInfo model = new DeleteEmployeeInfo()
            {
                Id = agentId
            };

            OperationResult<bool> result = await cmdHdlr.DeleteEmployeeInfo(model);
            Assert.True(result.Success);

            employee = await context.Employees.FindAsync(agentId);
            Assert.Null(employee);
        }

        [Fact]
        public async Task CmdHdlr_Delete_Employee_BadID_ShouldFail()
        {
            using SqliteDbContextFactory factory = new();
            using AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);
            IEmployeeAggregateCommandHandlerService cmdHdlr = new EmployeeCommandHandlerServiceSqliteInMemory(repo, uow);

            DeleteEmployeeInfo model = new DeleteEmployeeInfo()
            {
                Id = Guid.NewGuid()
            };

            OperationResult<bool> result = await cmdHdlr.DeleteEmployeeInfo(model);
            Assert.False(result.Success);
            Assert.NotNull(result.Exception);
        }

        private CreateEmployeeInfo GetCreateEmployeeInfo() =>
            new CreateEmployeeInfo()
            {
                Id = Guid.NewGuid(),
                SupervisorId = new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E"),
                LastName = "Trump",
                FirstName = "Ivanka",
                MiddleInitial = "I",
                SSN = "434679876",
                Telephone = "555-555-5555",
                AddressLine1 = "139th Street NW",
                AddressLine2 = "B1",
                City = "Edison",
                StateCode = "NJ",
                Zipcode = "08837",
                MaritalStatus = "M",
                Exemptions = 3,
                PayRate = 25M,
                StartDate = new DateTime(2022, 2, 13),
                IsActive = true,
                IsSupervisor = false
            };

        private EditEmployeeInfo GetEditEmployeeInfo() =>
            new EditEmployeeInfo()
            {
                Id = new Guid("e6b86ea3-6479-48a2-b8d4-54bd6cbbdbc5"),
                SupervisorId = new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E"),
                LastName = "Trump",
                FirstName = "Ivanka",
                MiddleInitial = "I",
                SSN = "434679876",
                Telephone = "555-555-5555",
                AddressLine1 = "139th Street NW",
                AddressLine2 = "B1",
                City = "Edison",
                StateCode = "NJ",
                Zipcode = "08837",
                MaritalStatus = "M",
                Exemptions = 3,
                PayRate = 25M,
                StartDate = new DateTime(2022, 2, 13),
                IsActive = true,
                IsSupervisor = false
            };

        private EmployeeCommandHandlerServiceSqliteInMemory GetCommandHandler()
        {
            using SqliteDbContextFactory factory = new();
            using AppDbContext context = factory.CreateInMemoryContext();

            AppUnitOfWork uow = new AppUnitOfWork(context);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(context);
            return new EmployeeCommandHandlerServiceSqliteInMemory(repo, uow);
        }
    }
}