using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public abstract class BaseCashTransactionCommandDispatcher
    {
        public BaseCashTransactionCommandDispatcher
        (
            CreateCashAccountTransactionInfo model,
            ICashAccountAggregateRepository repository,
            ICashAccountAggregateValidationService validationService,
            IUnitOfWork unitOfWork
        )
        {
            Model = model;
            Repository = repository;
            ValidationService = validationService;
            UnitOfWork = unitOfWork;
        }

        protected CreateCashAccountTransactionInfo Model { get; init; }
        protected ICashAccountAggregateRepository Repository { get; init; }
        protected ICashAccountAggregateValidationService ValidationService { get; init; }
        protected IUnitOfWork UnitOfWork { get; init; }

        public abstract Task<OperationResult<bool>> Dispatch();
    }
}
