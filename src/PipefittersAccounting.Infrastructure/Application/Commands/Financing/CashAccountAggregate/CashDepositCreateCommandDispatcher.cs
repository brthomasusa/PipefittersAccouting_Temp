using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing.CashAccountAggregate
{
    public class CashDepositCreateCommandDispatcher
    {
        public async static Task<OperationResult<bool>> Process
        (
            CashTransactionWriteModel model,
            ICashAccountAggregateRepository repository,
            ICashAccountAggregateValidationService validationService,
            IUnitOfWork unitOfWork
        )
            => (CashTransactionTypeEnum)model.TransactionType switch
            {
                CashTransactionTypeEnum.CashReceiptSales => throw new NotImplementedException(),

                CashTransactionTypeEnum.CashReceiptDebtIssueProceeds =>
                    await CreateCashDepositForDebtIssueProceedsCommand.Process(model, repository, validationService, unitOfWork),

                CashTransactionTypeEnum.CashReceiptStockIssueProceeds =>
                    await CreateCashDepositForStockIssueProceedsCommand.Process(model, repository, validationService, unitOfWork),

                _ => OperationResult<bool>.CreateFailure($"Unexpected deposit transaction type: {(CashTransactionTypeEnum)model.TransactionType}"),
            };
    }
}
