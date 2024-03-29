using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;

namespace PipefittersAccounting.Infrastructure.Interfaces.CashManagement
{
    public abstract class CashAccountTransactionValidatorBase
    {
        public CashAccountTransactionValidatorBase
        (
            CashTransactionWriteModel transactionInfo,
            ICashAccountQueryService queryService,
            ISharedQueryService sharedQueryService
        )
        {
            CashAccountTransactionInfo = transactionInfo;
            CashAccountQueryService = queryService;
            SharedQueryService = sharedQueryService;
        }

        protected CashTransactionWriteModel CashAccountTransactionInfo { get; init; }

        protected ICashAccountQueryService CashAccountQueryService { get; init; }

        protected ISharedQueryService SharedQueryService { get; init; }

        public abstract Task<ValidationResult> Validate();
    }
}