using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Interfaces
{
    public interface ILoanAgreementRepository
    {
        Task<OperationResult<LoanAgreementReadModel>> CreateLoanAgreement(LoanAgreementWriteModel writeModel);
        Task<OperationResult<bool>> EditLoanAgreement(LoanAgreementWriteModel writeModel);
        Task<OperationResult<bool>> DeleteLoanAgreement(LoanAgreementWriteModel writeModel);

        Task<OperationResult<LoanAgreementReadModel>> GetLoanAgreementDetails(GetLoanAgreement queryParameters);
        Task<OperationResult<PagingResponse<LoanAgreementListItem>>> GetLoanAgreementListItems(GetLoanAgreements queryParameters);
        Task<OperationResult<PagingResponse<LoanAgreementListItem>>> GetLoanAgreementListItems(GetLoanAgreementByLoanNumber queryParameters);
        Task<OperationResult<List<FinancierLookup>>> GetFinanciersLookup(GetFinanciersLookup queryParameters);
    }
}