using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Interfaces
{
    public interface IStockSubscriptionRepository
    {
        // Write
        Task<OperationResult<StockSubscriptionReadModel>> CreateStockSubscription(StockSubscriptionWriteModel writeModel);
        Task<OperationResult<bool>> EditStockSubscription(StockSubscriptionWriteModel writeModel);
        Task<OperationResult<bool>> DeleteStockSubscription(StockSubscriptionWriteModel writeModel);

        // Read
        Task<OperationResult<StockSubscriptionReadModel>> GetStockSubscriptionReadModel(GetStockSubscriptionParameter queryParameters);
        Task<OperationResult<PagingResponse<StockSubscriptionListItem>>> GetStockSubscriptionListItems(GetStockSubscriptionListItem queryParameters);
        Task<OperationResult<PagingResponse<StockSubscriptionListItem>>> GetStockSubscriptionListItems(GetStockSubscriptionListItemByInvestorName queryParameters);
        Task<OperationResult<DividendDeclarationReadModel>> GetDividendDeclarationReadModel(GetDividendDeclarationParameter queryParameters);
        Task<OperationResult<PagingResponse<DividendDeclarationListItem>>> GetDividendDeclarationListItems(GetDividendDeclarationsParameters queryParameters);
    }
}