using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    [Trait("Integration", "DapperQueryService")]
    public class StockSubscriptionQueryServiceTests : TestBaseDapper
    {
        IStockSubscriptionQueryService _queryService;
        public StockSubscriptionQueryServiceTests()
            => _queryService = new StockSubscriptionQueryService(_dapperCtx);

        [Fact]
        public async Task GetStockSubscriptionDetails_StockSubscriptionQueryService_ShouldSucceed()
        {
            GetStockSubscriptionParameters queryParameters = new() { StockId = new Guid("62d6e2e6-215d-4157-b7ec-1ba9b137c770") };
            OperationResult<StockSubscriptionDetails> result = await _queryService.GetStockSubscriptionDetails(queryParameters);

            Assert.True(result.Success);
            Assert.Equal("New World Tatoo Parlor", result.Result.InvestorName);
        }

        [Fact]
        public async Task GetCashAccountListItems_StockSubscriptionQueryService_ShouldSucceed()
        {
            GetStockSubscriptionListItemParameters queryParameters = new() { Page = 1, PageSize = 10 };
            OperationResult<PagedList<StockSubscriptionListItem>> result = await _queryService.GetCashAccountListItems(queryParameters);

            Assert.True(result.Success);

            int records = result.Result.Count;
            Assert.Equal(7, records);
        }
    }
}