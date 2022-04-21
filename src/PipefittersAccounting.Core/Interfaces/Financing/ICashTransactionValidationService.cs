using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Shared;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public interface ICashTransactionValidationService
    {
        Task<ValidationResult> IsValid(CashTransaction cashTransaction);

        Task<ValidationResult> IsValidCashDisbursement(CashDisbursement disbursement);

        Task<ValidationResult> IsValidCashDeposit(CashDeposit deposit);
    }
}