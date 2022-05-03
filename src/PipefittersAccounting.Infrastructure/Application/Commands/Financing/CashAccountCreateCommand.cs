#pragma warning disable CS8604

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing
{
    public class CashAccountCreateCommand : IWriteModelProcessor<CreateCashAccountInfo, ICashAccountAggregateRepository>
    {
        private readonly ICashTransactionValidationService _validationService;

        public CashAccountCreateCommand(ICashTransactionValidationService validationService)
            => _validationService = validationService;

        public async Task<OperationResult<bool>> Process
        (
            CreateCashAccountInfo model,
            ICashAccountAggregateRepository repository,
            IUnitOfWork uow
        )
        {
            try
            {
                OperationResult<bool> result = await repository.DoesCashAccountExist(model.CashAccountId);

                if (!result.Success)
                {
                    string errMsg = $"Create operation failed! A cash account with this Id: {model.CashAccountId} already exists!";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                OperationResult<Guid> dupeNameResult = await repository.CheckForDuplicateAccountName(model.CashAccountName);
                if (dupeNameResult.Result != Guid.Empty)
                {
                    string errMsg = $"A cash account with account name: {model.CashAccountName} is already in the database.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                OperationResult<Guid> dupeAcctNumberResult = await repository.CheckForDuplicateAccountNumber(model.CashAccountNumber);
                if (dupeAcctNumberResult.Result != Guid.Empty)
                {
                    string msg = $"A cash account with account number: {model.CashAccountNumber} is already in the database.";
                    return OperationResult<bool>.CreateFailure(msg);
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
                    EntityGuidID.Create(model.UserId),
                    _validationService
                );

                await repository.AddCashAccountAsync(cashAccount);
                await uow.Commit();

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }
    }
}