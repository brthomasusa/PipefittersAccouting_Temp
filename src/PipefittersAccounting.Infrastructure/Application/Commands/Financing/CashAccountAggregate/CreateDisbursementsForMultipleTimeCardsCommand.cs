#pragma warning disable CS8604

using PipefittersAccounting.Infrastructure.Application.Services.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing.CashAccountAggregate
{
    public class CreateDisbursementsForMultipleTimeCardsCommand
        : WriteCommandHandler<CreatePayrollCashTransactions,
                              ICashAccountAggregateRepository,
                              ICashAccountAggregateValidationService,
                              CashAccount>
    {
        public CreateDisbursementsForMultipleTimeCardsCommand
        (
            CreatePayrollCashTransactions writeModelCollection,
            ICashAccountAggregateRepository repo,
            ICashAccountAggregateValidationService validationService,
            IUnitOfWork unitOfWork
        ) : base(writeModelCollection, repo, validationService, unitOfWork)
        {

        }

        protected async override Task<ValidationResult> Validate()
        {
            return await ValidationService.IsValidTimeCardPaymentInfo(WriteModel.CashTransactions);
        }

        protected override async Task<OperationResult<bool>> ProcessCommand()
        {
            try
            {
                OperationResult<CashAccount> result = await Repository.GetByIdAsync(WriteModel.CashAccountId);

                if (result.Success)
                {
                    CashAccount cashAccount = result.Result;
                    List<CashTransaction> transactions = new();
                    int checkNumber = 5015;

                    WriteModel.CashTransactions.ForEach(model =>
                    {
                        transactions.Add
                        (
                            new CashTransaction
                            (
                                (CashTransactionTypeEnum)model.TransactionType,
                                EntityGuidID.Create(model.CashAccountId),
                                CashTransactionDate.Create(model.TransactionDate),
                                CashTransactionAmount.Create(model.TransactionAmount),
                                EntityGuidID.Create(model.AgentId),
                                EntityGuidID.Create(model.EventId),
                                CheckNumber.Create(checkNumber++.ToString()),
                                RemittanceAdvice.Create(model.RemittanceAdvice),
                                EntityGuidID.Create(model.UserId)
                            )
                        );
                    });

                    cashAccount.DisburseCash(transactions);

                    OperationResult<bool> updateResult = await Repository.UpdateCashAccountAsync(cashAccount);

                    if (updateResult.Success)
                    {
                        return OperationResult<bool>.CreateSuccessResult(true);
                    }
                    else
                    {
                        return OperationResult<bool>.CreateFailure(updateResult.NonSuccessMessage);
                    }
                }
                else
                {
                    return OperationResult<bool>.CreateFailure(result.NonSuccessMessage);
                }

            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }
    }
}
