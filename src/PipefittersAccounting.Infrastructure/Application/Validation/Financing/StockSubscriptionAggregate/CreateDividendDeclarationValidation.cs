using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate
{
    public class CreateDividendDeclarationValidation
    {
        public static async Task<ValidationResult> Validate(DividendDeclarationWriteModel dividendInfo, IStockSubscriptionQueryService queryService)
        {
            VerifyDividendDeclarationStockIdRule verifyStockSubscription = new(queryService);
            CannotAddDividendUntilStockIssueProceedsHaveBeenRcvdRule proceedsHaveBeenRcvdRule = new(queryService);

            verifyStockSubscription.SetNext(proceedsHaveBeenRcvdRule);

            return await verifyStockSubscription.Validate(dividendInfo);
        }
    }
}