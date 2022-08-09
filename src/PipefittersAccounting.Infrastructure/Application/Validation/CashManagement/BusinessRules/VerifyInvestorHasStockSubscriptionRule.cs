#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules
{
    // Verify that a financier and a stock subscription are known to the system  
    // and that the financier is associated with this particular stock subscription.

    public class VerifyInvestorHasStockSubscriptionRule : BusinessRule<CashTransactionWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public VerifyInvestorHasStockSubscriptionRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashTransactionWriteModel transaction)
        {
            ValidationResult validationResult = new();

            GetInvestorIdForStockSubscriptionParameter queryParameters =
                new() { StockId = transaction.EventId };

            OperationResult<Guid> result =
                await _cashAcctQrySvc.GetInvestorIdForStockSubscription(queryParameters);

            if (result.Success)
            {
                if (result.Result == transaction.AgentId)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(transaction);
                    }
                }
                else
                {
                    // The stock id and financier id have been validated.
                    // So, to be here means the stock subscription is known to the
                    // sytem but was not issued to this financier.
                    string msg = "Invalid stock subscription <--> investor combo! The stock subscription was not issued to this investor";
                    validationResult.Messages.Add(msg);
                }

            }
            else
            {
                validationResult.Messages.Add(result.NonSuccessMessage);
            }

            return validationResult; ;
        }
    }
}