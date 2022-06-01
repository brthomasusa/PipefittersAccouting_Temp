using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate
{
    public class EditStockSubscriptionValidation
    {
        public static async Task<ValidationResult> Validate(StockSubscriptionWriteModel subscription, IStockSubscriptionQueryService queryService)
        {
            VerifyStockSubscriptionIdentificationRule stockIdRule = new(queryService);
            VerifyInvestorIdentificationRule financierIdRule = new(queryService);
            VerifyStockSubscriptionIsUniqueRule uniqueRule = new(queryService);
            CannotEditOrDeleteIfStockIssueProceedsRcvdRule checkDepositRule = new(queryService);


            stockIdRule.SetNext(financierIdRule);
            financierIdRule.SetNext(uniqueRule);
            uniqueRule.SetNext(checkDepositRule);

            return await stockIdRule.Validate(subscription);
        }
    }
}