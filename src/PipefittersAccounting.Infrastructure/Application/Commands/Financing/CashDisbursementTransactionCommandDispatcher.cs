using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing
{
    public class CashDisbursementTransactionCommandDispatcher : BaseCashTransactionCommandDispatcher
    {
        public CashDisbursementTransactionCommandDispatcher
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
                CashTransactionTypeEnum.CashDisbursementDividentPayment => throw new NotImplementedException(),
                CashTransactionTypeEnum.CashDisbursementLoanPayment => throw new NotImplementedException(),
                CashTransactionTypeEnum.CashDisbursementPurchaseReceipt => throw new NotImplementedException(),
                CashTransactionTypeEnum.CashDisbursementTimeCardPayment => throw new NotImplementedException(),

                //TODO This is placeholder code, all transaction types need to be disbursement; no receipts!!
                CashTransactionTypeEnum.CashReceiptDebtIssueProceeds =>
                    await CreateCashDepositForDebtIssueProceedsCommand.Process(Model, Repository, ValidationService, UnitOfWork),

                _ => OperationResult<bool>.CreateFailure($"Unexpected deposit transaction type: {(CashTransactionTypeEnum)Model.TransactionType}"),
            };
    }
}