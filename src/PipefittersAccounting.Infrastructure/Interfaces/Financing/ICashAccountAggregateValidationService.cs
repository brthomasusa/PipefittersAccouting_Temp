using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface ICashAccountAggregateValidationService
    {
        Task<ValidationResult> IsValidCashDisbursement(CashDisbursement disbursement);

        Task<ValidationResult> IsValidCashDeposit(CashDeposit deposit);

        Task<ValidationResult> IsValidCashAccount(CashAccount cashAccount);

        Task<ValidationResult> IsValidCreateCashAccountInfo(CreateCashAccountInfo writeModel);

        Task<ValidationResult> IsValidEditCashAccountInfo(EditCashAccountInfo writeModel);

        Task<ValidationResult> IsValidDeleteCashAccountInfo(DeleteCashAccountInfo writeModel);
    }
}