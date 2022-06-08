using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Infrastructure.Application.Services.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.Readmodels.Shared;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    [Trait("Integration", "DapperQueryService")]
    public class CashAccountQueryServiceTests : TestBaseDapper
    {
        private readonly ICashAccountQueryService _queryService;
        private readonly ISharedQueryService _sharedQryService;

        public CashAccountQueryServiceTests()
        {
            _queryService = new CashAccountQueryService(_dapperCtx);
            _sharedQryService = new SharedQueryService(_dapperCtx);
        }

        [Fact]
        public async Task GetCashAccountDetails_CashAccountQueryService_ShouldSucceed()
        {
            GetCashAccount queryParameters = new() { CashAccountId = new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e") };
            OperationResult<CashAccountDetail> result = await _queryService.GetCashAccountDetails(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(35625M, result.Result.Balance);
        }

        [Fact]
        public async Task GetCashAccountListItems_CashAccountQueryService_ShouldSucceed()
        {
            GetCashAccounts queryParameters = new() { Page = 1, PageSize = 5 };
            OperationResult<PagedList<CashAccountListItem>> result = await _queryService.GetCashAccountListItems(queryParameters);

            Assert.True(result.Success);

            int records = result.Result.Count;
            Assert.Equal(4, records);
        }

        [Fact]
        public async Task GetCashAccountWithAccountName_CashAccountQueryService_ExistingName_ShouldSucceed()
        {
            GetCashAccountWithAccountName queryParameters = new() { AccountName = "Payroll" };
            OperationResult<CashAccountReadModel> result = await _queryService.GetCashAccountWithAccountName(queryParameters);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetCashAccountWithAccountName_CashAccountQueryService_NonExistingName_ShouldFail()
        {
            GetCashAccountWithAccountName queryParameters = new() { AccountName = "Money Laundering" };
            OperationResult<CashAccountReadModel> result = await _queryService.GetCashAccountWithAccountName(queryParameters);

            Assert.False(result.Success);

            string msg = $"Unable to locate a cash account with account name '{queryParameters.AccountName}'!";
            Assert.Equal(msg, result.NonSuccessMessage);
        }

        [Fact]
        public async Task GetCashAccountWithAccountNumber_CashAccountQueryService_ExistingNumber_ShouldSucceed()
        {
            GetCashAccountWithAccountNumber queryParameters = new() { AccountNumber = "36547-9098812" };
            OperationResult<CashAccountReadModel> result = await _queryService.GetCashAccountWithAccountNumber(queryParameters);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetCashAccountWithAccountNumber_CashAccountQueryService_NonExistingNumber_ShouldFail()
        {
            GetCashAccountWithAccountNumber queryParameters = new() { AccountNumber = "12345-9098812" };
            OperationResult<CashAccountReadModel> result = await _queryService.GetCashAccountWithAccountNumber(queryParameters);

            Assert.False(result.Success);

            string msg = $"Unable to locate a cash account with account number '{queryParameters.AccountNumber}'!";
            Assert.Equal(msg, result.NonSuccessMessage);
        }

        [Fact]
        public async Task GetNumberOfCashAccountTransactions_CashAccountQueryService_AcctWithTransactions_ShouldSucceed()
        {
            GetCashAccount queryParameters = new() { CashAccountId = new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e") };
            OperationResult<int> result = await _queryService.GetNumberOfCashAccountTransactions(queryParameters);

            Assert.True(result.Success);

            Assert.Equal(17, result.Result);
        }

        [Fact]
        public async Task GetNumberOfCashAccountTransactions_CashAccountQueryService_AcctWithoutTransactions_ShouldSucceed()
        {
            GetCashAccount queryParameters = new() { CashAccountId = new Guid("765ec2b0-406a-4e42-b831-c9aa63800e76") };
            OperationResult<int> result = await _queryService.GetNumberOfCashAccountTransactions(queryParameters);

            Assert.True(result.Success);

            Assert.Equal(0, result.Result);
        }

        [Fact]
        public async Task GetExternalAgentIdentificationInfo_CashAccountQueryService_ShouldSucceed()
        {
            AgentIdentificationParameter queryParameters = new() { AgentId = new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601") };
            OperationResult<AgentIdentificationInfo> result = await _sharedQryService.GetExternalAgentIdentificationInfo(queryParameters);

            Assert.True(result.Success);

            Assert.Equal(6, result.Result.AgentTypeId);
            Assert.Equal("Financier", result.Result.AgentTypeName);
        }

        [Fact]
        public async Task GetEconomicEventIdentificationInfo_CashAccountQueryService_ShouldSucceed()
        {
            EventIdentificationParameter queryParameters = new() { EventId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1") };
            OperationResult<EventIdentificationInfo> result = await _sharedQryService.GetEconomicEventIdentificationInfo(queryParameters);

            Assert.True(result.Success);

            Assert.Equal(2, result.Result.EventTypeId);
            Assert.Equal("Cash Receipt from Loan Agreement", result.Result.EventTypeName);
        }

        [Fact]
        public async Task GetCreditorIssuedLoanAgreementValidationInfo_CashAccountQueryService_ShouldSucceed()
        {
            CreditorLoanAgreementValidationParameters queryParameters =
                new() { LoanId = new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1"), FinancierId = new Guid("12998229-7ede-4834-825a-0c55bde75695") };
            OperationResult<CreditorIssuedLoanAgreementValidationInfo> result = await _queryService.GetCreditorIssuedLoanAgreementValidationInfo(queryParameters);

            Assert.True(result.Success);

            Assert.Equal("Arturo Sandoval", result.Result.FinancierName);
            Assert.Equal(25000M, result.Result.LoanAmount);
        }

        [Fact]
        public async Task GetCashReceiptOfDebtIssueProceedsInfo_CashAccountQueryService_ShouldSucceed()
        {
            CreditorLoanAgreementValidationParameters queryParameters =
                new() { FinancierId = new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601"), LoanId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f") };
            OperationResult<CashReceiptOfDebtIssueProceedsInfo> result = await _queryService.GetCashReceiptOfDebtIssueProceedsInfo(queryParameters);

            Assert.True(result.Success);

            Assert.Equal("Paul Van Horn Enterprises", result.Result.FinancierName);
            Assert.Equal(new DateTime(2022, 2, 2), result.Result.LoanDate);
            Assert.Equal(new DateTime(2024, 2, 2), result.Result.MaturityDate);
            Assert.Equal(30000M, result.Result.LoanAmount);
            Assert.Equal(new DateTime(2022, 3, 19), result.Result.DateReceived);
            Assert.Equal(30000M, result.Result.AmountReceived);
        }

        [Fact]
        public async Task GetCashDisbursementForLoanInstallmentPaymentInfoQuery_CashAccountQueryService_ShouldSucceed()
        {
            GetLoanInstallmentInfoParameters queryParameters =
                new() { LoanInstallmentId = new Guid("8e804651-5021-4577-bbda-e7ee45a74e44") };
            OperationResult<CashDisbursementForLoanInstallmentPaymentInfo> result = await _queryService.GetCashDisbursementForLoanInstallmentPaymentInfo(queryParameters);

            Assert.True(result.Success);

            Assert.Equal(new DateTime(2022, 2, 2), result.Result.LoanDate);
            Assert.Equal(new DateTime(2024, 2, 2), result.Result.MaturityDate);
            Assert.Equal(1370.54M, result.Result.EqualMonthlyInstallment);
            Assert.Equal(new DateTime(2022, 3, 2), result.Result.DatePaid);
            Assert.Equal(1370.54M, result.Result.AmountPaid);
        }

        [Fact]
        public async Task GetFinancierToLoanInstallmentValidationInfoQuery_CashAccountQueryService_ShouldSucceed()
        {
            GetLoanInstallmentInfoParameters queryParameters =
                new() { LoanInstallmentId = new Guid("8e804651-5021-4577-bbda-e7ee45a74e44") };
            OperationResult<CreditorIsOwedThisLoanInstallmentValidationInfo> result = await _queryService.GetCreditorIsOwedThisLoanInstallmentValidationInfo(queryParameters);

            Assert.True(result.Success);

            Assert.Equal(new Guid("94b1d516-a1c3-4df8-ae85-be1f34966601"), result.Result.FinancierId);
            Assert.Equal(new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f"), result.Result.LoanId);
        }

        [Fact]
        public async Task GetCashAccountTransactionDetailsQuery_CashAccountQueryService_ShouldSucceed()
        {
            GetCashAccountTransactionDetailParameters queryParameters =
                new() { CashTransactionId = 1 };
            OperationResult<CashAccountTransactionDetail> result = await _queryService.GetCashAccountTransactionDetail(queryParameters);

            Assert.True(result.Success);

            Assert.Equal(new DateTime(2022, 1, 10), result.Result.CashAcctTransactionDate);
            Assert.Equal(10000M, result.Result.CashAcctTransactionAmount);
        }

        [Fact]
        public async Task GetCashAccountTransactionListItemQuery_CashAccountQueryService_ShouldSucceed()
        {
            GetCashAccountTransactionListItemsParameters queryParameters =
                new() { CashAccountId = new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e"), Page = 1, PageSize = 10 };
            OperationResult<PagedList<CashAccountTransactionListItem>> result = await _queryService.GetCashAccountTransactionListItem(queryParameters);

            Assert.True(result.Success);
            int count = result.Result.Count;
            Assert.True(count > 0);
        }

        [Fact]
        public async Task GetInvestorIdForStockSubscription_CashAccountQueryService_ShouldSucceed()
        {
            Guid stockId = new Guid("62d6e2e6-215d-4157-b7ec-1ba9b137c770");
            Guid investorId = new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886");

            GetInvestorIdForStockSubscriptionParameter queryParameters = new() { StockId = stockId };

            OperationResult<Guid> result = await _queryService.GetInvestorIdForStockSubscription(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(investorId, result.Result);
        }

        [Fact]
        public async Task GetInvestorIdForStockSubscription_CashAccountQueryService_ShouldFail()
        {
            Guid stockId = new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e");
            GetInvestorIdForStockSubscriptionParameter queryParameters = new() { StockId = stockId };

            OperationResult<Guid> result = await _queryService.GetInvestorIdForStockSubscription(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(Guid.Empty, result.Result);
        }

        [Fact]
        public async Task VerifyCashDepositOfStockIssueProceeds_CashAccountQueryService_ShouldSucceed()
        {
            Guid stockId = new Guid("62d6e2e6-215d-4157-b7ec-1ba9b137c770");

            GetStockSubscriptionParameter queryParameters = new() { StockId = stockId };

            OperationResult<VerificationOfCashDepositStockIssueProceeds> result = await _queryService.VerifyCashDepositOfStockIssueProceeds(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(10000M, result.Result.AmountReceived);
        }

        [Fact]
        public async Task GetInvestorIdForDividendDeclaration_CashAccountQueryService_ShouldSucceed()
        {
            Guid dividendId = new Guid("ff0dc77f-7f80-426a-bc24-09d3c10a957f");
            Guid investorId = new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886");

            GetDividendDeclarationParameter queryParameters = new() { DividendId = dividendId };

            OperationResult<Guid> result = await _queryService.GetInvestorIdForDividendDeclaration(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(investorId, result.Result);
        }

        [Fact]
        public async Task GetInvestorIdForDividendDeclaration_CashAccountQueryService_ShouldFail()
        {
            Guid dividendId = new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886");

            GetDividendDeclarationParameter queryParameters = new() { DividendId = dividendId };

            OperationResult<Guid> result = await _queryService.GetInvestorIdForDividendDeclaration(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(Guid.Empty, result.Result);
        }

        [Fact]
        public async Task GetDividendDeclarationDetails_CashAccountQueryService_ShouldSucceed()
        {
            Guid dividendId = new Guid("ff0dc77f-7f80-426a-bc24-09d3c10a957f");

            GetDividendDeclarationParameter queryParameters = new() { DividendId = dividendId };

            OperationResult<DividendDeclarationDetails> result = await _queryService.GetDividendDeclarationDetails(queryParameters);

            Assert.True(result.Success);
            Assert.Equal(.01M, result.Result.DividendPerShare);
        }

    }
}