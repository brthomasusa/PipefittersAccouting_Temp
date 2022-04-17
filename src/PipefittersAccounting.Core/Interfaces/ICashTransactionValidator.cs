using PipefittersAccounting.Core.Financing.CashAccountAggregate;

namespace PipefittersAccounting.Core.Interfaces
{
    public interface ICashTransactionValidator
    {
        Task<ValidationResult> Validate(CashTransaction transaction);
    }
}