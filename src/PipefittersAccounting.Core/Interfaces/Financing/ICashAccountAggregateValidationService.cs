using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public interface ICashAccountAggregateValidationService
    {
        Task<ValidationResult> IsValidCashDisbursement(CashDisbursement disbursement);

        Task<ValidationResult> IsValidCashDeposit(CashDeposit deposit);

        Task<ValidationResult> IsValidCashAccount(CashAccount cashAccount);
    }
}