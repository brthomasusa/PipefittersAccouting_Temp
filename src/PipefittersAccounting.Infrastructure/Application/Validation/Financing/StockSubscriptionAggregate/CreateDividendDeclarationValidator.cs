using PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate
{
    public class CreateDividendDeclarationValidator : ValidatorBase<DividendDeclarationWriteModel, IQueryServicesRegistry>
    {
        public CreateDividendDeclarationValidator
        (
            DividendDeclarationWriteModel writeModel,
            IQueryServicesRegistry queryServicesRegistry

        ) : base(writeModel, queryServicesRegistry)
        {

        }

        public override async Task<ValidationResult> Validate()
        {
            StockSubscriptionQueryService queryService
                = QueryServicesRegistry.GetService<StockSubscriptionQueryService>("StockSubscriptionQueryService");

            VerifyDividendDeclarationStockIdRule verifyStockSubscription = new(queryService);
            CannotAddDividendUntilStockIssueProceedsHaveBeenRcvdRule proceedsHaveBeenRcvdRule = new(queryService);

            verifyStockSubscription.SetNext(proceedsHaveBeenRcvdRule);

            return await verifyStockSubscription.Validate(WriteModel);
        }
    }
}