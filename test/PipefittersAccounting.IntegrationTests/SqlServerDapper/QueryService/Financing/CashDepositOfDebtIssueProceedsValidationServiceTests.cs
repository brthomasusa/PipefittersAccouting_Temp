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
    public class CashDepositOfDebtIssueProceedsValidationServiceTests : TestBaseDapper
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;
        private readonly ISharedQueryService _sharedQrySvc;
        private readonly ICashAccountAggregateValidationService _validationService;

        public CashDepositOfDebtIssueProceedsValidationServiceTests()
        {
            _cashAcctQrySvc = new CashAccountQueryService(_dapperCtx);
            _sharedQrySvc = new SharedQueryService(_dapperCtx);
            _validationService = new CashAccountAggregateValidationService(_cashAcctQrySvc, _sharedQrySvc);
        }

        /*********************************************************************/
        /*                   Cash deposit of debt issue proceeds validators                      */
        /*********************************************************************/

        [Fact]
        public async Task Validate_FinancierAsExternalAgentValidator_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            VerifyAgentIsFinancierRule agentValidator = new(_sharedQrySvc);

            ValidationResult validationResult = await agentValidator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_FinancierAsExternalAgentValidator_ExistingButInvalidAgentId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            model.AgentId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");
            VerifyAgentIsFinancierRule agentValidator = new(_sharedQrySvc);

            ValidationResult validationResult = await agentValidator.Validate(model);

            Assert.False(validationResult.IsValid);
            Assert.Equal("An agent of type 'Employee' is not valid for this operation. Expecting a financier!", validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_FinancierAsExternalAgentValidator_NonExistingAgentId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            model.AgentId = new Guid("1234b318-649e-470d-9d2b-693bfb0b2744");
            VerifyAgentIsFinancierRule agentValidator = new(_sharedQrySvc);

            ValidationResult validationResult = await agentValidator.Validate(model);

            Assert.False(validationResult.IsValid);
            Assert.Equal("Unable to locate an external agent with Id '1234b318-649e-470d-9d2b-693bfb0b2744'!", validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_LoanAgreementAsEconomicEventValidator_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            VerifyEventIsLoanAgreementRule validator = new(_sharedQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_LoanAgreementAsEconomicEventValidator_StockSubscriptionId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            model.EventId = new Guid("5997f125-bfca-4540-a144-01e444f6dc25");

            VerifyEventIsLoanAgreementRule validator = new(_sharedQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
            Assert.Equal("An event of type 'Cash Receipt from Stock Subscription' is not valid for this operation. Expecting a loan agreement!", validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_LoanAgreementAsEconomicEventValidator_NonExistingEventId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            model.EventId = new Guid("2227f125-bfca-4540-a144-01e444f6dc25");

            VerifyEventIsLoanAgreementRule validator = new(_sharedQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
            Assert.Equal("Unable to locate an economic event with Id '2227f125-bfca-4540-a144-01e444f6dc25'!", validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_IsCreditorAssociatedWithThisLoanAgreeValidator_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            VerifyCreditorHasLoanAgreementRule validator = new(_cashAcctQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_IsCreditorAssociatedWithThisLoanAgreeValidator_WrongLoanId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            model.EventId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");
            VerifyCreditorHasLoanAgreementRule validator = new(_cashAcctQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"Unable to locate a loan agreement with id '{model.EventId}' issued by financier with id '{model.AgentId}'!";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsValidator_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsRule validator = new(_cashAcctQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsValidator_TransactionDateOutOfRange_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            model.TransactionDate = new DateTime(2022, 4, 10);
            VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsRule validator = new(_cashAcctQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsValidator_TransactionAmtNotEqualLoanAmt_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            model.TransactionAmount = 5000M;
            VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsRule validator = new(_cashAcctQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsValidator_DuplicateDeposit_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDuplicateLoanProceedsDeposit();
            VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsRule validator = new(_cashAcctQrySvc);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_ShouldSucceed()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            DepositOfDebtIssueProceedsValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);

            ValidationResult validationResult = await validation.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_ExistingButInvalidAgentId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            model.AgentId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");
            DepositOfDebtIssueProceedsValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_NonExistingdAgentId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            model.AgentId = new Guid("1234b318-649e-470d-9d2b-693bfb0b2744");
            DepositOfDebtIssueProceedsValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_StockSubscriptionId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            model.EventId = new Guid("5997f125-bfca-4540-a144-01e444f6dc25");
            DepositOfDebtIssueProceedsValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_NonExistingEventId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            model.EventId = new Guid("2227f125-bfca-4540-a144-01e444f6dc25");
            DepositOfDebtIssueProceedsValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_ExistingButWrongLoanId_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            model.EventId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");
            DepositOfDebtIssueProceedsValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_TransactionDateOutOfRange_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            model.TransactionDate = new DateTime(2022, 4, 10);
            DepositOfDebtIssueProceedsValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_TransactionAmtNotEqualLoanAmt_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionLoanProceedsInfo();
            model.TransactionAmount = 5000M;
            DepositOfDebtIssueProceedsValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_DuplicateDeposit_ShouldFail()
        {
            CashTransactionWriteModel model = CashAccountTestData.GetCreateCashAccountTransactionInfoDuplicateLoanProceedsDeposit();
            DepositOfDebtIssueProceedsValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }
    }
}