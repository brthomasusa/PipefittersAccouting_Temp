using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.SharedModel.Interfaces;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Interfaces
{
    public abstract class WriteCommandHandler<TWriteModel, TAggregateReposity, TDomainService, TEntity>
        where TWriteModel : IWriteModel
        where TAggregateReposity : IAggregateRootRepository<TEntity>
        where TDomainService : IValidationService
    {
        public WriteCommandHandler
        (
            TWriteModel writeModel,
            TAggregateReposity repository,
            TDomainService validationService,
            IUnitOfWork uow
        )
        {
            WriteModel = writeModel;
            Repository = repository;
            ValidationService = validationService;
            UnitOfWork = uow;
        }

        protected TWriteModel WriteModel { get; init; }

        protected TAggregateReposity Repository { get; init; }

        protected TDomainService ValidationService { get; init; }

        protected IUnitOfWork UnitOfWork { get; init; }

        public async Task<OperationResult<bool>> Process()
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