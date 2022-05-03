using PipefittersAccounting.Core.Financing.CashAccountAggregate;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public interface ICashTransactionValidator
    {
        Task<ValidationResult> Validate(CashAccountTransaction transaction);
    }
}