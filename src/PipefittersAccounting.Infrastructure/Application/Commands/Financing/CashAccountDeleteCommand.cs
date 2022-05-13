#pragma warning disable CS8604

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing
{
    public class CashAccountDeleteCommand
    {
        public static async Task<OperationResult<bool>> Process
        (
            DeleteCashAccountInfo model,
            ICashAccountAggregateRepository repository,
            ICashAccountAggregateValidationService validationService,
            IUnitOfWork uow
        )
        {
            OperationResult<bool> result = await repository.DoesCashAccountExist(model.CashAccountId);

            if (!result.Success)
            {
                string errMsg = $"Delete cash account failed! A cash account with Id '{model.CashAccountId}' could not be located!";
                return OperationResult<bool>.CreateFailure(errMsg);
            }

            ValidationResult validationResult = await validationService.IsValidDeleteCashAccountInfo(model);

            if (validationResult.IsValid)
            {
                OperationResult<CashAccount> getResult = await repository.GetCashAccountByIdAsync(model.CashAccountId);

                if (getResult.Success)
                {
                    CashAccount cashAccount = getResult.Result;
                    OperationResult<bool> deleteResult = await repository.DeleteCashAccountAsync(model.CashAccountId);

                    if (deleteResult.Success)
                    {
                        await uow.Commit();
                        return OperationResult<bool>.CreateSuccessResult(true);
                    }
                    else
                    {
                        return OperationResult<bool>.CreateFailure(deleteResult.NonSuccessMessage);
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

