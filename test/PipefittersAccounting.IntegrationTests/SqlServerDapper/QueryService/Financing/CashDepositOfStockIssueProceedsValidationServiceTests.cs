using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    public class CashDepositOfStockIssueProceedsValidationServiceTests : TestBaseDapper
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;
        private readonly ISharedQueryService _sharedQrySvc;
        private readonly ICashAccountAggregateValidationService _validationService;

        public CashDepositOfStockIssueProceedsValidationServiceTests()
        {
            _cashAcctQrySvc = new CashAccountQueryService(_dapperCtx);
            _sharedQrySvc = new SharedQueryService(_dapperCtx);
            _validationService = new CashAccountAggregateValidationService(_cashAcctQrySvc, _sharedQrySvc);
        }

        [Fact]
        public async Task Validate_VerifyAgentIsFinancierRule_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            VerifyAgentIsFinancierRule rule = new(_sharedQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyAgentIsFinancierRule_InvalidInvestorId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            model.AgentId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");
            VerifyAgentIsFinancierRule rule = new(_sharedQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyEventIsStockSubscriptionRule_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            VerifyEventIsStockSubscriptionRule rule = new(_sharedQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyEventIsStockSubscriptionRule_InvalidEvent_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            model.EventId = new Guid("12998229-7ede-4834-825a-0c55bde75695");
            VerifyEventIsStockSubscriptionRule rule = new(_sharedQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyInvestorHasStockSubscriptionRule_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            VerifyInvestorHasStockSubscriptionRule rule = new(_cashAcctQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyInvestorHasStockSubscriptionRule_WrongAgentId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            model.AgentId = new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886");

            VerifyInvestorHasStockSubscriptionRule rule = new(_cashAcctQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyDetailsOfDepositOfStockIssueProceedsRule_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            VerifyDetailsOfDepositOfStockIssueProceedsRule rule = new(_cashAcctQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyDetailsOfDepositOfStockIssueProceedsRule_InvalidTransactionDate_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            model.TransactionDate = new DateTime(2022, 5, 26);

            VerifyDetailsOfDepositOfStockIssueProceedsRule rule = new(_cashAcctQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyDetailsOfDepositOfStockIssueProceedsRule_InvalidTransactionAmount_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            model.TransactionAmount = 5700M;

            VerifyDetailsOfDepositOfStockIssueProceedsRule rule = new(_cashAcctQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyDetailsOfDepositOfStockIssueProceedsRule_ProceedsAlreadyRcvd_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfo_ProceedsAlreadyRcvd();

            VerifyDetailsOfDepositOfStockIssueProceedsRule rule = new(_cashAcctQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountAggregateValidationService_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();

            ValidationResult validationResult = await _validationService.IsValidCashDepositOfStockIssueProceeds(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountAggregateValidationService_InvalidInvestorId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            model.AgentId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            ValidationResult validationResult = await _validationService.IsValidCashDepositOfStockIssueProceeds(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountAggregateValidationService_InvalidEvent_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            model.EventId = new Guid("12998229-7ede-4834-825a-0c55bde75695");

            ValidationResult validationResult = await _validationService.IsValidCashDepositOfStockIssueProceeds(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountAggregateValidationService_WrongAgentId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            model.AgentId = new Guid("bf19cf34-f6ba-4fb2-b70e-ab19d3371886");

            ValidationResult validationResult = await _validationService.IsValidCashDepositOfStockIssueProceeds(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountAggregateValidationService_InvalidTransactionDate_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            model.TransactionDate = new DateTime(2022, 5, 26);

            ValidationResult validationResult = await _validationService.IsValidCashDepositOfStockIssueProceeds(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountAggregateValidationService_InvalidTransactionAmount_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionStockIssueProceedsInfo();
            model.TransactionAmount = 5700M;

            ValidationResult validationResult = await _validationService.IsValidCashDepositOfStockIssueProceeds(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountAggregateValidationService_ProceedsAlreadyRcvd_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfo_ProceedsAlreadyRcvd();

            ValidationResult validationResult = await _validationService.IsValidCashDepositOfStockIssueProceeds(model);

            Assert.False(validationResult.IsValid);
        }
    }
}