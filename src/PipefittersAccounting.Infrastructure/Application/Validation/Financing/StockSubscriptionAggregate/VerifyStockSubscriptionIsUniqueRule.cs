#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate
{
    public class VerifyStockSubscriptionIsUniqueRule : BusinessRule<StockSubscriptionWriteModel>
    {
        private readonly IStockSubscriptionQueryService _qrySvc;

        public VerifyStockSubscriptionIsUniqueRule(IStockSubscriptionQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(StockSubscriptionWriteModel subscriptionInfo)
        {
            ValidationResult validationResult = new();
            UniqueStockSubscriptionParameters queryParameters =
                new()
                {
                    FinancierId = subscriptionInfo.FinancierId,
                    StockIssueDate = subscriptionInfo.StockIssueDate,
                    SharesIssued = subscriptionInfo.SharesIssued,
                    PricePerShare = subscriptionInfo.PricePerShare
                };

            OperationResult<Guid> result =
                await _qrySvc.VerifyStockSubscriptionIsUnique(queryParameters);

            if (result.Success)
            {
                if (result.Result == Guid.Empty || result.Result == subscriptionInfo.StockId)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next.Validate(subscriptionInfo);
                    }
                }
                else
                {
                    string msg = $"Validation failed: there is an existing stock subscription ({result.Result}) that matches this one.";
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