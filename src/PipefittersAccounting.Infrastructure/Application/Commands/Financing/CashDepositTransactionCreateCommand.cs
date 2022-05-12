#pragma warning disable CS8604

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing
{
    public class CashDepositTransactionCreateCommand : IWriteModelProcessor<CreateCashDepositInfo, ICashAccountAggregateRepository>
    {
        private readonly ICashAccountAggregateValidationService _validationService;

        public CashDepositTransactionCreateCommand(ICashAccountAggregateValidationService validationService)
            => _validationService = validationService;

        public async Task<OperationResult<bool>> Process
        (
            CreateCashDepositInfo model,
            ICashAccountAggregateRepository repository,
            IUnitOfWork uow
        )
        {
            OperationResult<bool> result = await repository.DoesCashAccountExist(model.CashAccountId);

            if (!result.Success)
            {
                string errMsg = $"Create operation failed! Could not locate a cash account with this Id '{model.CashAccountId}'!";
                return OperationResult<bool>.CreateFailure(errMsg);
            }

            throw new NotImplementedException();
            // ValidationResult validationResult = await _validationService.IsValidCreateCashAccountInfo(model);

            // if (validationResult.IsValid)
            // {
            //     try
            //     {
            //         CashAccount cashAccount = new
            //         (
            //             EntityGuidID.Create(model.CashAccountId),
            //             (CashAccountTypeEnum)Enum.ToObject(typeof(CashAccountTypeEnum), model.CashAccountType),
            //             BankName.Create(model.BankName),
            //             CashAccountName.Create(model.CashAccountName),
            //             CashAccountNumber.Create(model.CashAccountNumber),
            //             RoutingTransitNumber.Create(model.RoutingTransitNumber),
            //             DateOpened.Create(model.DateOpened),
            //             EntityGuidID.Create(model.UserId)
            //         );

            //         await repository.AddCashAccountAsync(cashAccount);
            //         await uow.Commit();

            //         // CashAccountCreatedEvent createCashAcct = new(cashAccount);
            //         // DomainEvent.Raise(createCashAcct);

            //         return OperationResult<bool>.CreateSuccessResult(true);
            //     }
            //     catch (Exception ex)
            //     {
            //         return OperationResult<bool>.CreateFailure(ex.Message);
            //     }
            // }
            // else
            // {
            //     return OperationResult<bool>.CreateFailure(validationResult.Messages[0]);
            // }
        }
    }
}