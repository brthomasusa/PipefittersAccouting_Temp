#pragma warning disable CS8602

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Shared;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules
{
    public class VerifyEventIsDividendDeclarationRule : BusinessRule<CashTransactionWriteModel>
    {
        private readonly ISharedQueryService _qrySvc;

        public VerifyEventIsDividendDeclarationRule(ISharedQueryService qrySvc)
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
                // Does the event id represent a dividend declaration?
                if ((EventTypeEnum)eventResult.Result.EventTypeId == EventTypeEnum.DividentDeclaration)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(transactionInfo);
                    }
                }
                else
                {
                    string msg = $"An event of type '{eventResult.Result.EventTypeName}' is not valid for this operation. Expecting a dividend declaration!";
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