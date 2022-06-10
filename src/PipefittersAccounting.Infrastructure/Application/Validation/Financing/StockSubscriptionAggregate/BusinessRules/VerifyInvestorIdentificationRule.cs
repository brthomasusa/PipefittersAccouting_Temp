#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate.BusinessRules
{
    public class VerifyInvestorIdentificationRule : BusinessRule<StockSubscriptionWriteModel>
    {
        private readonly IStockSubscriptionQueryService _qrySvc;

        public VerifyInvestorIdentificationRule(IStockSubscriptionQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(StockSubscriptionWriteModel subscriptionInfo)
        {
            ValidationResult validationResult = new();
            GetInvestorIdentificationParameter queryParameters =
                new()
                {
                    FinancierId = subscriptionInfo.FinancierId
                };

            OperationResult<Guid> result =
                await _qrySvc.VerifyInvestorIdentification(queryParameters);

            if (result.Success)
            {
                if (result.Result != Guid.Empty)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next.Validate(subscriptionInfo);
                    }
                }
                else
                {
                    string msg = $"Validation failed: '{subscriptionInfo.FinancierId}' is not a valid investor identifier.";
                    validationResult.Messages.Add(msg);
                }
            }
            else
            {
                validationResult.Messages.Add(result.NonSuccessMessage);
            }

            return validationResult;
        }
    }
}