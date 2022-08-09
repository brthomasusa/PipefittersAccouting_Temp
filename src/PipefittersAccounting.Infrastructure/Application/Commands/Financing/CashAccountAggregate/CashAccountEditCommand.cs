#pragma warning disable CS8604

using PipefittersAccounting.Core.CashManagement.CashAccountAggregate;
using PipefittersAccounting.Core.CashManagement.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.CashManagement;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing.CashAccountAggregate
{
    public class CashAccountEditCommand
    {
        public static async Task<OperationResult<bool>> Process
        (
            CashAccountWriteModel model,
            ICashAccountAggregateRepository repository,
            ICashAccountAggregateValidationService validationService,
            IUnitOfWork uow
        )
        {
            OperationResult<bool> result = await repository.Exists(model.CashAccountId);

            if (!result.Success)
            {
                string errMsg = $"Update cash account failed! A cash account with Id '{model.CashAccountId}' could not be located!";
                return OperationResult<bool>.CreateFailure(errMsg);
            }

            ValidationResult validationResult = await validationService.IsValidEditCashAccountInfo(model);

            if (validationResult.IsValid)
            {
                OperationResult<CashAccount> getResult = await repository.GetByIdAsync(model.CashAccountId);

                if (getResult.Success)
                {
                    try
                    {
                        CashAccount cashAccount = getResult.Result;

                        cashAccount.UpdateCashAccountType((CashAccountTypeEnum)model.CashAccountType);
                        cashAccount.UpdateBankName(BankName.Create(model.BankName));
                        cashAccount.UpdateCashAccountName(CashAccountName.Create(model.CashAccountName));
                        cashAccount.UpdateRoutingTransitNumber(RoutingTransitNumber.Create(model.RoutingTransitNumber));
                        cashAccount.UpdateDateOpened(DateOpened.Create(model.DateOpened));
                        cashAccount.UpdateUserId(EntityGuidID.Create(model.UserId));

                        await repository.UpdateCashAccountAsync(cashAccount);
                        await uow.Commit();

                        return OperationResult<bool>.CreateSuccessResult(true);
                    }
                    catch (Exception ex)
                    {
                        return OperationResult<bool>.CreateFailure(ex.Message);
                    }
                }
                else
                {
                    return OperationResult<bool>.CreateFailure(getResult.NonSuccessMessage);
                }
            }
            else
            {
                return OperationResult<bool>.CreateFailure(validationResult.Messages[0]);
            }
        }
    }
}