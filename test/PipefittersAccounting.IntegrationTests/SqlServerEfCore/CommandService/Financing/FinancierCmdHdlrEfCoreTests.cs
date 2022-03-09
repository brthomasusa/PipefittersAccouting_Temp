#pragma warning disable CS8600
#pragma warning disable CS8602

using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Core.Financing.FinancierAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing;
using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.CommandService.Financing
{
    [Trait("Integration", "EfCoreCmdSvc")]
    public class FinancierCmdHdlrEfCoreTests : TestBaseEfCore
    {
        [Fact]
        public async Task CreateFinancierInfo_WithValidInfo_ShouldSucceed()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);
            IFinancierCommandService cmdSvc = new FinancierCommandService(repo, uow);

            CreateFinancierInfo model = TestUtilities.GetCreateFinancierInfo();

            OperationResult<bool> result = await cmdSvc.CreateFinancierInfo(model);
            Assert.True(result.Success);

            var newEmployee = await repo.GetByIdAsync(model.Id);

            Assert.NotNull(newEmployee);
        }

        [Fact]
        public async Task CreateFinancierInfo_FinancierAlreadyExists_ShouldFail()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);
            IFinancierCommandService cmdSvc = new FinancierCommandService(repo, uow);

            CreateFinancierInfo model = TestUtilities.GetCreateFinancierInfo();
            model.Id = new Guid("12998229-7ede-4834-825a-0c55bde75695");

            OperationResult<bool> result = await cmdSvc.CreateFinancierInfo(model);

            Assert.False(result.Success);
            Assert.Equal($"Create operation failed! A financier with this Id: {model.Id} already exists!", result.NonSuccessMessage);
        }

        [Fact]
        public async Task CreateFinancierInfo_WithDuplicateFinancierName_ShouldFail()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);
            IFinancierCommandService cmdSvc = new FinancierCommandService(repo, uow);

            CreateFinancierInfo model = TestUtilities.GetCreateFinancierInfo();
            model.FinancierName = "New World Tatoo Parlor";

            OperationResult<bool> result = await cmdSvc.CreateFinancierInfo(model);

            Assert.False(result.Success);
            Assert.Equal($"A financier with name: {model.FinancierName} is already in the database.", result.NonSuccessMessage);
        }

        [Fact]
        public async Task EdiFinancierInfo_WithValidInfo_ShouldSucceed()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);
            IFinancierCommandService cmdSvc = new FinancierCommandService(repo, uow);

            EditFinancierInfo model = TestUtilities.GetEditFinancierInfo();

            OperationResult<bool> result = await cmdSvc.EditFinancierInfo(model);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task EdiFinancierInfo_WithDuplicateFinanciername_ShouldFail()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);
            IFinancierCommandService cmdSvc = new FinancierCommandService(repo, uow);

            EditFinancierInfo model = TestUtilities.GetEditFinancierInfo();
            model.FinancierName = "New World Tatoo Parlor";

            OperationResult<bool> result = await cmdSvc.EditFinancierInfo(model);

            Assert.False(result.Success);
            Assert.Equal($"A financier with name: {model.FinancierName} is already in the database.", result.NonSuccessMessage);
        }

        [Fact]
        public async Task EditEmployeeInfo_WithInvalidEmployeeID_ShouldFail()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);
            IFinancierCommandService cmdSvc = new FinancierCommandService(repo, uow);

            EditFinancierInfo model = TestUtilities.GetEditFinancierInfo();
            model.Id = Guid.NewGuid();

            OperationResult<bool> result = await cmdSvc.EditFinancierInfo(model);

            Assert.False(result.Success);
            Assert.Equal($"Update failed, a financier with id: {model.Id} could not be found!", result.NonSuccessMessage);
        }

        [Fact]
        public async Task EditEmployeeInfo_WithBadInputData_ShouldFail()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);
            IFinancierCommandService cmdSvc = new FinancierCommandService(repo, uow);

            EditFinancierInfo model = TestUtilities.GetEditFinancierInfo();
            model.Telephone = "214-55-666";

            OperationResult<bool> result = await cmdSvc.EditFinancierInfo(model);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task DeleteFinancierInfo_WithValidFinancierId_ShouldSucceed()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);
            IFinancierCommandService cmdSvc = new FinancierCommandService(repo, uow);

            Guid agentId = new Guid("84164388-28ff-4b47-bd63-dd9326d32236");
            Financier financier = await repo.GetByIdAsync(agentId);
            Assert.NotNull(financier);

            DeleteFinancierInfo model = new() { Id = agentId };


            OperationResult<bool> result = await cmdSvc.DeleteFinancierInfo(model);
            Assert.True(result.Success);

            financier = await repo.GetByIdAsync(agentId);
            Assert.Null(financier);
        }

        [Fact]
        public async Task DeleteFinancierInfo_WithInvalidFinancierID_ShouldFail()
        {
            AppUnitOfWork uow = new AppUnitOfWork(_dbContext);
            IFinancierAggregateRepository repo = new FinancierAggregateRepository(_dbContext);
            IFinancierCommandService cmdSvc = new FinancierCommandService(repo, uow);

            DeleteFinancierInfo model = new() { Id = Guid.NewGuid() };

            OperationResult<bool> result = await cmdSvc.DeleteFinancierInfo(model);

            Assert.False(result.Success);
            Assert.Equal($"Delete failed, a financier with id: {model.Id} could not be found!", result.NonSuccessMessage);
        }
    }
}