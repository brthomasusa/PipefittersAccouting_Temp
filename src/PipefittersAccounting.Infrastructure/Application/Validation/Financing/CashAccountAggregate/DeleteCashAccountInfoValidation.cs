using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class DeleteCashAccountInfoValidation
    {
        public static async Task<ValidationResult> Validate(DeleteCashAccountInfo accountInfo, ICashAccountQueryService queryService)
        {
            CannotDeleteCashAcctIfTransactionsAttachedValidator transactionCountValidator = new(queryService);

            return await transactionCountValidator.Validate(accountInfo);
        }
    }
}