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
using PipefittersAccounting.Infrastructure.Application.Services.HumanResources;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.CommandHandlers.HumanResources
{
    public class EmployeeAggregateCmdHdlrEfCoreTests : TestBaseEfCore
    {
        [Fact]
        public async Task CreateEmployeeInfo_WithValidInfo_ShouldSucceed()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);
            IEmployeeAggregateCommandService cmdHdlr = new EmployeeCommandServiceEfCore(repo, uow);

            CreateEmployeeInfo model = TestUtilities.GetCreateEmployeeInfo();

            OperationResult<bool> result = await cmdHdlr.CreateEmployeeInfo(model);
            Assert.True(result.Success);

            var newEmployee = await _dbContext.Employees.FindAsync(model.Id);

            Assert.NotNull(newEmployee);
        }

        [Fact]
        public async Task CreateEmployeeInfo_EmployeeAlreadyExists_ShouldFail()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);
            IEmployeeAggregateCommandService cmdHdlr = new EmployeeCommandServiceEfCore(repo, uow);

            CreateEmployeeInfo model = TestUtilities.GetCreateEmployeeInfo();
            model.Id = new Guid("4B900A74-E2D9-4837-B9A4-9E828752716E");

            OperationResult<bool> result = await cmdHdlr.CreateEmployeeInfo(model);
            Assert.False(result.Success);
            Assert.NotNull(result.Exception);
            Assert.IsType<InvalidOperationException>(result.Exception);
            Assert.Equal("Can not create this employee, they already exists!", result.Exception.Message);
        }

        [Fact]
        public async Task CreateEmployeeInfo_WithDuplicateEmployeeName_ShouldFail()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);
            IEmployeeAggregateCommandService cmdHdlr = new EmployeeCommandServiceEfCore(repo, uow);

            CreateEmployeeInfo model = TestUtilities.GetCreateEmployeeInfo();
            model.FirstName = "Jozef";
            model.LastName = "Goldberg";
            model.MiddleInitial = "P";

            OperationResult<bool> result = await cmdHdlr.CreateEmployeeInfo(model);
            Assert.False(result.Success);
            Assert.NotNull(result.NonSuccessMessage);
            string errMsg = $"An employee name {model.FirstName} {model.MiddleInitial} {model.LastName} is already in the database.";
            Assert.Equal(errMsg, result.NonSuccessMessage);
        }

        [Fact]
        public async Task CreateEmployeeInfo_WithDuplicateEmployeeSSN_ShouldFail()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);
            IEmployeeAggregateCommandService cmdHdlr = new EmployeeCommandServiceEfCore(repo, uow);

            CreateEmployeeInfo model = TestUtilities.GetCreateEmployeeInfo();
            model.SSN = "775559874";

            OperationResult<bool> result = await cmdHdlr.CreateEmployeeInfo(model);
            Assert.False(result.Success);
            Assert.NotNull(result.NonSuccessMessage);
            string errMsg = $"An employee with social security number: {model.SSN} is already in the database.";
            Assert.Equal(errMsg, result.NonSuccessMessage);
        }

        [Fact]
        public async Task EditEmployeeInfo_WithValidInfo_ShouldPass()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);
            IEmployeeAggregateCommandService cmdHdlr = new EmployeeCommandServiceEfCore(repo, uow);

            EditEmployeeInfo model = TestUtilities.GetEditEmployeeInfo();

            OperationResult<bool> result = await cmdHdlr.EditEmployeeInfo(model);
            Assert.True(result.Success);

            var updatedEmployee = await _dbContext.Employees.FindAsync(model.Id);

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
        public async Task EditEmployeeInfo_WithDuplicateEmployeeName_ShouldFail()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);
            IEmployeeAggregateCommandService cmdHdlr = new EmployeeCommandServiceEfCore(repo, uow);

            EditEmployeeInfo model = TestUtilities.GetEditEmployeeInfo();
            model.FirstName = "Terri";
            model.LastName = "Duffy";
            model.MiddleInitial = "L";

            OperationResult<bool> result = await cmdHdlr.EditEmployeeInfo(model);

            Assert.False(result.Success);
            Assert.NotNull(result.NonSuccessMessage);
            string errMsg = $"An employee name {model.FirstName} {model.MiddleInitial} {model.LastName} is already in the database.";
            Assert.Equal(errMsg, result.NonSuccessMessage);
        }

        [Fact]
        public async Task EditEmployeeInfo_WithDuplicateEmployeeSSN_ShouldFail()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);
            IEmployeeAggregateCommandService cmdHdlr = new EmployeeCommandServiceEfCore(repo, uow);

            EditEmployeeInfo model = TestUtilities.GetEditEmployeeInfo();
            model.SSN = "775559874";

            OperationResult<bool> result = await cmdHdlr.EditEmployeeInfo(model);

            Assert.False(result.Success);
            Assert.NotNull(result.NonSuccessMessage);
            string errMsg = $"An employee with social security number: {model.SSN} is already in the database.";
            Assert.Equal(errMsg, result.NonSuccessMessage);
        }

        [Fact]
        public async Task EditEmployeeInfo_WithDuplicateEmployeeID_ShouldFail()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);
            IEmployeeAggregateCommandService cmdHdlr = new EmployeeCommandServiceEfCore(repo, uow);

            EditEmployeeInfo model = TestUtilities.GetEditEmployeeInfo();
            model.Id = Guid.NewGuid();

            OperationResult<bool> result = await cmdHdlr.EditEmployeeInfo(model);

            Assert.False(result.Success);
            Assert.NotNull(result.Exception);
            Assert.IsType<ArgumentException>(result.Exception);
        }

        [Fact]
        public async Task EditEmployeeInfo_WithBadInputData_ShouldFail()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);
            IEmployeeAggregateCommandService cmdHdlr = new EmployeeCommandServiceEfCore(repo, uow);

            EditEmployeeInfo model = TestUtilities.GetEditEmployeeInfo();
            model.Telephone = "2144-897-99";

            OperationResult<bool> result = await cmdHdlr.EditEmployeeInfo(model);

            Assert.False(result.Success);
            Assert.NotNull(result.Exception);
        }

        [Fact]
        public async Task DeleteEmployeeInfo_WithValidEmployeeId_ShouldSucceed()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);
            IEmployeeAggregateCommandService cmdHdlr = new EmployeeCommandServiceEfCore(repo, uow);

            Guid agentId = new Guid("e6b86ea3-6479-48a2-b8d4-54bd6cbbdbc5");
            Employee employee = await _dbContext.Employees.FindAsync(agentId);
            Assert.NotNull(employee);

            DeleteEmployeeInfo model = new DeleteEmployeeInfo()
            {
                Id = agentId
            };

            OperationResult<bool> result = await cmdHdlr.DeleteEmployeeInfo(model);
            Assert.True(result.Success);

            employee = await _dbContext.Employees.FindAsync(agentId);
            Assert.Null(employee);
        }

        [Fact]
        public async Task DeleteEmployeeInfo_WithInvalidEmployeeID_ShouldFail()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);
            IEmployeeAggregateCommandService cmdHdlr = new EmployeeCommandServiceEfCore(repo, uow);

            DeleteEmployeeInfo model = new DeleteEmployeeInfo()
            {
                Id = Guid.NewGuid()
            };

            OperationResult<bool> result = await cmdHdlr.DeleteEmployeeInfo(model);
            Assert.False(result.Success);
            Assert.NotNull(result.Exception);
        }
    }
}