using PipefittersAccounting.Infrastructure.Application.Queries.Financing.StockSubscriptionAggregate;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
    public class StockSubscriptionQueryService : IStockSubscriptionQueryService
    {
        private readonly DapperContext _dapperCtx;

        public StockSubscriptionQueryService(DapperContext ctx) => _dapperCtx = ctx;

        public async Task<OperationResult<StockSubscriptionDetails>> GetStockSubscriptionDetails(GetStockSubscriptionParameters queryParameters)
            => await GetStockSubscriptionDetailsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<PagedList<StockSubscriptionListItem>>> GetStockSubscriptionListItems(GetStockSubscriptionListItemParameters queryParameters)
            => await GetStockSubscriptionListItemsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> VerifyStockSubscriptionIsUnique(UniqueStockSubscriptionParameters queryParameters)
            => await VerifyStockSubscriptionIsUniqueQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<VerificationOfCashDepositStockIssueProceeds>>
            VerifyCashDepositOfStockIssueProceeds(GetStockSubscriptionParameters queryParameters)
                => await VerifyCashDepositOfStockIssueProceedsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<VerifyCashDisbursementForDividendPayment>>
            VerifyCashDisbursementDividendPayment(GetDividendDeclarationParameters queryParameters)
                => await VerifyCashDisbursementDividendPaymentQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> VerifyStockSubscriptionIdentification(GetStockSubscriptionParameters queryParameters)
            => await VerifyStockSubscriptionIdentificationQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> VerifyInvestorIdentification(GetInvestorIdentificationParameters queryParameters)
            => await VerifyInvestorIdentificationQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> VerifyDividendDeclarationIdentification(GetDividendDeclarationParameters queryParameters)
            => await VerifyDividendDeclarationIdentificationQuery.Query(queryParameters, _dapperCtx);
    }
}