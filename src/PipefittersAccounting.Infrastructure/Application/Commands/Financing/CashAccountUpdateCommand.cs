#pragma warning disable CS8604

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing
{
    public class CashAccountUpdateCommand
    {
        public static async Task<OperationResult<bool>> Process
        (
            CashAccountWriteModel model,
            ICashAccountAggregateRepository repository,
            ICashAccountAggregateValidationService validationService,
            IUnitOfWork uow
        )
        {
            OperationResult<bool> result = await repository.DoesCashAccountExist(model.CashAccountId);

            if (!result.Success)
            {
                string errMsg = $"Update cash account failed! A cash account with Id '{model.CashAccountId}' could not be located!";
                return OperationResult<bool>.CreateFailure(errMsg);
            }

            ValidationResult validationResult = await validationService.IsValidEditCashAccountInfo(model);

            if (validationResult.IsValid)
            {
                OperationResult<CashAccount> getResult = await repository.GetCashAccountByIdAsync(model.CashAccountId);

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