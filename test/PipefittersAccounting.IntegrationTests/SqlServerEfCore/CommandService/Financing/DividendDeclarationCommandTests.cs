using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Application.Commands.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Application.Services;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.Repositories;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.CommandService.Financing
{
    public class DividendDeclarationCommandTests : TestBaseEfCore
    {
        private readonly IStockSubscriptionValidationService _validationService;
        private readonly IStockSubscriptionAggregateRepository _repository;
        private readonly AppUnitOfWork _unitOfWork;

        public DividendDeclarationCommandTests()
        {
            IStockSubscriptionQueryService queryService = new StockSubscriptionQueryService(_dapperCtx);
            IQueryServicesRegistry registry = new QueryServicesRegistry();
            _repository = new StockSubscriptionAggregateRepository(_dbContext);
            _validationService = new StockSubscriptionValidationService(queryService, registry);
            _unitOfWork = new AppUnitOfWork(_dbContext);
        }

        [Fact]
        public async Task AddDividendDeclaration_StockSubscription_ShouldSucceed()
        {
            Guid stockId = new Guid("62d6e2e6-215d-4157-b7ec-1ba9b137c770");

            Guid dividendId = new Guid("341237b0-6122-4763-ac5f-d394234c7213");
            DateTime dividendDeclarationDate = new DateTime(2022, 6, 1);
            decimal dividendPerShare = .01M;
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            OperationResult<StockSubscription> getResult = await _repository.GetStockSubscriptionByIdAsync(stockId);
            StockSubscription subscription = getResult.Result;

            subscription.AddDividendDeclaration(dividendId, dividendDeclarationDate, dividendPerShare, userId);
            _repository.UpdateStockSubscription(subscription);
            await _unitOfWork.Commit();

            OperationResult<DividendDeclaration> result = await _repository.GetDividendDeclarationByIdAsync(dividendId);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task AddDividendDeclaration_StockSubscription_InvalidDividendId_ShouldFail()
        {
            Guid stockId = new Guid("62d6e2e6-215d-4157-b7ec-1ba9b137c770");

            Guid dividendId = Guid.Empty;
            DateTime dividendDeclarationDate = new DateTime(2022, 6, 1);
            decimal dividendPerShare = .01M;
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            OperationResult<StockSubscription> getResult = await _repository.GetStockSubscriptionByIdAsync(stockId);
            StockSubscription subscription = getResult.Result;

            OperationResult<bool> addResult = subscription.AddDividendDeclaration(dividendId, dividendDeclarationDate, dividendPerShare, userId);

            Assert.False(addResult.Success);
        }

        [Fact]
        public async Task AddDividendDeclaration_StockSubscription_InvalidDeclarationDate_ShouldFail()
        {
            Guid stockId = new Guid("62d6e2e6-215d-4157-b7ec-1ba9b137c770");

            Guid dividendId = new Guid("341237b0-6122-4763-ac5f-d394234c7213");
            DateTime dividendDeclarationDate = new DateTime(2022, 1, 2);
            decimal dividendPerShare = .01M;
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            OperationResult<StockSubscription> getResult = await _repository.GetStockSubscriptionByIdAsync(stockId);
            StockSubscription subscription = getResult.Result;

            OperationResult<bool> addResult = subscription.AddDividendDeclaration(dividendId, dividendDeclarationDate, dividendPerShare, userId);

            Assert.False(addResult.Success);
        }

        [Fact]
        public async Task EditDividendDeclaration_StockSubscription_ShouldSucceed()
        {
            Guid stockId = new Guid("62d6e2e6-215d-4157-b7ec-1ba9b137c770");

            Guid dividendId = new Guid("ff0dc77f-7f80-426a-bc24-09d3c10a957f");
            DateTime dividendDeclarationDate = new DateTime(2022, 6, 5);
            decimal dividendPerShare = .01M;
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            OperationResult<StockSubscription> getResult = await _repository.GetStockSubscriptionByIdAsync(stockId);
            Assert.True(getResult.Success);
            StockSubscription subscription = getResult.Result;

            OperationResult<bool> result = subscription.EditDividendDeclaration(dividendId, dividendDeclarationDate, dividendPerShare, userId);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task EditDividendDeclaration_StockSubscription_InvalidDeclarationDate_ShouldFail()
        {
            Guid stockId = new Guid("264632b4-20bd-473f-9a9b-dd6f3b6ddbac");

            Guid dividendId = new Guid("2558ab00-118c-4b67-a6d0-1b9888f841bc");
            DateTime dividendDeclarationDate = new DateTime(2022, 1, 31);
            decimal dividendPerShare = .05M;
            Guid userId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");

            OperationResult<StockSubscription> getResult = await _repository.GetStockSubscriptionByIdAsync(stockId);
            Assert.True(getResult.Success);
            StockSubscription subscription = getResult.Result;

            OperationResult<bool> result = subscription.EditDividendDeclaration(dividendId, dividendDeclarationDate, dividendPerShare, userId);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Process_DividendDeclarationCreateCommand_ShouldSucceed()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForCreate();
            OperationResult<bool> result = await DividendDeclarationCreateCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task Process_DividendDeclarationCreateCommand_InvalidStockId_ShouldFail()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForCreate();
            model.StockId = new Guid("1c967462-140a-4e08-9ba2-04ff760bb1d9");

            OperationResult<bool> result = await DividendDeclarationCreateCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Process_DividendDeclarationCreateCommand_ProceedsNotRcvd_ShouldFail()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForCreate();
            model.StockId = new Guid("971bb315-9d40-4c87-b43b-359b33c31354");

            OperationResult<bool> result = await DividendDeclarationCreateCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Process_DividendDeclarationEditCommand_NotPaid_ShouldSucceed()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForEditNotPaid();
            OperationResult<bool> result = await DividendDeclarationEditCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task Process_DividendDeclarationEditCommand_NotPaid__InvalidDeclarationDate_ShouldFail()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForEditNotPaid();
            model.DividendDeclarationDate = new DateTime(2022, 1, 2);

            OperationResult<bool> result = await DividendDeclarationEditCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Process_DividendDeclarationEditCommand_Paid_ShouldFail()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForEditPaid();
            OperationResult<bool> result = await DividendDeclarationEditCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Process_DividendDeclarationDeleteCommand_NotPaid_ShouldSucceed()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForEditNotPaid();
            OperationResult<bool> result = await DividendDeclarationDeleteCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task Process_DividendDeclarationDeleteCommand_Paid_ShouldFail()
        {
            DividendDeclarationWriteModel model = StockSubscriptionTestData.GetDividendDeclarationWriteModelForEditPaid();
            OperationResult<bool> result = await DividendDeclarationDeleteCommand.Process(model, _repository, _validationService, _unitOfWork);

            Assert.False(result.Success);
        }
    }
}