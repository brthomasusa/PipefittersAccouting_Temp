using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public interface ICashTransactionValidationService
    {
        Task<ValidationResult> IsValid(CashTransaction cashTransaction);
    }
}