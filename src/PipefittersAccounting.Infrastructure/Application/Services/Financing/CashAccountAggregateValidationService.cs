using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
    public class CashAccountAggregateValidationService : ICashAccountAggregateValidationService
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public CashAccountAggregateValidationService(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;


        public virtual Task<ValidationResult> IsValidCashAccount(CashAccount cashAccount)
            => throw new NotImplementedException();

        public virtual async Task<ValidationResult> IsValidCreateCashAccountInfo(CreateCashAccountInfo writeModel)
            => await CreateCashAccountInfoValidation.Validate(writeModel, _cashAcctQrySvc);

        public virtual async Task<ValidationResult> IsValidEditCashAccountInfo(EditCashAccountInfo writeModel)
            => await EditCashAccountInfoValidation.Validate(writeModel, _cashAcctQrySvc);

        public virtual async Task<ValidationResult> IsValidDeleteCashAccountInfo(DeleteCashAccountInfo writeModel)
            => await DeleteCashAccountInfoValidation.Validate(writeModel, _cashAcctQrySvc);

        public virtual Task<ValidationResult> IsValidCashDepositOfSalesProceeds(CreateCashAccountTransactionInfo transactionInfo)
            => throw new NotImplementedException();

        public virtual async Task<ValidationResult> IsValidCashDepositOfDebtIssueProceeds(CreateCashAccountTransactionInfo transactionInfo)
            => await ValidateDepositOfDebtIssueProceeds(transactionInfo);


        public virtual Task<ValidationResult> IsValidCashDepositOfStockIssueProceeds(CreateCashAccountTransactionInfo transactionInfo)
        => throw new NotImplementedException();

        private async Task<ValidationResult> ValidateDepositOfDebtIssueProceeds(CreateCashAccountTransactionInfo model)
        {
            DepositOfDebtIssueProceedsValidation depositValidation = new(model, _cashAcctQrySvc);
            return await depositValidation.Validate();
        }
    }
}