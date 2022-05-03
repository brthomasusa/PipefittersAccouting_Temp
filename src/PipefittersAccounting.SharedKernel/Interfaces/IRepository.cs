using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.SharedKernel.Interfaces
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        Task<OperationResult<T>> GetByIdAsync(Guid id);
        Task<OperationResult<bool>> Exists(Guid id);
        Task<OperationResult<bool>> AddAsync(T entity);
        OperationResult<bool> Update(T entity);
        OperationResult<bool> Delete(T entity);
    }
}