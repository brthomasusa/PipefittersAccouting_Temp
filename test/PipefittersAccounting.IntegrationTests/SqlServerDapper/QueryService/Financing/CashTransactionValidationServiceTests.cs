using System;
using System.Threading.Tasks;
using Xunit;

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Services.Financing;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.IntegrationTests.SqlServerDapper.QueryService.Financing
{
    [Trait("Integration", "DapperQueryService")]
    public class CashTransactionValidationServiceTests : TestBaseDapper
    {
        private readonly ICashAccountQueryService _queryService;
        private readonly ICashTransactionValidationService _cashTransactionValidationService;

        public CashTransactionValidationServiceTests()
        {
            _queryService = new CashAccountQueryService(_dapperCtx);
            _cashTransactionValidationService = new CashTransactionValidationService(_queryService);
        }

        [Fact]
        public async Task Validate_FinancierValidator_ShouldSucceed()
        {
            CashTransaction cashTransaction = GetCashTransactionLoanProceeds();
            FinancierValidator financierValidator = new(_queryService);

            ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_FinancierValidator_InvalidFinancierId_ShouldFail()
        {
            CashTransaction cashTransaction = GetCashTransactionLoanProceeds();
            cashTransaction.UpdateAgentId(EntityGuidID.Create(new Guid("123471a0-5c1e-4a4d-97e7-288fb0f6338a")));
            FinancierValidator financierValidator = new(_queryService);

            ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

            string errMsg = $"Unable to locate a financier with FinancierId: {cashTransaction.AgentId}.";
            Assert.False(validationResult.IsValid);
            Assert.Equal(errMsg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_CreditorHasLoanAgreeValidator_ShouldSucceed()
        {
            CashTransaction cashTransaction = GetCashTransactionLoanProceeds();

            FinancierValidator financierValidator = new(_queryService);
            CreditorHasLoanAgreeValidator creditorHasLoanAgreeValidator = new(_queryService);

            financierValidator.Next = creditorHasLoanAgreeValidator;

            ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CreditorHasLoanAgreeValidator_InvalidLoanId_ShouldFail()
        {
            CashTransaction cashTransaction = GetCashTransactionLoanProceeds();
            // An existing loan agreement but not for this financier
            cashTransaction.UpdateEventId(EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")));

            FinancierValidator financierValidator = new(_queryService);
            CreditorHasLoanAgreeValidator creditorHasLoanAgreeValidator = new(_queryService);

            financierValidator.Next = creditorHasLoanAgreeValidator;

            ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

            string errMsg = $"Unable to locate a loan agreement with loan Id: {cashTransaction.EventId} for financier: {cashTransaction.AgentId}.";
            Assert.False(validationResult.IsValid);
            Assert.Equal(errMsg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_ReceiptLoanProceedsValidator_ShouldSucceed()
        {
            CashTransaction cashTransaction = GetCashTransactionLoanProceeds();

            FinancierValidator financierValidator = new(_queryService);
            CreditorHasLoanAgreeValidator creditorHasLoanAgreeValidator = new(_queryService);
            ReceiptLoanProceedsValidator receiptLoanProceedsValidator = new(_queryService);

            financierValidator.Next = creditorHasLoanAgreeValidator;
            creditorHasLoanAgreeValidator.Next = receiptLoanProceedsValidator;

            ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_ReceiptLoanProceedsValidator_AmountsNotEqual_ShouldFail()
        {
            // Loan amount and deposit amount don't match
            CashTransaction cashTransaction = GetCashTransactionLoanProceeds();
            cashTransaction.UpdateCashTransactionAmount(CashTransactionAmount.Create(3000M));

            FinancierValidator financierValidator = new(_queryService);
            CreditorHasLoanAgreeValidator creditorHasLoanAgreeValidator = new(_queryService);
            ReceiptLoanProceedsValidator receiptLoanProceedsValidator = new(_queryService);

            financierValidator.Next = creditorHasLoanAgreeValidator;
            creditorHasLoanAgreeValidator.Next = receiptLoanProceedsValidator;

            ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

            Assert.False(validationResult.IsValid);
            string errMsg = "The loan proceed amount and the loan agreement amount do not match.";
            Assert.Equal(errMsg, validationResult.Messages[0]);
        }

        [Fact]
        public async Task Validate_ReceiptLoanProceedsValidator_DepositedAlready_ShouldFail()
        {
            // Loan amount and deposit amount don't match
            CashTransaction cashTransaction = GetCashTransactionLoanProceedsDuplicateDeposit();

            FinancierValidator financierValidator = new(_queryService);
            CreditorHasLoanAgreeValidator creditorHasLoanAgreeValidator = new(_queryService);
            ReceiptLoanProceedsValidator receiptLoanProceedsValidator = new(_queryService);

            financierValidator.Next = creditorHasLoanAgreeValidator;
            creditorHasLoanAgreeValidator.Next = receiptLoanProceedsValidator;

            ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisburesementForLoanPymtValidator_ShouldSucceed()
        {
            // Loan amount and deposit amount don't match
            CashTransaction cashTransaction = GetCashTransactionLoanInstallmentPymt();

            FinancierValidator financierValidator = new(_queryService);
            DisburesementForLoanPymtValidator paymentValidator = new(_queryService);

            financierValidator.Next = paymentValidator;

            ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisburesementForLoanPymtValidator_ExistingButInvalidFinancierID_ShouldFail()
        {
            CashTransaction cashTransaction = GetCashTransactionLoanInstallmentPymt();
            cashTransaction.UpdateAgentId(EntityGuidID.Create(new Guid("12998229-7ede-4834-825a-0c55bde75695")));

            FinancierValidator financierValidator = new(_queryService);
            DisburesementForLoanPymtValidator paymentValidator = new(_queryService);

            financierValidator.Next = paymentValidator;

            ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisburesementForLoanPymtValidator_InvalidPymtAmt_ShouldFail()
        {
            // Installment amount and payment amount don't match
            CashTransaction cashTransaction = GetCashTransactionLoanInstallmentPymt();
            cashTransaction.UpdateCashTransactionAmount(CashTransactionAmount.Create(1000M));

            FinancierValidator financierValidator = new(_queryService);
            DisburesementForLoanPymtValidator paymentValidator = new(_queryService);

            financierValidator.Next = paymentValidator;

            ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_DisburesementForLoanPymtValidator_PaidAlready_ShouldFail()
        {
            // Installment amount and payment amount don't match
            CashTransaction cashTransaction = GetCashTransactionLoanInstallmentPymtAlreadyPaid();

            FinancierValidator financierValidator = new(_queryService);
            DisburesementForLoanPymtValidator paymentValidator = new(_queryService);

            financierValidator.Next = paymentValidator;

            ValidationResult validationResult = await financierValidator.Validate(cashTransaction);

            Assert.False(validationResult.IsValid);
        }

        // Test CashReceiptForDebtIssueValidator

        [Fact]
        public async Task Validate_CashReceiptForDebtIssueValidator_ShouldSucceed()
        {
            CashTransaction cashTransaction = GetCashTransactionLoanProceeds();

            ValidationResult validationResult = await CashReceiptForDebtIssueValidator.Validate(cashTransaction, _queryService);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashReceiptForDebtIssueValidator_InvalidFinancierId_ShouldFail()
        {
            CashTransaction cashTransaction = GetCashTransactionLoanProceeds();
            cashTransaction.UpdateAgentId(EntityGuidID.Create(new Guid("123471a0-5c1e-4a4d-97e7-288fb0f6338a")));

            ValidationResult validationResult = await CashReceiptForDebtIssueValidator.Validate(cashTransaction, _queryService);

            Assert.False(validationResult.IsValid); ;
        }

        [Fact]
        public async Task Validate_CashReceiptForDebtIssueValidator_InvalidLoanId_ShouldFail()
        {
            CashTransaction cashTransaction = GetCashTransactionLoanProceeds();
            // An existing loan agreement but not for this financier
            cashTransaction.UpdateEventId(EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")));

            ValidationResult validationResult = await CashReceiptForDebtIssueValidator.Validate(cashTransaction, _queryService);

            Assert.False(validationResult.IsValid); ;
        }

        // Test CashTransactionValidationService

        [Fact]
        public async Task Validate_CashTransactionValidationService_LoanProceeds_ShouldSucceed()
        {
            CashTransaction cashTransaction = GetCashTransactionLoanProceeds();

            ICashTransactionValidationService cashTransactionValidationService = new CashTransactionValidationService(_queryService);
            ValidationResult validationResult = await cashTransactionValidationService.IsValid(cashTransaction);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task Validate_CashTransactionValidationService_LoanPymt_ShouldSucceed()
        {
            CashTransaction cashTransaction = GetCashTransactionLoanInstallmentPymt();

            ICashTransactionValidationService cashTransactionValidationService = new CashTransactionValidationService(_queryService);
            ValidationResult validationResult = await cashTransactionValidationService.IsValid(cashTransaction);

            Assert.True(validationResult.IsValid);
        }

        // Test data

        private CashTransaction GetCashTransactionLoanProceeds()
            => new
            (
                CashTransactionTypeEnum.CashReceiptDebtIssueProceeds,
                EntityGuidID.Create(new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e")),  // CashAccount Id financing proceeds
                CashTransactionDate.Create(new DateTime(2022, 4, 15)),                  // Loan date
                CashTransactionAmount.Create(4000M),                                    // LoanAmount
                EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")),  // AgentId financier
                EntityGuidID.Create(new Guid("17b447ea-90a7-45c3-9fc2-c4fb2ea71867")),  // EventId loan agreement id
                CheckNumber.Create("2001"),
                RemittanceAdvice.Create("ABCDE"),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        private CashTransaction GetCashTransactionLoanProceedsDuplicateDeposit()
            => new
            (
                CashTransactionTypeEnum.CashReceiptDebtIssueProceeds,
                EntityGuidID.Create(new Guid("6a7ed605-c02c-4ec8-89c4-eac6306c885e")),  // CashAccount Id financing proceeds
                CashTransactionDate.Create(new DateTime(2022, 1, 5)),
                CashTransactionAmount.Create(25000M),
                EntityGuidID.Create(new Guid("12998229-7ede-4834-825a-0c55bde75695")),  // AgentId financier
                EntityGuidID.Create(new Guid("41ca2b0a-0ed5-478b-9109-5dfda5b2eba1")),  // EventId loan agreement id
                CheckNumber.Create("2001"),
                RemittanceAdvice.Create("ABCDE"),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        private CashTransaction GetCashTransactionLoanInstallmentPymt()
            => new
            (
                CashTransactionTypeEnum.CashDisbursementLoanPayment,
                EntityGuidID.Create(new Guid("417f8a5f-60e7-411a-8e87-dfab0ae62589")),  // CashAccount Id primary checking
                CashTransactionDate.Create(new DateTime(2022, 7, 15)),                  // Payment due date
                CashTransactionAmount.Create(1100M),                                    // Equal Monthly Installment
                EntityGuidID.Create(new Guid("b49471a0-5c1e-4a4d-97e7-288fb0f6338a")),  // AgentId financier
                EntityGuidID.Create(new Guid("0bd39edb-8da3-40f9-854f-b90e798b82c2")),  // EventId loan installment id
                CheckNumber.Create("2011"),
                RemittanceAdvice.Create("ABCDE"),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );

        private CashTransaction GetCashTransactionLoanInstallmentPymtAlreadyPaid()
            => new
            (
                CashTransactionTypeEnum.CashDisbursementLoanPayment,
                EntityGuidID.Create(new Guid("417f8a5f-60e7-411a-8e87-dfab0ae62589")),  // CashAccount Id primary checking
                CashTransactionDate.Create(new DateTime(2022, 3, 5)),                  // Payment due date
                CashTransactionAmount.Create(2186.28M),                                    // Equal Monthly Installment
                EntityGuidID.Create(new Guid("12998229-7ede-4834-825a-0c55bde75695")),  // AgentId financier
                EntityGuidID.Create(new Guid("f479f59a-5001-47af-9d6c-2eae07077490")),  // EventId loan installment id
                CheckNumber.Create("2011"),
                RemittanceAdvice.Create("ABCDE"),
                EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
            );
    }
}