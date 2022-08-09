#pragma warning disable CS8604

using PipefittersAccounting.Core.CashManagement.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.CashManagement;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing.CashAccountAggregate
{
    public class CashAccountDeleteCommand
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
                string errMsg = $"Delete cash account failed! A cash account with Id '{model.CashAccountId}' could not be located!";
                return OperationResult<bool>.CreateFailure(errMsg);
            }

            ValidationResult validationResult = await validationService.IsValidDeleteCashAccountInfo(model);

            if (validationResult.IsValid)
            {
                OperationResult<CashAccount> getResult = await repository.GetByIdAsync(model.CashAccountId);

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

