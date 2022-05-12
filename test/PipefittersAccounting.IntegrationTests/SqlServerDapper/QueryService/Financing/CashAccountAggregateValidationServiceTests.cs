using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.IntegrationTests.Base;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    [Trait("Integration", "DapperQueryService")]
    public class CashAccountAggregateValidationServiceTests : TestBaseDapper
    {
        private readonly ICashAccountQueryService _queryService;
        private readonly ICashAccountAggregateValidationService _validationService;

        public CashAccountAggregateValidationServiceTests()
        {
            _queryService = new CashAccountQueryService(_dapperCtx);
            _validationService = new CashAccountAggregateValidationService(_queryService);
        }

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


        // [Fact]
        // public async Task Validate_FinancierValidator_ValidFinancierId_ShouldSucceed()
        // {
        //     CashDeposit deposit = GetCashDepositForLoanProceeds();
        //     FinancierAsPayorIdentificationValidator financierValidator = new(_queryService);
        //     ValidationResult validationResult = await financierValidator.Validate(deposit);

        //     Assert.True(validationResult.IsValid);
        // }

        // [Fact]
        // public async Task Validate_FinancierValidator_InvalidFinancierId_ShouldFail()
        // {
        //     CashTransaction cashTransaction = GetCashTransactionLoanProceeds();
        //     cashTransaction.UpdateAgentId(EntityGuidID.Create(new Guid("123471a0-5c1e-4a4d-97e7-288fb0f6338a")));
        //     FinancierValidator financierValidator = new(_queryService);

        //     ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

        //     string errMsg = $"Unable to locate a financier with FinancierId: {cashTransaction.AgentId}.";
        //     Assert.False(validationResult.IsValid);
        //     Assert.Equal(errMsg, validationResult.Messages[0]);
        // }

        // [Fact]
        // public async Task Validate_CreditorHasLoanAgreeValidator_ShouldSucceed()
        // {
        //     CashTransaction cashTransaction = GetCashTransactionLoanProceeds();

        //     FinancierValidator financierValidator = new(_queryService);
        //     CreditorHasLoanAgreeValidator creditorHasLoanAgreeValidator = new(_queryService);

        //     financierValidator.Next = creditorHasLoanAgreeValidator;

        //     ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

        //     Assert.True(validationResult.IsValid);
        // }

        // [Fact]
        // public async Task Validate_CreditorHasLoanAgreeValidator_InvalidLoanId_ShouldFail()
        // {
        //     CashTransaction cashTransaction = GetCashTransactionLoanProceeds();
        //     // An existing loan agreement but not for this financier
        //     cashTransaction.UpdateEventId(EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")));

        //     FinancierValidator financierValidator = new(_queryService);
        //     CreditorHasLoanAgreeValidator creditorHasLoanAgreeValidator = new(_queryService);

        //     financierValidator.Next = creditorHasLoanAgreeValidator;

        //     ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

        //     string errMsg = $"Unable to locate a loan agreement with loan Id: {cashTransaction.EventId} for financier: {cashTransaction.AgentId}.";
        //     Assert.False(validationResult.IsValid);
        //     Assert.Equal(errMsg, validationResult.Messages[0]);
        // }

        // [Fact]
        // public async Task Validate_ReceiptLoanProceedsValidator_ShouldSucceed()
        // {
        //     CashTransaction cashTransaction = GetCashTransactionLoanProceeds();

        //     FinancierValidator financierValidator = new(_queryService);
        //     CreditorHasLoanAgreeValidator creditorHasLoanAgreeValidator = new(_queryService);
        //     ReceiptLoanProceedsValidator receiptLoanProceedsValidator = new(_queryService);

        //     financierValidator.Next = creditorHasLoanAgreeValidator;
        //     creditorHasLoanAgreeValidator.Next = receiptLoanProceedsValidator;

        //     ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

        //     Assert.True(validationResult.IsValid);
        // }

        // [Fact]
        // public async Task Validate_ReceiptLoanProceedsValidator_AmountsNotEqual_ShouldFail()
        // {
        //     // Loan amount and deposit amount don't match
        //     CashTransaction cashTransaction = GetCashTransactionLoanProceeds();
        //     cashTransaction.UpdateCashTransactionAmount(CashTransactionAmount.Create(3000M));

        //     FinancierValidator financierValidator = new(_queryService);
        //     CreditorHasLoanAgreeValidator creditorHasLoanAgreeValidator = new(_queryService);
        //     ReceiptLoanProceedsValidator receiptLoanProceedsValidator = new(_queryService);

        //     financierValidator.Next = creditorHasLoanAgreeValidator;
        //     creditorHasLoanAgreeValidator.Next = receiptLoanProceedsValidator;

        //     ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

        //     Assert.False(validationResult.IsValid);
        //     string errMsg = "The loan proceed amount and the loan agreement amount do not match.";
        //     Assert.Equal(errMsg, validationResult.Messages[0]);
        // }

        // [Fact]
        // public async Task Validate_ReceiptLoanProceedsValidator_DepositedAlready_ShouldFail()
        // {
        //     // Loan amount and deposit amount don't match
        //     CashTransaction cashTransaction = GetCashTransactionLoanProceedsDuplicateDeposit();

        //     FinancierValidator financierValidator = new(_queryService);
        //     CreditorHasLoanAgreeValidator creditorHasLoanAgreeValidator = new(_queryService);
        //     ReceiptLoanProceedsValidator receiptLoanProceedsValidator = new(_queryService);

        //     financierValidator.Next = creditorHasLoanAgreeValidator;
        //     creditorHasLoanAgreeValidator.Next = receiptLoanProceedsValidator;

        //     ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

        //     Assert.False(validationResult.IsValid);
        // }

        // [Fact]
        // public async Task Validate_DisburesementForLoanPymtValidator_ShouldSucceed()
        // {
        //     // Loan amount and deposit amount don't match
        //     CashTransaction cashTransaction = GetCashTransactionLoanInstallmentPymt();

        //     FinancierValidator financierValidator = new(_queryService);
        //     DisburesementForLoanPymtValidator paymentValidator = new(_queryService);

        //     financierValidator.Next = paymentValidator;

        //     ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

        //     Assert.True(validationResult.IsValid);
        // }

        // [Fact]
        // public async Task Validate_DisburesementForLoanPymtValidator_ExistingButInvalidFinancierID_ShouldFail()
        // {
        //     CashTransaction cashTransaction = GetCashTransactionLoanInstallmentPymt();
        //     cashTransaction.UpdateAgentId(EntityGuidID.Create(new Guid("12998229-7ede-4834-825a-0c55bde75695")));

        //     FinancierValidator financierValidator = new(_queryService);
        //     DisburesementForLoanPymtValidator paymentValidator = new(_queryService);

        //     financierValidator.Next = paymentValidator;

        //     ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

        //     Assert.False(validationResult.IsValid);
        // }

        // [Fact]
        // public async Task Validate_DisburesementForLoanPymtValidator_InvalidPymtAmt_ShouldFail()
        // {
        //     // Installment amount and payment amount don't match
        //     CashTransaction cashTransaction = GetCashTransactionLoanInstallmentPymt();
        //     cashTransaction.UpdateCashTransactionAmount(CashTransactionAmount.Create(1000M));

        //     FinancierValidator financierValidator = new(_queryService);
        //     DisburesementForLoanPymtValidator paymentValidator = new(_queryService);

        //     financierValidator.Next = paymentValidator;

        //     ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

        //     Assert.False(validationResult.IsValid);
        // }

        // [Fact]
        // public async Task Validate_DisburesementForLoanPymtValidator_PaidAlready_ShouldFail()
        // {
        //     // Installment amount and payment amount don't match
        //     CashTransaction cashTransaction = GetCashTransactionLoanInstallmentPymtAlreadyPaid();

        //     FinancierValidator financierValidator = new(_queryService);
        //     DisburesementForLoanPymtValidator paymentValidator = new(_queryService);

        //     financierValidator.Next = paymentValidator;

        //     ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

        //     Assert.False(validationResult.IsValid);
        // }

        // // Test CashReceiptForDebtIssueValidator

        // [Fact]
        // public async Task Validate_CashReceiptForDebtIssueValidator_ShouldSucceed()
        // {
        //     CashTransaction cashTransaction = GetCashTransactionLoanProceeds();

        //     ValidationResult validationResult = await CashReceiptForDebtIssueValidator.Validate(cashTransaction, _queryService);

        //     Assert.True(validationResult.IsValid);
        // }

        // [Fact]
        // public async Task Validate_CashReceiptForDebtIssueValidator_InvalidFinancierId_ShouldFail()
        // {
        //     CashTransaction cashTransaction = GetCashTransactionLoanProceeds();
        //     cashTransaction.UpdateAgentId(EntityGuidID.Create(new Guid("123471a0-5c1e-4a4d-97e7-288fb0f6338a")));

        //     ValidationResult validationResult = await CashReceiptForDebtIssueValidator.Validate(cashTransaction, _queryService);

        //     Assert.False(validationResult.IsValid); ;
        // }

        // [Fact]
        // public async Task Validate_CashReceiptForDebtIssueValidator_InvalidLoanId_ShouldFail()
        // {
        //     CashTransaction cashTransaction = GetCashTransactionLoanProceeds();
        //     // An existing loan agreement but not for this financier
        //     cashTransaction.UpdateEventId(EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")));

        //     ValidationResult validationResult = await CashReceiptForDebtIssueValidator.Validate(cashTransaction, _queryService);

        //     Assert.False(validationResult.IsValid); ;
        // }

        // // Test CashTransactionValidationService

        // [Fact]
        // public async Task Validate_validationService_LoanProceeds_ShouldSucceed()
        // {
        //     CashTransaction cashTransaction = GetCashTransactionLoanProceeds();

        //     ICashTransactionValidationService cashTransactionValidationService = new CashTransactionValidationService(_queryService);
        //     ValidationResult validationResult = await cashTransactionValidationService.IsValid(cashTransaction);

        //     Assert.True(validationResult.IsValid);
        // }

        // [Fact]
        // public async Task Validate_validationService_LoanPymt_ShouldSucceed()
        // {
        //     CashTransaction cashTransaction = GetCashTransactionLoanInstallmentPymt();

        //     ICashTransactionValidationService cashTransactionValidationService = new CashTransactionValidationService(_queryService);
        //     ValidationResult validationResult = await cashTransactionValidationService.IsValid(cashTransaction);

        //     Assert.True(validationResult.IsValid);
        // }

        // Test data

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