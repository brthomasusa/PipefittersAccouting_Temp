using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate
{
    public interface ILoanInstallmentPaymentValidator
    {
        OperationResult<bool> Validate(CashTransaction transaction);
    }
}