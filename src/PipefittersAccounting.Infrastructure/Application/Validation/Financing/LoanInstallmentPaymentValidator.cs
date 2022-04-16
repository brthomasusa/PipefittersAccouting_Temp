using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing
{
    public class LoanInstallmentPaymentValidator
    {
        public static Task<ValidationResult> Validate(CashTransaction transaction, ICashAccountQueryService queryService)
        {


            throw new NotImplementedException();
        }
    }
}