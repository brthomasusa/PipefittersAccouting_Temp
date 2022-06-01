using PipefittersAccounting.Infrastructure.Application.Validation.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
    public class StockSubscriptionValidationService : IStockSubscriptionValidationService
    {
        private readonly IStockSubscriptionQueryService _qrySvc;

        public StockSubscriptionValidationService(IStockSubscriptionQueryService qrySvc)
            => _qrySvc = qrySvc;

        public async Task<ValidationResult> IsValidCreateStockSubscriptionInfo(StockSubscriptionWriteModel writeModel)
            => await CreateStockSubscriptionValidation.Validate(writeModel, _qrySvc);

        public async Task<ValidationResult> IsValidEditStockSubscriptionInfo(StockSubscriptionWriteModel writeModel)
            => await EditStockSubscriptionValidation.Validate(writeModel, _qrySvc);

        public async Task<ValidationResult> IsValidDeleteStockSubscriptionInfo(StockSubscriptionWriteModel writeModel)
            => await DeleteStockSubscriptionValidation.Validate(writeModel, _qrySvc);
    }
}