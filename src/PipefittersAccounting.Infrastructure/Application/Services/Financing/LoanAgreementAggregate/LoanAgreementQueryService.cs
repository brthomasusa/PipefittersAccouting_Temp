using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.Infrastructure.Application.Queries.Financing.LoanAgreementAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing.LoanAgreementAggregate
{
    public class LoanAgreementQueryService : ILoanAgreementQueryService
    {
        private readonly DapperContext _dapperCtx;

        public LoanAgreementQueryService(DapperContext ctx) => _dapperCtx = ctx;

        public async Task<OperationResult<LoanAgreementDetail>> GetLoanAgreementDetails(GetLoanAgreement queryParameters)
            => await GetLoanAgreementDetailsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<PagedList<LoanAgreementListItem>>> GetLoanAgreementListItems(GetLoanAgreements queryParameters)
            => await GetLoanAgreementListItemQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> GetLoanIdOfDuplicationLoanAgreement(GetDuplicateLoanAgreement queryParameters)
            => await GetLoanIdOfDuplicationLoanAgreementQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<decimal>> VerifyCashDepositForDebtIssueProceeds(ReceiptLoanProceedsValidationParams queryParameters)
            => await VerifyCashDepositForDebtIssueProceedsQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<Guid>> VerifyCreditorIsLinkedToLoanAgreement(ReceiptLoanProceedsValidationParams queryParameters)
            => await VerifyCreditorIsLinkedToLoanAgreementQuery.Query(queryParameters, _dapperCtx);
    }
}