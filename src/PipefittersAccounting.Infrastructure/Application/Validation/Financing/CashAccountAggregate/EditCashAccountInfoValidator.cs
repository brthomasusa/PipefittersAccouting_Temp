using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class EditCashAccountInfoValidator
    {
        public static async Task<ValidationResult> Validate(CashAccountWriteModel accountInfo, ICashAccountQueryService queryService)
        {
            EditedCashAccountNameMustBeUniqueRule acctNameValidator = new(queryService);
            CannotEditOrDeleteCashAcctWithTransactionsRule transactionCountValidator = new(queryService);

            acctNameValidator.SetNext(transactionCountValidator);

            return await acctNameValidator.Validate(accountInfo);
        }
    }
}