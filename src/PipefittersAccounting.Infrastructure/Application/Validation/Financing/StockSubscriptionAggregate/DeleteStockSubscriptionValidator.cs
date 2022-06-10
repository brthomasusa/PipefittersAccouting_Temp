using PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate
{
    public class DeleteStockSubscriptionValidator : ValidatorBase<StockSubscriptionWriteModel, IQueryServicesRegistry>
    {
        public DeleteStockSubscriptionValidator
        (
            StockSubscriptionWriteModel writeModel,
            IQueryServicesRegistry queryServicesRegistry

        ) : base(writeModel, queryServicesRegistry)
        {

        }

        public override async Task<ValidationResult> Validate()
        {
            StockSubscriptionQueryService queryService
                = QueryServicesRegistry.GetService<StockSubscriptionQueryService>("StockSubscriptionQueryService");

            VerifyStockSubscriptionStockIdRule stockIdRule = new(queryService);
            CannotEditDeleteIfStockIssueProceedsHaveBeenRcvdRule checkDepositRule = new(queryService);

            stockIdRule.SetNext(checkDepositRule);

            return await stockIdRule.Validate(WriteModel);
        }
    }
}