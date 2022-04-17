using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing
{
    public class LoanInstallmentPaymentValidator
    {
        public static async Task<ValidationResult> Validate(CashTransaction transaction, ICashAccountQueryService queryService)
        {
            FinancierValidator financierValidator = new(queryService);
            DisburesementForLoanPymtValidator disburesementForLoanPymtValidator = new(queryService);

            financierValidator.Next = disburesementForLoanPymtValidator;

            return await financierValidator.Validate(transaction);
        }
    }
}