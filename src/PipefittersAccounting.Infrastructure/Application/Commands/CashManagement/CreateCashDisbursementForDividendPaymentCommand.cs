#pragma warning disable CS8604

using PipefittersAccounting.Core.CashManagement.CashAccountAggregate;
using PipefittersAccounting.Core.CashManagement.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.CashManagement;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Infrastructure.Application.Commands.CashManagement
{
    public class CreateCashDisbursementForDividendPaymentCommand
    {
        public static async Task<OperationResult<bool>> Process
        (
            CashTransactionWriteModel model,
            ICashAccountAggregateRepository repository,
            ICashAccountAggregateValidationService validationService,
            IUnitOfWork uow
        )
        {
            OperationResult<CashAccount> getCashAcct = await repository.GetByIdAsync(model.CashAccountId);

            if (getCashAcct.Success)
            {
                CashAccount cashAccount = getCashAcct.Result;

                if (cashAccount.CashAccountType == CashAccountTypeEnum.NonPayrollOperations)
                {
                    ValidationResult validationResult = await validationService.IsValidCashDisbursementForDividendPayment(model);

                    if (validationResult.IsValid)
                    {
                        CashTransaction transaction = new
                        (
                            (CashTransactionTypeEnum)model.TransactionType,
                            EntityGuidID.Create(model.CashAccountId),
                            CashTransactionDate.Create(model.TransactionDate),
                            CashTransactionAmount.Create(model.TransactionAmount),
                            EntityGuidID.Create(model.AgentId),
                            EntityGuidID.Create(model.EventId),
                            CheckNumber.Create(model.CheckNumber),
                            RemittanceAdvice.Create(model.RemittanceAdvice),
                            EntityGuidID.Create(model.UserId)
                        );

                        cashAccount.DisburseCash(transaction);
                        await repository.UpdateCashAccountAsync(cashAccount);
                        await uow.Commit();
                        model.CashTransactionId = transaction.Id;
                    }
                    else
                    {
                        return OperationResult<bool>.CreateFailure(validationResult.Messages[0]);
                    }
                }
                else
                {
                    string errMsg = $"Create disbursement failed! Disbursement for a dividende payment can only be made from a primary checking account!";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }
            }
            else
            {
                string errMsg = $"Create operation failed! Could not locate a cash account with Id '{model.CashAccountId}'!";
                return OperationResult<bool>.CreateFailure(errMsg);
            }

            return OperationResult<bool>.CreateSuccessResult(true);
        }
    }
}