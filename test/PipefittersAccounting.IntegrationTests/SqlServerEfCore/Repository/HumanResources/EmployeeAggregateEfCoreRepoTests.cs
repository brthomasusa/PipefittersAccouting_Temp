#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8625

using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Core.HumanResources.EmployeeAggregate;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.HumanResources;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.Repository.HumanResources
{
    [Trait("Integration", "EfCoreRepo")]
    public class EmployeeAggregateEfCoreRepoTests : TestBaseEfCore
    {
        [Fact]
        public async Task EfCore_RepositoryAdd_ExternalAgentAndEmployee()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);

            Employee employee = TestUtilities.GetNewEmployee();
            Guid agentId = employee.Id;

            await repo.AddAsync(employee);
            await uow.Commit();

            var employeeResult = await _dbContext.Employees.FindAsync(agentId);

            Assert.Equal("Dough", employeeResult.EmployeeName.LastName);
        }

        [Fact]
        public async Task EfCore_RepositoryGetByID_Employee()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);

            Guid agentId = new Guid("0cf9de54-c2ca-417e-827c-a5b87be2d788");

            Employee employee = await repo.GetByIdAsync(agentId);
            Employee result = await _dbContext.Employees.FindAsync(agentId);

            Assert.Equal(employee.EmployeeName, result.EmployeeName);
        }

        [Fact]
        public async Task EfCore_RepositoryExists_Employee_true()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);

            Guid agentId = new Guid("0cf9de54-c2ca-417e-827c-a5b87be2d788");

            bool result = await repo.Exists(agentId);
            Assert.True(result);

            Employee employee = await _dbContext.Employees.FindAsync(agentId);
            Assert.NotNull(employee);
        }

        [Fact]
        public async Task EfCore_RepositoryExists_Employee_false()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);

            Guid agentId = new Guid("1234de54-c2ca-417e-827c-a5b87be2d788");

            bool result = await repo.Exists(agentId);
            Assert.False(result);

            Employee employee = await _dbContext.Employees.FindAsync(agentId);
            Assert.Null(employee);
        }

        [Fact]
        public async Task EfCoreRepositoryUpdate_Employee()
        {
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);

            Guid agentId = new Guid("0cf9de54-c2ca-417e-827c-a5b87be2d788");

            Employee employee = await _dbContext.Employees.FindAsync(agentId);
            Assert.Equal("Brown", employee.EmployeeName.LastName);
            Assert.Equal("Jamie", employee.EmployeeName.FirstName);

            PersonName newName = PersonName.Create("Garbor", "ZarZar", "Z");
            employee.UpdateEmployeeName(newName);

            repo.Update(employee);

            Employee result = await _dbContext.Employees.FindAsync(agentId);
            Assert.Equal("Garbor", result.EmployeeName.LastName);
            Assert.Equal("ZarZar", result.EmployeeName.FirstName);
        }

        [Fact]
        public async Task EfCore_RepositoryDelete_Employee()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            EmployeeAggregateRepository repo = new EmployeeAggregateRepository(_dbContext);

            Guid agentId = new Guid("0cf9de54-c2ca-417e-827c-a5b87be2d788");

            Employee employee = await _dbContext.Employees.FindAsync(agentId);
            Assert.NotNull(employee);
            ExternalAgent agent = await _dbContext.ExternalAgents.FindAsync(agentId);
            Assert.NotNull(agent);

            repo.Delete(employee);
            await uow.Commit();

            employee = await _dbContext.Employees.FindAsync(agentId);
            Assert.Null(employee);
            agent = await _dbContext.ExternalAgents.FindAsync(agentId);
            Assert.Null(agent);
        }
    }
}