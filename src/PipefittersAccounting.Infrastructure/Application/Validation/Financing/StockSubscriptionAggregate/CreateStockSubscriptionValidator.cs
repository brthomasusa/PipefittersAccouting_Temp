using PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate
{
    public class CreateStockSubscriptionValidator : ValidatorBase<StockSubscriptionWriteModel, IQueryServicesRegistry>
    {
        public CreateStockSubscriptionValidator
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

            VerifyInvestorIdentificationRule financierIdRule = new(queryService);
            VerifyStockSubscriptionIsUniqueRule uniqueRule = new(queryService);

            financierIdRule.SetNext(uniqueRule);

            return await financierIdRule.Validate(WriteModel);
        }
    }
}