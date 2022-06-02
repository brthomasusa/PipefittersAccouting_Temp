using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate
{
    public class EditDividendDeclarationValidation
    {
        public static async Task<ValidationResult> Validate(DividendDeclarationWriteModel dividendInfo, IStockSubscriptionQueryService queryService)
        {
            CannotEditDeleteDividendDeclarationIfPaidRule dividendNotPaidRule = new(queryService);

            return await dividendNotPaidRule.Validate(dividendInfo);
        }
    }
}