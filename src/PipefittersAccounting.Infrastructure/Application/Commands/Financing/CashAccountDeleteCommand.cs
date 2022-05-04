#pragma warning disable CS8604

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing
{
    public class CashAccountDeleteCommand : IWriteModelProcessor<DeleteCashAccountInfo, ICashAccountAggregateRepository>
    {
        private readonly ICashAccountAggregateValidationService _validationService;

        public CashAccountDeleteCommand(ICashAccountAggregateValidationService validationService)
            => _validationService = validationService;

        public async Task<OperationResult<bool>> Process
        (
            DeleteCashAccountInfo model,
            ICashAccountAggregateRepository repository,
            IUnitOfWork uow
        )
        {
            OperationResult<bool> result = await repository.DoesCashAccountExist(model.CashAccountId);

            if (!result.Success)
            {
                string errMsg = $"Delete cash account failed! A cash account with Id '{model.CashAccountId}' could not be located!";
                return OperationResult<bool>.CreateFailure(errMsg);
            }

            OperationResult<CashAccount> getResult = await repository.GetCashAccountByIdAsync(model.CashAccountId);

            if (getResult.Success)
            {
                CashAccount cashAccount = getResult.Result;
                await repository.DeleteCashAccountAsync(model.CashAccountId);
                await uow.Commit();

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            else
            {
                return OperationResult<bool>.CreateFailure(getResult.NonSuccessMessage);
            }
        }
    }
}