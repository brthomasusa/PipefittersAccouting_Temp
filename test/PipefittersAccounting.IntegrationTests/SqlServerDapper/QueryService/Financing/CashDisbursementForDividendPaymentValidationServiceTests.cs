using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    public class CashDisbursementForDividendPaymentValidationServiceTests : TestBaseDapper
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;
        private readonly ISharedQueryService _sharedQrySvc;
        private readonly ICashAccountAggregateValidationService _validationService;

        public CashDisbursementForDividendPaymentValidationServiceTests()
        {
            _cashAcctQrySvc = new CashAccountQueryService(_dapperCtx);
            _sharedQrySvc = new SharedQueryService(_dapperCtx);
            _validationService = new CashAccountAggregateValidationService(_cashAcctQrySvc, _sharedQrySvc);
        }

        [Fact]
        public async Task Validate_VerifyAgentIsFinancierRule_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            VerifyAgentIsFinancierRule agentRule = new(_sharedQrySvc);

            ValidationResult validationResult = await agentRule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyAgentIsFinancierRule_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            model.AgentId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            VerifyAgentIsFinancierRule agentRule = new(_sharedQrySvc);

            ValidationResult validationResult = await agentRule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyEventIsDividendDeclarationRule_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            VerifyEventIsDividendDeclarationRule rule = new(_sharedQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyEventIsDividendDeclarationRule_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            model.EventId = new Guid("12998229-7ede-4834-825a-0c55bde75695");

            VerifyEventIsDividendDeclarationRule rule = new(_sharedQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyInvestorHasDividendDeclarationRule_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            VerifyInvestorHasDividendDeclarationRule rule = new(_cashAcctQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyInvestorHasDividendDeclarationRule_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            model.AgentId = new Guid("01da50f9-021b-4d03-853a-3fd2c95e207d");

            VerifyInvestorHasDividendDeclarationRule rule = new(_cashAcctQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyDetailsOfDisbursementForDividendPaymentRule_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            VerifyInvestorHasDividendDeclarationRule rule = new(_cashAcctQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyDetailsOfDisbursementForDividendPaymentRule_InvalidTransactionDate_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            model.TransactionDate = new DateTime(2022, 1, 2);
            VerifyDetailsOfDisbursementForDividendPaymentRule rule = new(_cashAcctQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyDetailsOfDisbursementForDividendPaymentRule_InvalidTransactionAmount_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            model.TransactionAmount = 99.99M;
            VerifyDetailsOfDisbursementForDividendPaymentRule rule = new(_cashAcctQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyDetailsOfDisbursementForDividendPaymentRule_AlreadyPaid_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymtPaid();

            VerifyDetailsOfDisbursementForDividendPaymentRule rule = new(_cashAcctQrySvc);

            ValidationResult validationResult = await rule.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountAggregateValidationService_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();

            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForDividendPayment(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountAggregateValidationService_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            model.AgentId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForDividendPayment(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountAggregateValidationService_InvalidDividend_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            model.EventId = new Guid("12998229-7ede-4834-825a-0c55bde75695");

            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForDividendPayment(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountAggregateValidationService_InvalidInvestor_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            model.AgentId = new Guid("01da50f9-021b-4d03-853a-3fd2c95e207d");

            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForDividendPayment(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountAggregateValidationService_InvalidTransactionDate_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            model.TransactionDate = new DateTime(2022, 1, 2);

            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForDividendPayment(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountAggregateValidationService_InvalidTransactionAmount_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymt();
            model.TransactionAmount = 99.99M;

            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForDividendPayment(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashAccountAggregateValidationService_AlreadyPaid_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDividendPymtPaid();

            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForDividendPayment(model);

            Assert.False(validationResult.IsValid);
        }
    }
}