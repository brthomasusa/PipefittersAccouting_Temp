using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing
{
    public class CashReceiptForDebtIssueValidator
    {
        public static async Task<ValidationResult> Validate
        (
            CashTransaction transaction,
            ICashAccountQueryService queryService
        )
        {
            FinancierValidator financierValidator = new(queryService);
            CreditorHasLoanAgreeValidator creditorHasLoanAgreeValidator = new(queryService);
            ReceiptLoanProceedsValidator receiptLoanProceedsValidator = new(queryService);

            financierValidator.Next = creditorHasLoanAgreeValidator;
            creditorHasLoanAgreeValidator.Next = receiptLoanProceedsValidator;

            return await financierValidator.Validate(transaction);
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