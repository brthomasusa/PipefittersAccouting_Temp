using PipefittersAccounting.Core.Financing.CashAccountAggregate;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public interface ICashAccountAggregateValidationService
    {
        Task<ValidationResult> IsValidCashDisbursement(CashDisbursement disbursement);

        Task<ValidationResult> IsValidCashDeposit(CashDeposit deposit);
    }
}