using PipefittersAccounting.SharedKernel.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;

namespace PipefittersAccounting.Core.Interfaces.Financing
{
    public interface ICashAccountAggregateRepository
    {
        Task<OperationResult<CashAccount>> GetByIdAsync(Guid id);
        Task<OperationResult<bool>> Exists(Guid id);
        Task<OperationResult<bool>> AddAsync(CashAccount entity);
        Task<OperationResult<bool>> Update(CashAccount entity);
        OperationResult<bool> Delete(CashAccount entity);
    }
}
