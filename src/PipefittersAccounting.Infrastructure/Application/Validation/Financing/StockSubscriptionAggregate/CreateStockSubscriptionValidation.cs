using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate
{
    public class CreateStockSubscriptionValidation
    {
        public static async Task<ValidationResult> Validate(StockSubscriptionWriteModel subscription, IStockSubscriptionQueryService queryService)
        {
            VerifyInvestorIdentificationRule financierIdRule = new(queryService);
            VerifyStockSubscriptionIsUniqueRule uniqueRule = new(queryService);

            financierIdRule.SetNext(uniqueRule);

            return await financierIdRule.Validate(subscription);
        }
    }
}