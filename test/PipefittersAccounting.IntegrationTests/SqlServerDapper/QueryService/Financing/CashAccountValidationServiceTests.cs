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
    public class CashAccountValidationServiceTests : TestBaseDapper
    {
        private readonly ICashAccountQueryService _queryService;
        private readonly ICashAccountAggregateValidationService _validationService;

        public CashAccountValidationServiceTests()
        {
            _queryService = new CashAccountQueryService(_dapperCtx);
            _validationService = new CashAccountAggregateValidationService(_queryService);
        }

        /*********************************************************************/
        /*                   Cash account validators                      */
        /*********************************************************************/

        [Fact]
        public void Create_NewCashAccountNameMustBeUniqueValidator_ShouldSucceed()
        {
            var exception = Record.Exception(() => new NewCashAccountNameMustBeUniqueValidator(_queryService));
            Assert.Null(exception);
        }

        [Fact]
        public void Create_NewCashAccountNumberMustBeUniqueValidator_ShouldSucceed()
        {
            var exception = Record.Exception(() => new NewCashAccountNumberMustBeUniqueValidator(_queryService));
            Assert.Null(exception);
        }

        [Fact]
        public void Create_CashDeposit_ValidInfo_ShouldSucceed()
        {
            var exception = Record.Exception(() => GetCashDepositForLoanProceeds());
            Assert.Null(exception);
        }

        [Fact]
        public async Task Validate_NewCashAccountNameMustBeUniqueValidator_ValidAcctName_ShouldSucceed()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            NewCashAccountNameMustBeUniqueValidator acctNameValidator = new(_queryService);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_NewCashAccountNameMustBeUniqueValidator_ExistingAcctName_ShouldFail()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            model.CashAccountName = "Payroll";
            NewCashAccountNameMustBeUniqueValidator acctNameValidator = new(_queryService);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_NewCashAccountNumberMustBeUniqueValidator_ValidAcctNumber_ShouldSucceed()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            NewCashAccountNumberMustBeUniqueValidator acctNumberValidator = new(_queryService);

            ValidationResult validationResult = await acctNumberValidator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_NewCashAccountNumberMustBeUniqueValidator_ExistingAcctNumber_ShouldFail()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            model.CashAccountNumber = "36547-9098812";
            NewCashAccountNumberMustBeUniqueValidator acctNumberValidator = new(_queryService);

            ValidationResult validationResult = await acctNumberValidator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account number '{model.CashAccountNumber}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_ChainedCashAccountNameAndNumberValidators_ValidAcctNameAndNumber_ShouldSucceed()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            NewCashAccountNameMustBeUniqueValidator acctNameValidator = new(_queryService);
            NewCashAccountNumberMustBeUniqueValidator acctNumberValidator = new(_queryService);
            acctNameValidator.SetNext(acctNumberValidator);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_ChainedCashAccountNameAndNumberValidators_InvalidAcctNameAndValidNumber_ShouldFail()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            NewCashAccountNameMustBeUniqueValidator acctNameValidator = new(_queryService);
            model.CashAccountName = "Payroll";
            NewCashAccountNumberMustBeUniqueValidator acctNumberValidator = new(_queryService);
            acctNameValidator.SetNext(acctNumberValidator);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_ChainedCashAccountNameAndNumberValidators_InvalidAcctNameAndInvalidNumber_ShouldFail()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            NewCashAccountNameMustBeUniqueValidator acctNameValidator = new(_queryService);
            model.CashAccountName = "Payroll";
            model.CashAccountNumber = "36547-9098812";
            NewCashAccountNumberMustBeUniqueValidator acctNumberValidator = new(_queryService);
            acctNameValidator.SetNext(acctNumberValidator);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_ChainedCashAccountNameAndNumberValidators_ValidAcctNameAndInvalidNumber_ShouldFail()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            NewCashAccountNameMustBeUniqueValidator acctNameValidator = new(_queryService);
            model.CashAccountNumber = "36547-9098812";
            NewCashAccountNumberMustBeUniqueValidator acctNumberValidator = new(_queryService);
            acctNameValidator.SetNext(acctNumberValidator);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account number '{model.CashAccountNumber}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_EditedCashAccountNameMustBeUniqueValidator_ValidAcctName_ShouldSucceed()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfo();
            EditedCashAccountNameMustBeUniqueValidator acctNameValidator = new(_queryService);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditedCashAccountNameMustBeUniqueValidator_ExistingAcctName_ShouldFail()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfo();
            model.CashAccountName = "Payroll";
            EditedCashAccountNameMustBeUniqueValidator acctNameValidator = new(_queryService);

            ValidationResult validationResult = await acctNameValidator.Validate(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        /*********************************************************************/
        /*                   Validation, not validators                      */
        /*********************************************************************/

        [Fact]
        public async Task Validate_CreateCashAccountInfoValidation_ShouldSucceed()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            ValidationResult validationResult = await CreateCashAccountInfoValidation.Validate(model, _queryService);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreateCashAccountInfoValidation_ShouldFail()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            model.CashAccountName = "Payroll";
            ValidationResult validationResult = await CreateCashAccountInfoValidation.Validate(model, _queryService);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_EditCashAccountInfoValidation_ShouldSucceed()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfo();
            ValidationResult validationResult = await EditCashAccountInfoValidation.Validate(model, _queryService);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_EditCashAccountInfoValidation_ShouldFail()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfo();
            model.CashAccountName = "Financing Proceeds";
            ValidationResult validationResult = await EditCashAccountInfoValidation.Validate(model, _queryService);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        /*********************************************************************/
        /*                   Validation service                      */
        /*********************************************************************/

        [Fact]
        public async Task IsValidCreateCashAccountInfo_CashAccountAggregateValidationService_ShouldSucceed()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            ValidationResult validationResult = await _validationService.IsValidCreateCashAccountInfo(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidCreateCashAccountInfo_CashAccountAggregateValidationService_ShouldFail()
        {
            CreateCashAccountInfo model = CashAccountTestData.GetCreateCashAccountInfo();
            model.CashAccountName = "Payroll";
            ValidationResult validationResult = await _validationService.IsValidCreateCashAccountInfo(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task IsValidEditCashAccountInfo_CashAccountAggregateValidationService_ShouldSucceed()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfo();
            ValidationResult validationResult = await _validationService.IsValidEditCashAccountInfo(model);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidEditCashAccountInfo_CashAccountAggregateValidationService_CannotChangeAcctType_ShouldFail()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfoWithAcctTypeUpdate();
            ValidationResult validationResult = await _validationService.IsValidEditCashAccountInfo(model);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task IsValidEditCashAccountInfo_CashAccountAggregateValidationService_ShouldFail()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfo();
            model.CashAccountName = "Primary Checking";
            ValidationResult validationResult = await _validationService.IsValidEditCashAccountInfo(model);

            Assert.False(validationResult.IsValid);

            string msg = $"There is an existing cash account with account name '{model.CashAccountName}'";
            Assert.Equal(msg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task IsValidEditCashAccountInfo_CashAccountAggregateValidationService_ChangeAcctTypeNoTrans_ShouldSucceed()
        {
            EditCashAccountInfo model = CashAccountTestData.GetEditCashAccountInfo();
            model.CashAccountType = 3;
            ValidationResult validationResult = await _validationService.IsValidEditCashAccountInfo(model);

            Assert.True(validationResult.IsValid);
        }

        /*********************************************************************/
        /*                   Refactor all of this!!                      */
        /*********************************************************************/

        private CashDeposit GetCashDepositForLoanProceeds()
            => new
            (
                CashTransactionTypeEnum.CashReceiptDebtIssueProceeds,
                new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier),
                new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.LoanAgreement),
                4000M,
                new DateTime(2022, 4, 15),
                "2001",
                "ABCDE",
                new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            );

        private CashDeposit GetCashDepositForLoanProceedsInvalidFinancierId()
            => new
            (
                CashTransactionTypeEnum.CashReceiptDebtIssueProceeds,
                new ExternalAgent(EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")), AgentTypeEnum.Financier), // ** Invalid **
                new EconomicEvent(EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")), EventTypeEnum.LoanAgreement),
                25000M,
                new DateTime(2022, 1, 5),
                "2001",
                "ABCDE",
                new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            );

        private CashDeposit GetCashDepositForLoanProceedsInvalidLoanId()
            => new
            (
                CashTransactionTypeEnum.CashReceiptDebtIssueProceeds,
                new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier),
                new EconomicEvent(EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")), EventTypeEnum.LoanAgreement), // ** Invalid **
                25000M,
                new DateTime(2022, 1, 5),
                "2001",
                "ABCDE",
                new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            );

        private CashDeposit GetCashDepositForLoanProceedsInvalidDepositAmount()
            => new
            (
                CashTransactionTypeEnum.CashReceiptDebtIssueProceeds,
                new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier),
                new EconomicEvent(EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")), EventTypeEnum.LoanAgreement),
                25001M,     // ** Invalid **
                new DateTime(2022, 1, 5),
                "2001",
                "ABCDE",
                new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            );

        private CashDeposit GetCashDepositForLoanProceedsDuplicateDeposit()
            => new
            (
                CashTransactionTypeEnum.CashReceiptDebtIssueProceeds,
                new ExternalAgent(EntityGuidID.Create(new Guid("12998229-7ede-4834-825a-0c55bde75695")), AgentTypeEnum.Financier),
                new EconomicEvent(EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")), EventTypeEnum.LoanAgreement),
                25000M,
                new DateTime(2022, 1, 5),
                "2001",
                "ABCDE",
                new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            );

        private CashDisbursement GetCashDisbursementForLoanPayment()
            => new
            (
                CashTransactionTypeEnum.CashDisbursementLoanPayment,
                new ExternalAgent(EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")), AgentTypeEnum.Financier),
                new EconomicEvent(EntityGuidID.Create(new Guid("0bd39edb-8da3-40f9-854f-b90e798b82c2")), EventTypeEnum.LoanPayment),
                1100M,
                new DateTime(2022, 7, 15),
                "2011",
                "ABCDE",
                new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            );

    }
}