using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public interface ILoanAgreementCommandService
    {
        Task<OperationResult<bool>> CreateLoanAgreement(CreateLoanAgreementInfo writeModel);
        Task<OperationResult<bool>> DeleteLoanAgreement(DeleteLoanAgreementInfo writeModel);
    }
}