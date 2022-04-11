using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.Infrastructure.Application.Queries.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
    public class LoanAgreementQueryService : ILoanAgreementQueryService
    {
        private readonly DapperContext _dapperCtx;

        public LoanAgreementQueryService(DapperContext ctx) => _dapperCtx = ctx;

        public async Task<OperationResult<LoanAgreementDetail>> GetLoanAgreementDetails(GetLoanAgreement queryParameters)
        {
            OperationResult<LoanAgreementDetail> detail = await GetLoanAgreementDetailsQuery.Query(queryParameters, _dapperCtx);

            if (detail.Success)
            {
                GetLoanAgreementInstallments installmentParams = new GetLoanAgreementInstallments { LoanId = queryParameters.LoanId };
                OperationResult<List<LoanInstallmentListItem>> installments = await GetLoanInstallmentListItemQuery.Query(installmentParams, _dapperCtx);

                if (installments.Success)
                {
                    detail.Result.LoanInstallmentListItems = installments.Result;
                }
            }

            return detail;
        }

        public async Task<OperationResult<PagedList<LoanAgreementListItem>>> GetLoanAgreementListItems(GetLoanAgreements queryParameters)
            => await GetLoanAgreementListItemQuery.Query(queryParameters, _dapperCtx);

        private async Task<OperationResult<List<LoanInstallmentListItem>>> GetLoanAgreementInstallments(GetLoanAgreementInstallments queryParameters)
            => await GetLoanInstallmentListItemQuery.Query(queryParameters, _dapperCtx);
    }
}