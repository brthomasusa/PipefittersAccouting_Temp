using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Application.Commands.Financing;
using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate;
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
    public class DividendDeclarationCommandTests : TestBaseEfCore
    {
        private readonly IStockSubscriptionValidationService _validationService;
        private readonly IStockSubscriptionAggregateRepository _repository;
        private readonly AppUnitOfWork _unitOfWork;

        public DividendDeclarationCommandTests()
        {
            IStockSubscriptionQueryService queryService = new StockSubscriptionQueryService(_dapperCtx);
            _repository = new StockSubscriptionAggregateRepository(_dbContext);
            _validationService = new StockSubscriptionValidationService(queryService);
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
    }
}