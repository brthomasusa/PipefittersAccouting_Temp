#pragma warning disable CS8602

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedModel.Readmodels.Shared;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules
{
    // A cash transaction must point to the event that cause 
    // the deposit or disbursement of cash. For disbursement for
    // loan payment that event should be a specific loan installment.

    public class VerifyEventIsLoanInstallmentRule : BusinessRule<CashTransactionWriteModel>
    {
        private readonly ISharedQueryService _qrySvc;

        public VerifyEventIsLoanInstallmentRule(ISharedQueryService qrySvc)
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
                // Does the event id represent a loan installment?
                if ((EventTypeEnum)eventResult.Result.EventTypeId == EventTypeEnum.LoanPayment)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(transactionInfo);
                    }
                }
                else
                {
                    string msg = $"An event of type '{eventResult.Result.EventTypeName}' is not valid for this operation. Expecting 'Cash Disbursement for Loan Payment'!";
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