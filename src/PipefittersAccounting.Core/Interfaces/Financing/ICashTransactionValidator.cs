using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public interface ICashTransactionValidator
    {
        Task<ValidationResult> Validate(CashAccountTransaction transaction);
    }
}