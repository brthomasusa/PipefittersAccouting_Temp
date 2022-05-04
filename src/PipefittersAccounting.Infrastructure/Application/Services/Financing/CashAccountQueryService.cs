using PipefittersAccounting.Infrastructure.Persistence.DatabaseContext;
using PipefittersAccounting.Infrastructure.Application.Queries.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
    public class CashAccountQueryService : ICashAccountQueryService
    {
        private readonly DapperContext _dapperCtx;

        public CashAccountQueryService(DapperContext ctx) => _dapperCtx = ctx;

        public async Task<OperationResult<FinancierIdValidationModel>>
            GetFinancierIdValidationModel(FinancierIdValidationParams queryParameters)
                => await GetFinancierIdValidationModelQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<CreditorHasLoanAgreeValidationModel>>
            GetCreditorHasLoanAgreeValidationModel(CreditorHasLoanAgreeValidationParams queryParameters)
                => await GetCreditorHasLoanAgreeValidationModelQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<DepositLoanProceedsValidationModel>>
            GetReceiptLoanProceedsValidationModel(ReceiptLoanProceedsValidationParams queryParameters)
                => await GetReceiptLoanProceedsValidationModelQuery.Query(queryParameters, _dapperCtx);

        public async Task<OperationResult<DisburesementLoanPymtValidationModel>>
            GetDisburesementLoanPymtValidationModel(DisburesementLoanPymtValidationParams queryParameters)
                => await GetDisburesementLoanPymtValidationModelQuery.Query(queryParameters, _dapperCtx);
    }
}