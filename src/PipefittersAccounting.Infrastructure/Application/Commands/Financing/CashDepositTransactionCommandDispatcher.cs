using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing
{
    public class CashDepositTransactionCommandDispatcher : BaseCashTransactionCommandDispatcher
    {
        public CashDepositTransactionCommandDispatcher
        (
            CreateCashAccountTransactionInfo model,
            ICashAccountAggregateRepository repository,
            ICashAccountAggregateValidationService validationService,
            IUnitOfWork unitOfWork
        )
            : base(model, repository, validationService, unitOfWork)
        {

        }

        public override async Task<OperationResult<bool>> Dispatch()
            => (CashTransactionTypeEnum)Model.TransactionType switch
            {
                CashTransactionTypeEnum.CashReceiptSales => throw new NotImplementedException(),

                CashTransactionTypeEnum.CashReceiptDebtIssueProceeds =>
                    await CreateCashDepositForDebtIssueProceedsCommand.Process(Model, Repository, ValidationService, UnitOfWork),

                CashTransactionTypeEnum.CashReceiptStockIssueProceeds => throw new NotImplementedException(),

                _ => OperationResult<bool>.CreateFailure($"Unexpected deposit transaction type: {(CashTransactionTypeEnum)Model.TransactionType}"),
            };
    }
}