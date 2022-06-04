using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class LoanInstallmentPaymentValidation
    {
        public static async Task<ValidationResult> Validate
        (
            CashTransactionWriteModel transactionInfo,
            ICashAccountQueryService queryService
        )
        {
            FinancierAsExternalAgentRule financierValidator = new(queryService);
            // DisburesementForLoanPymtValidator disburesementForLoanPymtValidator = new(queryService);

            // financierValidator.Next = disburesementForLoanPymtValidator;

            return await financierValidator.Validate(transactionInfo);

            throw new NotImplementedException();
        }
    }
}
