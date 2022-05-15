using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    /*
        This class arranges a group of validators into a chain.
        Execution of the chain will completely validate a
        CreateCashAccountTransactionInfo. The position of each
        validator within the chain is important. Using the OCP,
        if new validation logic is required then we would create
        a new validation class rather than modifying this one.
    */
    public class CreateCashDepositOfDebtIssueProceedsValidation
    {
        public static async Task<ValidationResult> Validate
        (
            CreateCashAccountTransactionInfo deposit,
            ICashAccountQueryService queryService
        )
        {
            FinancierAsPayorIdentificationValidator financierValidator = new(queryService);
            // CreditorHasLoanAgreeValidator creditorHasLoanAgreeValidator = new(queryService);
            // ReceiptLoanProceedsValidator receiptLoanProceedsValidator = new(queryService);

            // financierValidator.Next = creditorHasLoanAgreeValidator;
            // creditorHasLoanAgreeValidator.Next = receiptLoanProceedsValidator;

            return await financierValidator.Validate(deposit);

            throw new NotImplementedException();
        }
    }
}