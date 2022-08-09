#pragma warning disable CS8604

using PipefittersAccounting.Core.CashManagement.CashAccountAggregate;
using PipefittersAccounting.Core.CashManagement.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.CashManagement;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing.CashAccountAggregate
{
    public class CashAccountCreateCommand
    {
        public static async Task<OperationResult<bool>> Process
        (
            CashAccountWriteModel model,
            ICashAccountAggregateRepository repository,
            ICashAccountAggregateValidationService validationService,
            IUnitOfWork _unitOfWork
        )
        {
            OperationResult<bool> result = await repository.Exists(model.CashAccountId);

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

                    await repository.AddAsync(cashAccount);
                    await _unitOfWork.Commit();

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
