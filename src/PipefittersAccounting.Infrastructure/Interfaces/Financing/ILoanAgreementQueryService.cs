using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface ILoanAgreementQueryService
    {
        Task<OperationResult<LoanAgreementDetail>> GetLoanAgreementDetails(GetLoanAgreement queryParameters);
        Task<OperationResult<PagedList<LoanAgreementListItem>>> GetLoanAgreementListItems(GetLoanAgreements queryParameters);
    }
}