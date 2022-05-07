#pragma warning disable CS8604

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.Events;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.EventHandlers.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing
{
    public class CashAccountCreateCommand : IWriteModelProcessor<CreateCashAccountInfo, ICashAccountAggregateRepository>
    {
        private readonly ICashAccountAggregateValidationService _validationService;

        public CashAccountCreateCommand(ICashAccountAggregateValidationService validationService)
            => _validationService = validationService;

        public async Task<OperationResult<bool>> Process
        (
            CreateCashAccountInfo model,
            ICashAccountAggregateRepository repository,
            IUnitOfWork uow
        )
        {
            OperationResult<bool> result = await repository.DoesCashAccountExist(model.CashAccountId);

            if (result.Success)
            {
                string errMsg = $"Create operation failed! A cash account with this Id: {model.CashAccountId} already exists!";
                return OperationResult<bool>.CreateFailure(errMsg);
            }

            CashAccount cashAccount = new
            (
                EntityGuidID.Create(model.CashAccountId),
                (CashAccountTypeEnum)Enum.ToObject(typeof(CashAccountTypeEnum), model.CashAccountType),
                BankName.Create(model.BankName),
                CashAccountName.Create(model.CashAccountName),
                CashAccountNumber.Create(model.CashAccountNumber),
                RoutingTransitNumber.Create(model.RoutingTransitNumber),
                DateOpened.Create(model.DateOpened),
                EntityGuidID.Create(model.UserId)
            );

            // Calling the CashAccount ctor causes a CreateCashAccount event to be raised.
            // The eventhandler for this event does validation and does nothing if no
            // validation errors were found and 'repository.AddCashAccountAsync(cashAccount)'
            // is called. 
            // CashAccountCreatedEvent createCashAcct = new(cashAccount);
            // DomainEvent.Raise(createCashAcct);

            await repository.AddCashAccountAsync(cashAccount);
            await uow.Commit();

            return OperationResult<bool>.CreateSuccessResult(true);
        }
    }
}
