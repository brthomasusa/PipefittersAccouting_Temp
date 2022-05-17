using System;
using System.Threading.Tasks;
using Xunit;
using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    [Trait("Integration", "DapperQueryService")]
    public class CashDisbursementForLoanPaymentValidationServiceTests : TestBaseDapper
    {
        private readonly ICashAccountQueryService _queryService;
        private readonly ICashAccountAggregateValidationService _validationService;

        public CashDisbursementForLoanPaymentValidationServiceTests()
        {
            _queryService = new CashAccountQueryService(_dapperCtx);
            _validationService = new CashAccountAggregateValidationService(_queryService);
        }

        /*********************************************************************/
        /*                   Cash disbursement for loan installment payment validators                      */
        /*********************************************************************/

        [Fact]
        public async Task Validate_LoanInstallmentPaymentAsEconomicEventValidator_ShouldSucceed()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            LoanInstallmentPaymentAsEconomicEventValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_LoanInstallmentPaymentAsEconomicEventValidator_UsingLoanId_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.EventId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            LoanInstallmentPaymentAsEconomicEventValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = "An event of type 'Cash Receipt from Loan Agreement' is not valid for this operation. Expecting 'Cash Disbursement for Loan Payment'!";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_FinancierHasLoanInstallmentValidator_ShouldSucceed()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            FinancierHasLoanInstallmentValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_FinancierHasLoanInstallmentValidator_InvalidAgentId_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.AgentId = new Guid("12998229-7ede-4834-825a-0c55bde75695");

            FinancierHasLoanInstallmentValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"This financier '{model.AgentId}' is not the correct payee for this loan installment!";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_DisburesementForLoanPymtValidator_ShouldSucceed()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();

            DisburesementForLoanPymtValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisburesementForLoanPymtValidator_InvalidTransactionDate_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.TransactionDate = new DateTime(2022, 3, 15);

            DisburesementForLoanPymtValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisburesementForLoanPymtValidator_InvalidTransactionAmount_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.TransactionAmount = 1111M;

            DisburesementForLoanPymtValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisburesementForLoanPymtValidator_DuplicatePymt_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymtDuplicate();

            DisburesementForLoanPymtValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisbursementForLoanPaymentValidation_ShouldSucceed()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            DisbursementForLoanPaymentValidation validation = new(model, _queryService);

            ValidationResult validationResult = await validation.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisbursementForLoanPaymentValidation_DuplicatePymt_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymtDuplicate();
            DisbursementForLoanPaymentValidation validation = new(model, _queryService);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisbursementForLoanPaymentValidation_InvalidTransactionAmount_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.TransactionAmount = 1111M;

            DisbursementForLoanPaymentValidation validation = new(model, _queryService);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisbursementForLoanPaymentValidation_TransactionDateOutOfRange_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.TransactionDate = new DateTime(2022, 3, 15);

            DisbursementForLoanPaymentValidation validation = new(model, _queryService);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisbursementForLoanPaymentValidation_InvalidLoanInstallmentId_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.EventId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            DisbursementForLoanPaymentValidation validation = new(model, _queryService);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        /*********************************************************************/
        /*                   Validation service                      */
        /*********************************************************************/

        [Fact]
        public async Task IsValidCashDisbursementForLoanPayment_CashAccountAggregateValidationService_ShouldSucceed()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForLoanPayment(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidCashDisbursementForLoanPayment_CashAccountAggregateValidationService_InvalidTransactionDate_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.TransactionDate = new DateTime(2022, 3, 15);

            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForLoanPayment(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidCashDisbursementForLoanPayment_CashAccountAggregateValidationService_InvalidEventType_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.EventId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");

            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForLoanPayment(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidCashDisbursementForLoanPayment_CashAccountAggregateValidationService_InvalidPaymentAmount_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymt();
            model.TransactionAmount = 1200M;

            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForLoanPayment(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidCashDisbursementForLoanPayment_CashAccountAggregateValidationService_DuplicatePymtAttempt_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoLoanPymtDuplicate();

            ValidationResult validationResult = await _validationService.IsValidCashDisbursementForLoanPayment(model);

            Assert.False(validationResult.IsValid);
        }
    }
}