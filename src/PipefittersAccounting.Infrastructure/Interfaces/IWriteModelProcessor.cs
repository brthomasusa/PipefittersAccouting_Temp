using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.SharedModel.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Interfaces
{
    public interface IWriteModelProcessor<TWriteModel, TAggregateReposity>
        where TWriteModel : IWriteModel
        where TAggregateReposity : IAggregateRootRepository
    {
        Task<OperationResult<bool>> Process(TWriteModel writeModel, TAggregateReposity repository, IUnitOfWork uow);
    }
}