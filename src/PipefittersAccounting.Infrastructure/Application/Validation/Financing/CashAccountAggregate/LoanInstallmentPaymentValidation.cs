using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class LoanInstallmentPaymentValidation
    {
        public static async Task<ValidationResult> Validate(CashAccountTransaction deposit, ICashAccountQueryService queryService)
        {
            // FinancierAsPayorIdentificationValidator financierValidator = new(queryService);
            // DisburesementForLoanPymtValidator disburesementForLoanPymtValidator = new(queryService);

            // financierValidator.Next = disburesementForLoanPymtValidator;

            // return await financierValidator.Validate(deposit);

            throw new NotImplementedException();
        }
    }
}
