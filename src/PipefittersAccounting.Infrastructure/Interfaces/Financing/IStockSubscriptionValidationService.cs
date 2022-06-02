using PipefittersAccounting.Core.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface IStockSubscriptionValidationService
    {
        Task<ValidationResult> IsValidCreateStockSubscriptionInfo(StockSubscriptionWriteModel writeModel);
        Task<ValidationResult> IsValidEditStockSubscriptionInfo(StockSubscriptionWriteModel writeModel);
        Task<ValidationResult> IsValidDeleteStockSubscriptionInfo(StockSubscriptionWriteModel writeModel);
        Task<ValidationResult> IsValidCreateDividendDeclarationInfo(DividendDeclarationWriteModel writeModel);
        Task<ValidationResult> IsValidEditDividendDeclarationInfo(DividendDeclarationWriteModel writeModel);
        Task<ValidationResult> IsValidDeleteDividendDeclarationInfo(DividendDeclarationWriteModel writeModel);
    }
}