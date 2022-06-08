using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Application.Services.Financing.StockSubscriptionAggregate;
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
            GetStockSubscriptionParameter queryParameters = new() { StockId = new Guid("62d6e2e6-215d-4157-b7ec-1ba9b137c770") };
            OperationResult<StockSubscriptionDetails> result = await _queryService.GetStockSubscriptionDetails(queryParameters);

            Assert.True(result.Success);
            Assert.Equal("New World Tatoo Parlor", result.Result.InvestorName);
        }

        [Fact]
        public async Task GetCashAccountListItems_StockSubscriptionQueryService_ShouldSucceed()
        {
            GetStockSubscriptionListItemParameters queryParameters = new() { Page = 1, PageSize = 10 };
            OperationResult<PagedList<StockSubscriptionListItem>> result = await _queryService.GetStockSubscriptionListItems(queryParameters);

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

            GetStockSubscriptionParameter queryParameters =
                new()
                {
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

            GetStockSubscriptionParameter queryParameters =
                new()
                {
                    StockId = stockId
                };

            OperationResult<VerificationOfCashDepositStockIssueProceeds> result =
                await _queryService.VerifyCashDepositOfStockIssueProceeds(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(new DateTime(), result.Result.DateReceived);
            Assert.Equal(0, result.Result.AmountReceived);
        }

        [Fact]
        public async Task VerifyCashDepositOfStockIssueProceeds_StockSubscriptionQueryService_InvalidStockId()
        {
            Guid stockId = new Guid("02d6e2e6-215d-4157-b7ec-1ba9b137c770");

            GetStockSubscriptionParameter queryParameters =
                new()
                {
                    StockId = stockId
                };

            OperationResult<VerificationOfCashDepositStockIssueProceeds> result =
                await _queryService.VerifyCashDepositOfStockIssueProceeds(queryParameters);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task VerifyStockSubscriptionIdentification_StockSubscriptionQueryService_ValidStockId()
        {
            Guid stockId = new Guid("62d6e2e6-215d-4157-b7ec-1ba9b137c770");

            GetStockSubscriptionParameter queryParameters =
                new()
                {
                    StockId = stockId
                };

            OperationResult<Guid> result =
                await _queryService.VerifyStockSubscriptionIdentification(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(stockId, result.Result);
        }

        [Fact]
        public async Task VerifyStockSubscriptionIdentification_StockSubscriptionQueryService_InvalidStockId()
        {
            Guid stockId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            GetStockSubscriptionParameter queryParameters =
                new()
                {
                    StockId = stockId
                };

            OperationResult<Guid> result =
                await _queryService.VerifyStockSubscriptionIdentification(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(Guid.Empty, result.Result);
        }

        [Fact]
        public async Task VerifyInvestorIdentification_StockSubscriptionQueryService_ValidFinancierId()
        {
            Guid id = new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601");

            GetInvestorIdentificationParameter queryParameters =
                new()
                {
                    FinancierId = id
                };

            OperationResult<Guid> result =
                await _queryService.VerifyInvestorIdentification(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(id, result.Result);
        }

        [Fact]
        public async Task VerifyInvestorIdentification_StockSubscriptionQueryService_InvalidFinancierId()
        {
            Guid id = new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e");

            GetInvestorIdentificationParameter queryParameters =
                new()
                {
                    FinancierId = id
                };

            OperationResult<Guid> result =
                await _queryService.VerifyInvestorIdentification(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(Guid.Empty, result.Result);
        }

        [Fact]
        public async Task VerifyCashDisbursementDividendPayment_StockSubscriptionQueryService_NotPaid()
        {
            Guid dividendId = new Guid("ff0dc77f-7f80-426a-bc24-09d3c10a957f");

            GetDividendDeclarationParameter queryParameters =
                new()
                {
                    DividendId = dividendId,
                };

            OperationResult<VerifyCashDisbursementForDividendPayment> result =
                await _queryService.VerifyCashDisbursementDividendPayment(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(new DateTime(), result.Result.DatePaid);
            Assert.Equal(0, result.Result.AmountPaid);
        }

        [Fact]
        public async Task VerifyCashDisbursementDividendPayment_StockSubscriptionQueryService_Paid()
        {
            Guid dividendId = new Guid("2558ab00-118c-4b67-a6d0-1b9888f841bc");

            GetDividendDeclarationParameter queryParameters =
                new()
                {
                    DividendId = dividendId,
                };

            OperationResult<VerifyCashDisbursementForDividendPayment> result =
                await _queryService.VerifyCashDisbursementDividendPayment(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(new DateTime(2022, 3, 4), result.Result.DatePaid);
            Assert.Equal(450.00M, result.Result.AmountPaid);
        }

        [Fact]
        public async Task VerifyDividendDeclarationIdentification_StockSubscriptionQueryService_ValidDividendId()
        {
            Guid id = new Guid("2558ab00-118c-4b67-a6d0-1b9888f841bc");

            GetDividendDeclarationParameter queryParameters =
                new()
                {
                    DividendId = id
                };

            OperationResult<Guid> result =
                await _queryService.VerifyDividendDeclarationIdentification(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(id, result.Result);
        }

        [Fact]
        public async Task VerifyDividendDeclarationIdentification_StockSubscriptionQueryService_InvalidDividendId()
        {
            Guid id = new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e");

            GetDividendDeclarationParameter queryParameters =
                new()
                {
                    DividendId = id
                };

            OperationResult<Guid> result =
                await _queryService.VerifyDividendDeclarationIdentification(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(Guid.Empty, result.Result);
        }

        [Fact]
        public async Task GetDividendDeclarationDetails_StockSubscriptionQueryService_ShouldSucceed()
        {
            GetDividendDeclarationParameter queryParameters = new() { DividendId = new Guid("2558ab00-118c-4b67-a6d0-1b9888f841bc") };
            OperationResult<DividendDeclarationDetails> result = await _queryService.GetDividendDeclarationDetails(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(new DateTime(2022, 2, 1), result.Result.StockIssueDate);
        }

        [Fact]
        public async Task GetDividendDeclarationListItems_StockSubscriptionQueryService_ShouldSucceed()
        {
            GetDividendDeclarationsParameters queryParameters =
                new() { StockId = new Guid("62d6e2e6-215d-4157-b7ec-1ba9b137c770"), Page = 1, PageSize = 10 };
            OperationResult<PagedList<DividendDeclarationListItem>> result = await _queryService.GetDividendDeclarationListItems(queryParameters);

            Assert.True(result.Success);

            int records = result.Result.Count;
            Assert.Equal(5, records);
        }
    }
}