using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Interfaces
{
    public abstract class WriteCommandCollectionHandler<TWriteModelCollection, TAggregateReposity, TDomainService, TEntity>
        where TWriteModelCollection : List<PayrollRegisterWriteModel>
        where TAggregateReposity : IAggregateRootRepository<TEntity>
        where TDomainService : IValidationService
    {
        public WriteCommandCollectionHandler
        (
            TWriteModelCollection writeModelCollection,
            TAggregateReposity repository,
            TDomainService validationService,
            IUnitOfWork uow
        )
        {
            WriteModels = writeModelCollection;
            Repository = repository;
            ValidationService = validationService;
            UnitOfWork = uow;
        }

        protected TWriteModelCollection WriteModels { get; init; }

        protected TAggregateReposity Repository { get; init; }

        protected TDomainService ValidationService { get; init; }

        protected IUnitOfWork UnitOfWork { get; init; }

        public async Task<OperationResult<bool>> Process(TWriteModelCollection writeModels)
        {
            ValidationResult validationResult = await Validate();

            if (validationResult.IsValid)
            {
                OperationResult<bool> result = await ProcessCommand();

                if (result.Success)
                {
                    return OperationResult<bool>.CreateSuccessResult(true);
                }
                else
                {
                    return OperationResult<bool>.CreateFailure(result.NonSuccessMessage);
                }
            }
            else
            {
                return OperationResult<bool>.CreateFailure(validationResult.Messages[0]);
            }
        }

        protected abstract Task<ValidationResult> Validate();

        protected abstract Task<OperationResult<bool>> ProcessCommand();

    }
}