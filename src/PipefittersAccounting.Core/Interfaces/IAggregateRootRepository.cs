using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Core.Interfaces
{
    public interface IAggregateRootRepository<TAggregateRootEntity>
    {
        Task<OperationResult<TAggregateRootEntity>> GetByIdAsync(Guid id);
        Task<OperationResult<bool>> Exists(Guid id);
        Task<OperationResult<bool>> AddAsync(TAggregateRootEntity entity);
        OperationResult<bool> Update(TAggregateRootEntity entity);
        OperationResult<bool> Delete(TAggregateRootEntity entity);
    }
}