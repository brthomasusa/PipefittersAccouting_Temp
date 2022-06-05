#pragma warning disable CS8602

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Shared;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules
{
    // A cash transaction must point to the event that cause 
    // the deposit or disbursement of cash. For deposit of stock issue
    // proceeds that event should be a specific stock subscription.

    public class VerifyEventIsStockSubscriptionRule : BusinessRule<CashTransactionWriteModel>
    {
        private readonly ISharedQueryService _qrySvc;

        public VerifyEventIsStockSubscriptionRule(ISharedQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(CashTransactionWriteModel transactionInfo)
        {
            ValidationResult validationResult = new();

            EventIdentificationParameter queryParameters = new() { EventId = transactionInfo.EventId };
            OperationResult<EventIdentificationInfo> eventResult =
                await _qrySvc.GetEconomicEventIdentificationInfo(queryParameters);

            // Is the event id known to the system?
            if (eventResult.Success)
            {
                // Does the event id represent a stock subscription?
                if ((EventTypeEnum)eventResult.Result.EventTypeId == EventTypeEnum.StockSubscription)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(transactionInfo);
                    }
                }
                else
                {
                    string msg = $"An event of type '{eventResult.Result.EventTypeName}' is not valid for this operation. Expecting a stock subscription!";
                    validationResult.Messages.Add(msg);
                }
            }
            else
            {
                validationResult.Messages.Add(eventResult.NonSuccessMessage);
            }

            return validationResult;
        }
    }
}