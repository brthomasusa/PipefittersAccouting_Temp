using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class CashAccountTransferValidation
    {
        public static async Task<ValidationResult> Validate(CreateCashAccountTransferInfo accountInfo, ICashAccountQueryService queryService)
        {
            SourceCashAccountValidator sourceValidator = new(queryService);
            DestinationCashAccountValidator destinationValidator = new(queryService);
            SourceCashAccountBalanceValidator sourceBalanceValidator = new(queryService);

            sourceValidator.SetNext(destinationValidator);
            destinationValidator.SetNext(sourceBalanceValidator);

            return await sourceValidator.Validate(accountInfo);
        }
    }
}