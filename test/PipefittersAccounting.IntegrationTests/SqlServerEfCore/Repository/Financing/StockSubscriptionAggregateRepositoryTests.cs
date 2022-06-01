#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8625

using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.Repositories.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerEfCore.Repository.Financing
{
    [Trait("Integration", "EfCoreRepo")]
    public class StockSubscriptionAggregateRepositoryTests : TestBaseEfCore
    {
        private readonly IStockSubscriptionAggregateRepository _repository;

        public StockSubscriptionAggregateRepositoryTests()
            => _repository = new StockSubscriptionAggregateRepository(_dbContext);

        [Fact]
        public async Task DoesStockSubscriptionExist_StockSubscriptionAggregateRepository_ShouldReturnTrue()
        {
            Guid acctId = new Guid("264632b4-20bd-473f-9a9b-dd6f3b6ddbac");
            OperationResult<bool> result = await _repository.DoesStockSubscriptionExist(acctId);

            Assert.True(result.Result);
        }

        [Fact]
        public async Task DoesStockSubscriptionExist_StockSubscriptionAggregateRepository_ShouldReturnFalse()
        {
            Guid stockId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");
            OperationResult<bool> result = await _repository.DoesStockSubscriptionExist(stockId);

            Assert.False(result.Result);
        }

        [Fact]
        public async Task GetStockSubscriptionByIdAsync_StockSubscriptionAggregateRepository_ShouldSucceed()
        {
            Guid stockId = new Guid("62d6e2e6-215d-4157-b7ec-1ba9b137c770");
            OperationResult<StockSubscription> result = await _repository.GetStockSubscriptionByIdAsync(stockId);

            Assert.NotNull(result.Result);

            Assert.Equal(1.00M, result.Result.PricePerShare);
        }

        [Fact]
        public async Task GetStockSubscriptionByIdAsync_StockSubscriptionAggregateRepository_ShouldFail()
        {
            Guid stockId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");
            OperationResult<StockSubscription> result = await _repository.GetStockSubscriptionByIdAsync(stockId);

            Assert.Null(result.Result);
        }

        [Fact]
        public async Task AddStockSubscriptionAsync_StockSubscriptionAggregateRepository_ShouldSucceed()
        {
            StockSubscription subscription = StockSubscriptionTestData.CreateStockSubscriptionValidInfo();

            OperationResult<bool> result = await _repository.AddStockSubscriptionAsync(subscription);

            Assert.True(result.Success);

            OperationResult<bool> searchResult = await _repository.DoesStockSubscriptionExist(subscription.Id);
            Assert.True(searchResult.Result);
        }

        [Fact]
        public async Task AddStockSubscriptionAsync_StockSubscriptionAggregateRepository_DuplicateStockId_ShouldFail()
        {
            StockSubscription subscription = StockSubscriptionTestData.CreateStockSubscriptionDuplicateStockId();

            OperationResult<bool> result = await _repository.AddStockSubscriptionAsync(subscription);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task UpdateStockSubscriptionAsync_StockSubscriptionAggregateRepository_ShouldSucceed()
        {
            Guid stockId = new Guid("971bb315-9d40-4c87-b43b-359b33c31354");
            OperationResult<StockSubscription> result = await _repository.GetStockSubscriptionByIdAsync(stockId);
            StockSubscription subscription = result.Result;

            subscription.UpdateStockIssueDate(StockIssueDate.Create(new DateTime(2022, 5, 28)));
            subscription.UpdateSharesIssured(SharesIssured.Create(5800));
            subscription.UpdatePricePerShare(PricePerShare.Create(1.22M));

            OperationResult<bool> updateResult = _repository.UpdateStockSubscription(subscription);

            Assert.True(updateResult.Success);
        }

        [Fact]
        public async Task DeleteStockSubscriptionAsync_StockSubscriptionAggregateRepository_ShouldSucceed()
        {
            Guid stockId = new Guid("971bb315-9d40-4c87-b43b-359b33c31354");
            OperationResult<bool> deleteResult = await _repository.DeleteStockSubscriptionAsync(stockId);
            await _dbContext.SaveChangesAsync();

            Assert.True(deleteResult.Success);

            OperationResult<StockSubscription> result = await _repository.GetStockSubscriptionByIdAsync(stockId);
            Assert.False(result.Success);
        }
    }
}