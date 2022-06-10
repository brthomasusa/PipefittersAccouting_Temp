using PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate.BusinessRules;
using PipefittersAccounting.Infrastructure.Application.Services.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate
{
    public class EditDividendDeclarationValidator : ValidatorBase<DividendDeclarationWriteModel, IQueryServicesRegistry>
    {
        public EditDividendDeclarationValidator
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

            CannotEditDeleteDividendDeclarationIfPaidRule dividendNotPaidRule = new(queryService);

            return await dividendNotPaidRule.Validate(WriteModel);
        }
    }
}