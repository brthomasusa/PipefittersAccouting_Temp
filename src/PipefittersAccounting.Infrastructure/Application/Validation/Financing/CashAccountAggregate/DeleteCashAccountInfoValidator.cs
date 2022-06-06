using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class DeleteCashAccountInfoValidator
    {
        public static async Task<ValidationResult> Validate(CashAccountWriteModel accountInfo, ICashAccountQueryService queryService)
        {
            CannotEditOrDeleteCashAcctWithTransactionsRule transactionCountValidator = new(queryService);

            return await transactionCountValidator.Validate(accountInfo);
        }
    }
}