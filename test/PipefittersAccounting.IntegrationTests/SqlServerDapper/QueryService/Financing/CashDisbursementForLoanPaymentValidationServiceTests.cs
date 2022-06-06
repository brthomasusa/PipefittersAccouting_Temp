using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Infrastructure.Application.Services.Shared;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    [Trait("Integration", "DapperQueryService")]
    public class CashDisbursementForLoanPaymentValidationServiceTests : TestBaseDapper
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;
        private readonly ISharedQueryService _sharedQrySvc;
        private readonly ICashAccountAggregateValidationService _validationService;

        public CashDisbursementForLoanPaymentValidationServiceTests()
        {
            _cashAcctQrySvc = new CashAccountQueryService(_dapperCtx);
            _sharedQrySvc = new SharedQueryService(_dapperCtx);
            _validationService = new CashAccountAggregateValidationService(_cashAcctQrySvc, _sharedQrySvc);
        }

        /*********************************************************************/
        /*                   Cash disbursement for loan installment payment validators                      */
        /*********************************************************************/

        [Fact]
        public async Task Validate_LoanInstallmentPaymentAsEconomicEventValidator_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            VerifyEventIsLoanInstallmentRule validator = new(_sharedQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_LoanInstallmentPaymentAsEconomicEventValidator_UsingLoanId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.EventId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            VerifyEventIsLoanInstallmentRule validator = new(_sharedQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = "An event of type 'Cash Receipt from Loan Agreement' is not valid for this operation. Expecting 'Cash Disbursement for Loan Payment'!";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_FinancierHasLoanInstallmentValidator_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            VerifyCreditorIsOwedLoanInstallmentRule validator = new(_cashAcctQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_FinancierHasLoanInstallmentValidator_InvalidAgentId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.AgentId = new Guid("12998229-7ede-4834-825a-0c55bde75695");

            VerifyCreditorIsOwedLoanInstallmentRule validator = new(_cashAcctQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"This financier '{model.AgentId}' is not the correct payee for this loan installment!";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_VerifyDebtIssueProceedsHaveBeenReceivedValidator_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();

            VerifyDebtIssueProceedsHaveBeenReceivedRule validator = new(_cashAcctQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyDebtIssueProceedsHaveBeenReceivedValidator_ProceedsNotReceived_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymtNoProeedsDeposited();

            VerifyDebtIssueProceedsHaveBeenReceivedRule validator = new(_cashAcctQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisburesementForLoanPymtValidator_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();

            VerifyLoanPymtDateAmountAndStatusRule validator = new(_cashAcctQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisburesementForLoanPymtValidator_InvalidTransactionDate_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.TransactionDate = new DateTime(2022, 2, 1);

            VerifyLoanPymtDateAmountAndStatusRule validator = new(_cashAcctQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisburesementForLoanPymtValidator_InvalidTransactionAmount_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.TransactionAmount = 1111M;

            VerifyLoanPymtDateAmountAndStatusRule validator = new(_cashAcctQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisburesementForLoanPymtValidator_DuplicatePymt_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymtDuplicate();

            VerifyLoanPymtDateAmountAndStatusRule validator = new(_cashAcctQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisbursementForLoanPaymentValidation_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            DisbursementForLoanPaymentValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);

            ValidationResult validationResult = await validation.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisbursementForLoanPaymentValidation_DuplicatePymt_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymtDuplicate();
            DisbursementForLoanPaymentValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisbursementForLoanPaymentValidation_InvalidTransactionAmount_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.TransactionAmount = 1111M;

            DisbursementForLoanPaymentValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisbursementForLoanPaymentValidation_TransactionDateOutOfRange_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.TransactionDate = new DateTime(2022, 2, 1);

            DisbursementForLoanPaymentValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisbursementForLoanPaymentValidation_InvalidLoanInstallmentId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.EventId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            DisbursementForLoanPaymentValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        /*********************************************************************/
        /*                   Validation service                      */
        /*********************************************************************/

        [Fact]
        public async Task IsValidCashDisbursementForLoanPayment_CashAccountAggregateValidationService_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForLoanPayment(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidCashDisbursementForLoanPayment_CashAccountAggregateValidationService_InvalidTransactionDate_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.TransactionDate = new DateTime(2022, 2, 1);

            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForLoanPayment(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidCashDisbursementForLoanPayment_CashAccountAggregateValidationService_InvalidEventType_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.EventId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForLoanPayment(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidCashDisbursementForLoanPayment_CashAccountAggregateValidationService_InvalidPaymentAmount_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.TransactionAmount = 1200M;

            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForLoanPayment(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidCashDisbursementForLoanPayment_CashAccountAggregateValidationService_DuplicatePymtAttempt_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymtDuplicate();

            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForLoanPayment(model);

            Assert.False(validationResult.IsValid);
        }
    }
}