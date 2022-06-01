using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Application.Commands.Financing;
using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.CommandService.Financing
{
    public class StockSubscriptionCommandTests : TestBaseEfCore
    {
        private readonly IStockSubscriptionValidationService _validationService;
        private readonly IStockSubscriptionAggregateRepository _repository;
        private readonly AppUnitOfWork _unitOfWork;

        public StockSubscriptionCommandTests()
        {
            IStockSubscriptionQueryService queryService = new StockSubscriptionQueryService(_dapperCtx);
            _repository = new StockSubscriptionAggregateRepository(_dbContext);
            _validationService = new StockSubscriptionValidationService(queryService);
            _unitOfWork = new AppUnitOfWork(_dbContext);
        }

        [Fact]
        public async Task Process_StockSubscriptionCreateCommand_ShouldSucceed()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();

            OperationResult<bool> result = await StockSubscriptionCreateCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task Process_StockSubscriptionCreateCommand_InvalidFinancierId_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.FinancierId = new Guid("bffbcf34-f6ba-4fb2-b70e-ab19d3371886");

            OperationResult<bool> result = await StockSubscriptionCreateCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Process_StockSubscriptionCreateCommand_Duplicate_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.FinancierId = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            model.StockIssueDate = new DateTime(2022, 4, 2);
            model.SharesIssued = 12500;
            model.PricePerShare = 1.25M;

            OperationResult<bool> result = await StockSubscriptionCreateCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Process_StockSubscriptionEditCommand_ShouldSucceed()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();

            OperationResult<bool> result = await StockSubscriptionEditCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task Process_StockSubscriptionEditCommand_InvalidStockId_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.StockId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            OperationResult<bool> result = await StockSubscriptionEditCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Process_StockSubscriptionEditCommand_InvalidFinancierIdCombo_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.FinancierId = new Guid("12353ffb-9983-4cde-b1d6-8a49e785177f");

            OperationResult<bool> result = await StockSubscriptionEditCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Process_StockSubscriptionEditCommand_InvalidStockIdFinancierIdCombo_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.FinancierId = new Guid("bffbcf34-f6ba-4fb2-b70e-ab19d3371886");

            OperationResult<bool> result = await StockSubscriptionEditCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Process_StockSubscriptionEditCommand_Duplicate_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.FinancierId = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            model.StockIssueDate = new DateTime(2022, 4, 2);
            model.SharesIssued = 12500;
            model.PricePerShare = 1.25M;

            OperationResult<bool> result = await StockSubscriptionEditCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Process_StockSubscriptionEditCommand_ExistingDeposit_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithDeposit();

            OperationResult<bool> result = await StockSubscriptionEditCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Process_StockSubscriptionDeleteCommand_ShouldSucceed()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();

            OperationResult<bool> result = await StockSubscriptionDeleteCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task Process_StockSubscriptionDeleteCommand_InvalidStockId_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithNoDeposit();
            model.StockId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            OperationResult<bool> result = await StockSubscriptionDeleteCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Process_StockSubscriptionDeleteCommand_ExistingDeposit_ShouldFail()
        {
            StockSubscriptionWriteModel model = StockSubscriptionTestData.GetStockSubscriptionWriteModel_ExistingWithDeposit();

            OperationResult<bool> result = await StockSubscriptionDeleteCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }
    }
}