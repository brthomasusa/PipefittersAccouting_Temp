using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing
{
    public class CashReceiptForDebtIssueValidator
    {
        public static async Task<ValidationResult> Validate
        (
            CashAccountTransaction deposit,
            ICashAccountQueryService queryService
        )
        {
            // FinancierAsPayorIdentificationValidator financierValidator = new(queryService);
            // CreditorHasLoanAgreeValidator creditorHasLoanAgreeValidator = new(queryService);
            // ReceiptLoanProceedsValidator receiptLoanProceedsValidator = new(queryService);

            // financierValidator.Next = creditorHasLoanAgreeValidator;
            // creditorHasLoanAgreeValidator.Next = receiptLoanProceedsValidator;

            // return await financierValidator.Validate(deposit);

            throw new NotImplementedException();
        }

        public static Task<ValidationResult> Validate
        (
            Guid goodsOrServiceProvided,
            Guid purchasedBy,
            decimal transactionAmount,
            ICashAccountQueryService queryService
        )
        {
            // FinancierValidator financierValidator = new(queryService);
            // CreditorHasLoanAgreeValidator creditorHasLoanAgreeValidator = new(queryService);
            // ReceiptLoanProceedsValidator receiptLoanProceedsValidator = new(queryService);

            // financierValidator.Next = creditorHasLoanAgreeValidator;
            // creditorHasLoanAgreeValidator.Next = receiptLoanProceedsValidator;

            // return await financierValidator.Validate(transaction);
            throw new NotImplementedException();
        }
    }
}