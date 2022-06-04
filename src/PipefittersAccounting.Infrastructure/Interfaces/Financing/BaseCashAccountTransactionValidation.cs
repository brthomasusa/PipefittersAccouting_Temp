using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Interfaces.Financing
{
    public abstract class BaseCashAccountTransactionValidation
    {
        public BaseCashAccountTransactionValidation
        (
            CashTransactionWriteModel transactionInfo,
            ICashAccountQueryService queryService
        )
        {
            CashAccountTransactionInfo = transactionInfo;
            QueryService = queryService;
        }

        protected CashTransactionWriteModel CashAccountTransactionInfo { get; init; }

        protected ICashAccountQueryService QueryService { get; init; }

        public abstract Task<ValidationResult> Validate();
    }
}