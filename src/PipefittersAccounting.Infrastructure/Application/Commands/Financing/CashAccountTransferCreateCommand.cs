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
    public class CashAccountTransferCreateCommand
    {
        public static async Task<OperationResult<bool>> Process
        (
            CashAccountTransferWriteModel model,
            ICashAccountAggregateRepository repository,
            ICashAccountAggregateValidationService validationService,
            IUnitOfWork uow
        )
        {
            ValidationResult validationResult = await validationService.IsValidCreateCashAccountTransferInfo(model);

            if (validationResult.IsValid)
            {
                // Create CashTransfer
                CashTransfer xfer = new
                (
                    EntityGuidID.Create(Guid.NewGuid()),
                    EntityGuidID.Create(model.SourceCashAccountId),
                    EntityGuidID.Create(model.DestinationCashAccountId),
                    CashTransactionAmount.Create(model.CashTransferAmount),
                    CashTransactionDate.Create(model.CashTransferDate),
                    EntityGuidID.Create(model.UserId)
                );

                OperationResult<bool> addTranferResult = await repository.AddCashTransferAsync(xfer);

                if (addTranferResult.Success)
                {
                    // Retrieve source cash account and call its TransferCashOutOfAccount method
                    OperationResult<CashAccount> sourceAcctResult = await repository.GetCashAccountByIdAsync(model.SourceCashAccountId);
                    if (sourceAcctResult.Success)
                    {
                        // Retrieve destination cash account and call its TransferCashIntoAccount method
                        OperationResult<CashAccount> destinationAcctResult = await repository.GetCashAccountByIdAsync(model.DestinationCashAccountId);
                        if (destinationAcctResult.Success)
                        {
                            // Four database operations need to be performed
                            // in order. Do first two and commit and then second
                            // two and commit. If there is failure, rollback everything.
                            async Task<OperationResult<bool>> WriteToDb()
                            {
                                await uow.Commit();
                                sourceAcctResult.Result.TransferCashOutOfAccount(xfer);
                                destinationAcctResult.Result.TransferCashIntoAccount(xfer);

                                await uow.Commit();

                                return OperationResult<bool>.CreateSuccessResult(true);
                            }

                            return await repository.ExecuteInATransaction(WriteToDb);
                        }
                        else
                        {
                            return OperationResult<bool>.CreateFailure(destinationAcctResult.NonSuccessMessage);
                        }
                    }
                    else
                    {
                        return OperationResult<bool>.CreateFailure(sourceAcctResult.NonSuccessMessage);
                    }
                }
                else
                {
                    return OperationResult<bool>.CreateFailure(addTranferResult.NonSuccessMessage);
                }
            }
            else
            {
                return OperationResult<bool>.CreateFailure(validationResult.Messages[0]);
            }
        }
    }
}
