using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface IStockSubscriptionQueryService
    {
        Task<OperationResult<StockSubscriptionReadModel>> GetStockSubscriptionReadModel(GetStockSubscriptionParameter queryParameters);
        Task<OperationResult<PagedList<StockSubscriptionListItem>>> GetStockSubscriptionListItems(GetStockSubscriptionListItem queryParameters);
        Task<OperationResult<PagedList<StockSubscriptionListItem>>> GetStockSubscriptionListItemsByFundsRcvd(GetStockSubscriptionListItem queryParameters);
        Task<OperationResult<PagedList<StockSubscriptionListItem>>> GetStockSubscriptionListItemsByFundsNotRcvd(GetStockSubscriptionListItem queryParameters);
        Task<OperationResult<PagedList<StockSubscriptionListItem>>> GetStockSubscriptionListItems(GetStockSubscriptionListItemByInvestorName queryParameters);
        Task<OperationResult<DividendDeclarationReadModel>> GetDividendDeclarationReadModel(GetDividendDeclarationParameter queryParameters);
        Task<OperationResult<PagedList<DividendDeclarationListItem>>> GetDividendDeclarationListItems(GetDividendDeclarationsParameters queryParameters);


        Task<OperationResult<Guid>> VerifyStockSubscriptionIsUnique(UniqueStockSubscriptionParameters queryParameters);
        Task<OperationResult<VerificationOfCashDepositStockIssueProceeds>> VerifyCashDepositOfStockIssueProceeds(GetStockSubscriptionParameter queryParameters);
        Task<OperationResult<VerifyCashDisbursementForDividendPayment>> VerifyCashDisbursementDividendPayment(GetDividendDeclarationParameter queryParameters);
        Task<OperationResult<Guid>> VerifyStockSubscriptionIdentification(GetStockSubscriptionParameter queryParameters);
        Task<OperationResult<Guid>> VerifyInvestorIdentification(GetInvestorIdentificationParameter queryParameters);
        Task<OperationResult<Guid>> VerifyDividendDeclarationIdentification(GetDividendDeclarationParameter queryParameters);
    }
}