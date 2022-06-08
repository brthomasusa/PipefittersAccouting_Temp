using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedKernel.Utilities;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing.CashAccountAggregate
{
    public class CashDisbursementCreateCommandDispatcher
    {
        public static async Task<OperationResult<bool>> Process
        (
            CashTransactionWriteModel model,
            ICashAccountAggregateRepository repository,
            ICashAccountAggregateValidationService validationService,
            IUnitOfWork unitOfWork
        )
            => (CashTransactionTypeEnum)model.TransactionType switch
            {
                CashTransactionTypeEnum.CashDisbursementDividentPayment
                    => await CreateCashDisbursementForDividendPaymentCommand.Process(model, repository, validationService, unitOfWork),

                CashTransactionTypeEnum.CashDisbursementLoanPayment
                    => await CreateCashDisbursementForLoanPaymentCommand.Process(model, repository, validationService, unitOfWork),

                CashTransactionTypeEnum.CashDisbursementPurchaseReceipt => throw new NotImplementedException(),

                CashTransactionTypeEnum.CashDisbursementTimeCardPayment => throw new NotImplementedException(),

                _ => OperationResult<bool>.CreateFailure($"Unexpected deposit transaction type: {(CashTransactionTypeEnum)model.TransactionType}"),
            };
    }
}