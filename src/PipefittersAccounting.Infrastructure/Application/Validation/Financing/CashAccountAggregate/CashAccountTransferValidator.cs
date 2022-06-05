using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class CashAccountTransferValidator
    {
        public static async Task<ValidationResult> Validate(CashAccountTransferWriteModel accountInfo, ICashAccountQueryService queryService)
        {
            SourceCashAccountRule sourceValidator = new(queryService);
            DestinationCashAccountRule destinationValidator = new(queryService);
            SourceCashAccountBalanceRule sourceBalanceValidator = new(queryService);

            sourceValidator.SetNext(destinationValidator);
            destinationValidator.SetNext(sourceBalanceValidator);

            return await sourceValidator.Validate(accountInfo);
        }
    }
}