using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate
{
    public class DeleteStockSubscriptionValidation
    {
        public static async Task<ValidationResult> Validate(StockSubscriptionWriteModel subscription, IStockSubscriptionQueryService queryService)
        {
            VerifyStockSubscriptionStockIdRule stockIdRule = new(queryService);
            CannotEditDeleteIfStockIssueProceedsHaveBeenRcvdRule checkDepositRule = new(queryService);

            stockIdRule.SetNext(checkDepositRule);

            return await stockIdRule.Validate(subscription);
        }
    }
}