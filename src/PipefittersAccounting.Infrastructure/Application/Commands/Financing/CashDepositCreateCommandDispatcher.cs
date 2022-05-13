using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing
{
    public class CashDepositCreateCommandDispatcher
    {
        public async static Task<OperationResult<bool>> Dispatch
        (
            CreateCashAccountTransactionInfo model,
            ICashAccountAggregateRepository repository,
            ICashAccountAggregateValidationService validationService,
            IUnitOfWork unitOfWork
        )
        {
            OperationResult<bool> result = await repository.DoesCashAccountExist(model.CashAccountId);

            if (!result.Success)
            {
                string errMsg = $"Create operation failed! Could not locate a cash account with this Id '{model.CashAccountId}'!";
                return OperationResult<bool>.CreateFailure(errMsg);
            }

            CashTransactionTypeEnum transactionType = (CashTransactionTypeEnum)model.TransactionType;

            switch (transactionType)
            {
                case CashTransactionTypeEnum.CashReceiptSales:
                    throw new NotImplementedException();

                case CashTransactionTypeEnum.CashReceiptDebtIssueProceeds:
                    return await CreateCashDepositForDebtIssueProceedsCommand.Process(model, repository, validationService, unitOfWork);

                case CashTransactionTypeEnum.CashReceiptStockIssueProceeds:
                    throw new NotImplementedException();

                default:
                    string msg = $"'{transactionType}' is an invalid transaction type for a cash deposit transaction.";
                    return OperationResult<bool>.CreateFailure(msg);
            }
        }
    }
}
