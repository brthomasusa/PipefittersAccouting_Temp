using PipefittersAccounting.Infrastructure.Application.Queries.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing.StockSubscriptionAggregate
{
    public class StockSubscriptionQueryService : IStockSubscriptionQueryService
    {
        private readonly DapperContext _dapperCtx;

        public StockSubscriptionQueryService(DapperContext ctx) => _dapperCtx = ctx;

        public async Task<OperationResult<StockSubscriptionReadModel>> GetStockSubscriptionReadModel(GetStockSubscriptionParameter queryParameters)
            => await GetStockSubscriptionReadModelQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<PagedList<StockSubscriptionListItem>>> GetStockSubscriptionListItems(GetStockSubscriptionListItem queryParameters)
            => await GetStockSubscriptionListItemsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<PagedList<StockSubscriptionListItem>>> GetStockSubscriptionListItemsByFundsRcvd(GetStockSubscriptionListItem queryParameters)
            => await GetStockSubscriptionListItemsQuery.QueryByFundsRcvd(queryParameters, _dapperCtx);

        public async Task<OperationResult<PagedList<StockSubscriptionListItem>>> GetStockSubscriptionListItemsByFundsNotRcvd(GetStockSubscriptionListItem queryParameters)
            => await GetStockSubscriptionListItemsQuery.QueryByFundsNotRcvd(queryParameters, _dapperCtx);

        public async Task<OperationResult<PagedList<StockSubscriptionListItem>>> GetStockSubscriptionListItems(GetStockSubscriptionListItemByInvestorName queryParameters)
            => await GetStockSubscriptionListItemsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<DividendDeclarationReadModel>> GetDividendDeclarationReadModel(GetDividendDeclarationParameter queryParameters)
            => await GetDividendDeclarationDetailsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<PagedList<DividendDeclarationListItem>>> GetDividendDeclarationListItems(GetDividendDeclarationsParameters queryParameters)
            => await GetDividendDeclarationListItemsQuery.Query(queryParameters, _dapperCtx);



        public async Task<OperationResult<Guid>> VerifyStockSubscriptionIsUnique(UniqueStockSubscriptionParameters queryParameters)
            => await VerifyStockSubscriptionIsUniqueQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<VerificationOfCashDepositStockIssueProceeds>>
            VerifyCashDepositOfStockIssueProceeds(GetStockSubscriptionParameter queryParameters)
                => await VerifyCashDepositOfStockIssueProceedsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<VerifyCashDisbursementForDividendPayment>>
            VerifyCashDisbursementDividendPayment(GetDividendDeclarationParameter queryParameters)
                => await VerifyCashDisbursementDividendPaymentQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> VerifyStockSubscriptionIdentification(GetStockSubscriptionParameter queryParameters)
            => await VerifyStockSubscriptionIdentificationQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> VerifyInvestorIdentification(GetInvestorIdentificationParameter queryParameters)
            => await VerifyInvestorIdentificationQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> VerifyDividendDeclarationIdentification(GetDividendDeclarationParameter queryParameters)
            => await VerifyDividendDeclarationIdentificationQuery.Query(queryParameters, _dapperCtx);


    }
}