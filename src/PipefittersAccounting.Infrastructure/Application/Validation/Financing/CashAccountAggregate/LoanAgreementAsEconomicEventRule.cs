#pragma warning disable CS8602

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    // A cash transaction must point to the event that cause 
    // the deposit or disbursement of cash. For deposit of debt issue
    // proceeds that event should be a specific loan agreement.

    public class LoanAgreementAsEconomicEventRule : BusinessRule<CashTransactionWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public LoanAgreementAsEconomicEventRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashTransactionWriteModel transactionInfo)
        {
            ValidationResult validationResult = new();

            EconomicEventIdentificationParameters queryParameters = new() { EventId = transactionInfo.EventId };
            OperationResult<EconomicEventIdentificationInfo> eventResult =
                await _cashAcctQrySvc.GetEconomicEventIdentificationInfo(queryParameters);

            // Is the event id known to the system?
            if (eventResult.Success)
            {
                // Does the event id represent a loan agreement?
                if ((EventTypeEnum)eventResult.Result.EventTypeId == EventTypeEnum.LoanAgreement)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(transactionInfo);
                    }
                }
                else
                {
                    string msg = $"An event of type '{eventResult.Result.EventTypeName}' is not valid for this operation. Expecting a loan agreement!";
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