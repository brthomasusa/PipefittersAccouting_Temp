using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    [Trait("Integration", "DapperQueryService")]
    public class CashDepositOfDebtIssueProceedsValidationServiceTests : TestBaseDapper
    {
        private readonly ICashAccountQueryService _queryService;
        private readonly ICashAccountAggregateValidationService _validationService;

        public CashDepositOfDebtIssueProceedsValidationServiceTests()
        {
            _queryService = new CashAccountQueryService(_dapperCtx);
            _validationService = new CashAccountAggregateValidationService(_queryService);
        }

        /*********************************************************************/
        /*                   Cash deposit of debt issue proceeds validators                      */
        /*********************************************************************/

        [Fact]
        public async Task Validate_FinancierAsExternalAgentValidator_ShouldSucceed()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            FinancierAsExternalAgentValidator agentValidator = new(_queryService);

            ValidationResult validationResult = await agentValidator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_FinancierAsExternalAgentValidator_ExistingButInvalidAgentId_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            model.AgentId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");
            FinancierAsExternalAgentValidator agentValidator = new(_queryService);

            ValidationResult validationResult = await agentValidator.Validate(model);

            Assert.False(validationResult.IsValid);
            Assert.Equal("An agent of type 'Employee' is not valid for this operation. Expecting a financier!", validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_FinancierAsExternalAgentValidator_NonExistingAgentId_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            model.AgentId = new Guid("1234b318-649e-470d-9d2b-693bfb0b2744");
            FinancierAsExternalAgentValidator agentValidator = new(_queryService);

            ValidationResult validationResult = await agentValidator.Validate(model);

            Assert.False(validationResult.IsValid);
            Assert.Equal("Unable to locate an external agent with Id '1234b318-649e-470d-9d2b-693bfb0b2744'!", validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_LoanAgreementAsEconomicEventValidator_ShouldSucceed()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            LoanAgreementAsEconomicEventValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_LoanAgreementAsEconomicEventValidator_StockSubscriptionId_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            model.EventId = new Guid("5997f125-bfca-4540-a144-01e444f6dc25");

            LoanAgreementAsEconomicEventValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
            Assert.Equal("An event of type 'Cash Receipt from Stock Subscription' is not valid for this operation. Expecting a loan agreement!", validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_LoanAgreementAsEconomicEventValidator_NonExistingEventId_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            model.EventId = new Guid("2227f125-bfca-4540-a144-01e444f6dc25");

            LoanAgreementAsEconomicEventValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
            Assert.Equal("Unable to locate an economic event with Id '2227f125-bfca-4540-a144-01e444f6dc25'!", validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_IsCreditorAssociatedWithThisLoanAgreeValidator_ShouldSucceed()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            IsCreditorAssociatedWithThisLoanAgreeValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_IsCreditorAssociatedWithThisLoanAgreeValidator_WrongLoanId_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            model.EventId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");
            IsCreditorAssociatedWithThisLoanAgreeValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"Unable to locate a loan agreement with id '{model.EventId}' issued by financier with id '{model.AgentId}'!";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsValidator_ShouldSucceed()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsValidator_TransactionDateOutOfRange_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            model.TransactionDate = new DateTime(2022, 4, 10);
            VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsValidator_TransactionAmtNotEqualLoanAmt_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            model.TransactionAmount = 5000M;
            VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsValidator_DuplicateDeposit_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoDuplicateDeposit();
            VerifyMiscDetailsOfCashDepositOfDebtIssueProceedsValidator validator = new(_queryService);

            ValidationResult validationResult = await validator.Validate(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_ShouldSucceed()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            DepositOfDebtIssueProceedsValidation validation = new(model, _queryService);

            ValidationResult validationResult = await validation.Validate();

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_ExistingButInvalidAgentId_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            model.AgentId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744");
            DepositOfDebtIssueProceedsValidation validation = new(model, _queryService);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_NonExistingdAgentId_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            model.AgentId = new Guid("1234b318-649e-470d-9d2b-693bfb0b2744");
            DepositOfDebtIssueProceedsValidation validation = new(model, _queryService);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_StockSubscriptionId_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            model.EventId = new Guid("5997f125-bfca-4540-a144-01e444f6dc25");
            DepositOfDebtIssueProceedsValidation validation = new(model, _queryService);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_NonExistingEventId_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            model.EventId = new Guid("2227f125-bfca-4540-a144-01e444f6dc25");
            DepositOfDebtIssueProceedsValidation validation = new(model, _queryService);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_ExistingButWrongLoanId_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            model.EventId = new Guid("09b53ffb-9983-4cde-b1d6-8a49e785177f");
            DepositOfDebtIssueProceedsValidation validation = new(model, _queryService);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_TransactionDateOutOfRange_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            model.TransactionDate = new DateTime(2022, 4, 10);
            DepositOfDebtIssueProceedsValidation validation = new(model, _queryService);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_TransactionAmtNotEqualLoanAmt_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfo();
            model.TransactionAmount = 5000M;
            DepositOfDebtIssueProceedsValidation validation = new(model, _queryService);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DepositOfDebtIssueProceedsValidation_DuplicateDeposit_ShouldFail()
        {
            CreateCashAccountTransactionInfo model = CashAccountTestData.GetCreateCashAccountTransactionInfoDuplicateDeposit();
            DepositOfDebtIssueProceedsValidation validation = new(model, _queryService);

            ValidationResult validationResult = await validation.Validate();

            Assert.False(validationResult.IsValid);
        }
    }
}