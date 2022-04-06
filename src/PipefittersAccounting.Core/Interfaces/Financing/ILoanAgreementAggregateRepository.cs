using PipefittersAccounting.SharedKernel.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Financing.LoanAgreementAggregate;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public interface ILoanAgreementAggregateRepository
    {
        Task<OperationResult<LoanAgreement>> GetByIdAsync(Guid id);
        Task<OperationResult<bool>> Exists(Guid id);
        Task<OperationResult<bool>> AddAsync(LoanAgreement entity);
        OperationResult<bool> Delete(LoanAgreement entity);
    }
}