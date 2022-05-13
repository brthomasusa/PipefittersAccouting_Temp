using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class LoanInstallmentPaymentValidation
    {
        public static async Task<ValidationResult> Validate
        (
            CreateCashAccountTransactionInfo transactionInfo,
            ICashAccountQueryService queryService
        )
        {
            FinancierAsPayorIdentificationValidator financierValidator = new(queryService);
            // DisburesementForLoanPymtValidator disburesementForLoanPymtValidator = new(queryService);

            // financierValidator.Next = disburesementForLoanPymtValidator;

            return await financierValidator.Validate(transactionInfo);

            throw new NotImplementedException();
        }
    }
}
