using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.SharedModel.Interfaces;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Interfaces
{
    public interface IWriteModelProcessor<TWriteModel, TAggregateReposity, TDomainService, TEntity>
        where TWriteModel : IWriteModel
        where TAggregateReposity : IAggregateRootRepository<TEntity>
        where TDomainService : IValidationService
    {
        Task<OperationResult<bool>> Process(TWriteModel writeModel,
                                            TAggregateReposity repository,
                                            TDomainService domainService,
                                            IUnitOfWork uow);
    }
}