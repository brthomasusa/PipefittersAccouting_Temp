using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface ILoanAgreementApplicationService
    {
        Task<OperationResult<bool>> CreateLoanAgreement(LoanAgreementWriteModel writeModel);
        Task<OperationResult<bool>> DeleteLoanAgreement(LoanAgreementWriteModel writeModel);
    }
}