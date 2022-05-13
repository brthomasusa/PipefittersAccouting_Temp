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
    public class CashAccountCreateCommand
    {
        public static async Task<OperationResult<bool>> Process
        (
            CreateCashAccountInfo model,
            ICashAccountAggregateRepository repository,
            ICashAccountAggregateValidationService validationService,
            IUnitOfWork uow
        )
        {
            OperationResult<bool> result = await repository.DoesCashAccountExist(model.CashAccountId);

            if (result.Success)
            {
                string errMsg = $"Create operation failed! A cash account with this Id: {model.CashAccountId} already exists!";
                return OperationResult<bool>.CreateFailure(errMsg);
            }

            ValidationResult validationResult = await validationService.IsValidCreateCashAccountInfo(model);

            if (validationResult.IsValid)
            {
                try
                {
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

                    await repository.AddCashAccountAsync(cashAccount);
                    await uow.Commit();

                    // CashAccountCreatedEvent createCashAcct = new(cashAccount);
                    // DomainEvent.Raise(createCashAcct);

                    return OperationResult<bool>.CreateSuccessResult(true);
                }
                catch (Exception ex)
                {
                    return OperationResult<bool>.CreateFailure(ex.Message);
                }
            }
            else
            {
                return OperationResult<bool>.CreateFailure(validationResult.Messages[0]);
            }
        }
    }
}
