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

        [Fact]
        public async Task VerifyStockSubscriptionIsUnique_StockSubscriptionQueryService_Unique()
        {
            Guid stockId = Guid.Empty;
            Guid financierId = new Guid("01da50f9-021b-4d03-853a-3fd2c95e207d");
            DateTime stockIssueDate = new DateTime(2022, 4, 2);
            int sharesIssured = 12500;
            decimal pricePerShare = 1.25M;

            UniqueStockSubscriptionParameters queryParameters =
                new()
                {
                    FinancierId = financierId,
                    StockIssueDate = stockIssueDate,
                    SharesIssued = sharesIssured,
                    PricePerShare = pricePerShare
                };

            OperationResult<Guid> result = await _queryService.VerifyStockSubscriptionIsUnique(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(stockId, result.Result);
        }

        [Fact]
        public async Task VerifyStockSubscriptionIsUnique_StockSubscriptionQueryService_NotUnique()
        {
            Guid stockId = new Guid("5997f125-bfca-4540-a144-01e444f6dc25");
            Guid financierId = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            DateTime stockIssueDate = new DateTime(2022, 4, 2);
            int sharesIssured = 12500;
            decimal pricePerShare = 1.25M;

            UniqueStockSubscriptionParameters queryParameters =
                new()
                {
                    FinancierId = financierId,
                    StockIssueDate = stockIssueDate,
                    SharesIssued = sharesIssured,
                    PricePerShare = pricePerShare
                };

            OperationResult<Guid> result = await _queryService.VerifyStockSubscriptionIsUnique(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(stockId, result.Result);
        }

        [Fact]
        public async Task VerifyCashDepositOfStockIssueProceeds_StockSubscriptionQueryService_DepositExist()
        {
            Guid stockId = new Guid("5997f125-bfca-4540-a144-01e444f6dc25");
            Guid financierId = new Guid("12998229-7ede-4834-825a-0c55bde75695");

            VerifyCashDepositOfStockIssueProceedsParameters queryParameters =
                new()
                {
                    FinancierId = financierId,
                    StockId = stockId
                };

            OperationResult<VerificationOfCashDepositStockIssueProceeds> result =
                await _queryService.VerifyCashDepositOfStockIssueProceeds(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(new DateTime(2022, 4, 2), result.Result.DateReceived);
            Assert.Equal(15625M, result.Result.AmountReceived);
        }

        [Fact]
        public async Task VerifyCashDepositOfStockIssueProceeds_StockSubscriptionQueryService_DepositDoesNotExist()
        {
            Guid stockId = new Guid("971bb315-9d40-4c87-b43b-359b33c31354");
            Guid financierId = new Guid("12998229-7ede-4834-825a-0c55bde75695");

            VerifyCashDepositOfStockIssueProceedsParameters queryParameters =
                new()
                {
                    FinancierId = financierId,
                    StockId = stockId
                };

            OperationResult<VerificationOfCashDepositStockIssueProceeds> result =
                await _queryService.VerifyCashDepositOfStockIssueProceeds(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(new DateTime(), result.Result.DateReceived);
            Assert.Equal(0, result.Result.AmountReceived);
        }

        [Fact]
        public async Task VerifyCashDepositOfStockIssueProceeds_StockSubscriptionQueryService_InvalidFinancierId()
        {
            Guid stockId = new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a");
            Guid financierId = new Guid("12998229-7ede-4834-825a-0c55bde75695");

            VerifyCashDepositOfStockIssueProceedsParameters queryParameters =
                new()
                {
                    FinancierId = financierId,
                    StockId = stockId
                };

            OperationResult<VerificationOfCashDepositStockIssueProceeds> result =
                await _queryService.VerifyCashDepositOfStockIssueProceeds(queryParameters);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task VerifyCashDepositOfStockIssueProceeds_StockSubscriptionQueryService_InvalidStockId()
        {
            Guid stockId = new Guid("62d6e2e6-215d-4157-b7ec-1ba9b137c770");
            Guid financierId = new Guid("12998229-7ede-4834-825a-0c55bde75695");

            VerifyCashDepositOfStockIssueProceedsParameters queryParameters =
                new()
                {
                    FinancierId = financierId,
                    StockId = stockId
                };

            OperationResult<VerificationOfCashDepositStockIssueProceeds> result =
                await _queryService.VerifyCashDepositOfStockIssueProceeds(queryParameters);

            Assert.False(result.Success);
        }
    }
}