#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate.BusinessRules
{
    public class VerifyDividendDeclarationStockIdRule : BusinessRule<DividendDeclarationWriteModel>
    {
        private readonly IStockSubscriptionQueryService _qrySvc;

        public VerifyDividendDeclarationStockIdRule(IStockSubscriptionQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(DividendDeclarationWriteModel subscriptionInfo)
        {
            ValidationResult validationResult = new();
            GetStockSubscriptionParameter queryParameters =
                new()
                {
                    StockId = subscriptionInfo.StockId
                };

            OperationResult<Guid> result =
                await _qrySvc.VerifyStockSubscriptionIdentification(queryParameters);

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
                    string msg = $"Validation failed: '{subscriptionInfo.StockId}' is not a valid stock subscription identifier.";
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