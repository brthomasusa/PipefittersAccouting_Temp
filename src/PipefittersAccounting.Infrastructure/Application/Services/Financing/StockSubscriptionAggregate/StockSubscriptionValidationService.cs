using PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing.StockSubscriptionAggregate
{
    public class StockSubscriptionValidationService : IStockSubscriptionValidationService
    {
        private readonly IStockSubscriptionQueryService _qrySvc;
        private IQueryServicesRegistry _servicesRegistry;

        public StockSubscriptionValidationService
        (
            IStockSubscriptionQueryService qrySvc,
            IQueryServicesRegistry servicesRegistry
        )
        {
            _qrySvc = qrySvc;
            _servicesRegistry = servicesRegistry;

            _servicesRegistry.RegisterService("StockSubscriptionQueryService", qrySvc);
        }

        public async Task<ValidationResult> IsValidCreateStockSubscriptionInfo(StockSubscriptionWriteModel writeModel)
        {
            CreateStockSubscriptionValidator validator = new(writeModel, _servicesRegistry);
            return await validator.Validate();
        }

        public async Task<ValidationResult> IsValidEditStockSubscriptionInfo(StockSubscriptionWriteModel writeModel)
        {
            EditStockSubscriptionValidator validator = new(writeModel, _servicesRegistry);
            return await validator.Validate();
        }

        public async Task<ValidationResult> IsValidDeleteStockSubscriptionInfo(StockSubscriptionWriteModel writeModel)
        {
            DeleteStockSubscriptionValidator validator = new(writeModel, _servicesRegistry);
            return await validator.Validate();
        }

        public async Task<ValidationResult> IsValidCreateDividendDeclarationInfo(DividendDeclarationWriteModel writeModel)
        {
            CreateDividendDeclarationValidator validator = new(writeModel, _servicesRegistry);
            return await validator.Validate();
        }

        public async Task<ValidationResult> IsValidEditDividendDeclarationInfo(DividendDeclarationWriteModel writeModel)
        {
            EditDividendDeclarationValidator validator = new(writeModel, _servicesRegistry);
            return await validator.Validate();
        }

        public async Task<ValidationResult> IsValidDeleteDividendDeclarationInfo(DividendDeclarationWriteModel writeModel)
        {
            EditDividendDeclarationValidator validator = new(writeModel, _servicesRegistry);
            return await validator.Validate();
        }
    }
}