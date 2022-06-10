using PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate
{
    public class EditStockSubscriptionValidator : ValidatorBase<StockSubscriptionWriteModel, IQueryServicesRegistry>
    {
        public EditStockSubscriptionValidator
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
            VerifyInvestorIdentificationRule financierIdRule = new(queryService);
            VerifyStockSubscriptionIsUniqueRule uniqueRule = new(queryService);
            CannotEditDeleteIfStockIssueProceedsHaveBeenRcvdRule checkDepositRule = new(queryService);


            stockIdRule.SetNext(financierIdRule);
            financierIdRule.SetNext(uniqueRule);
            uniqueRule.SetNext(checkDepositRule);

            return await stockIdRule.Validate(WriteModel);
        }
    }
}