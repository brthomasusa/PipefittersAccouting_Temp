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

        public virtual async Task<ValidationResult> IsValidCreateCashAccountInfo(CashAccountWriteModel writeModel)
            => await CreateCashAccountInfoValidation.Validate(writeModel, _cashAcctQrySvc);

        public virtual async Task<ValidationResult> IsValidEditCashAccountInfo(CashAccountWriteModel writeModel)
            => await EditCashAccountInfoValidation.Validate(writeModel, _cashAcctQrySvc);

        public virtual async Task<ValidationResult> IsValidDeleteCashAccountInfo(CashAccountWriteModel writeModel)
            => await DeleteCashAccountInfoValidation.Validate(writeModel, _cashAcctQrySvc);

        public virtual Task<ValidationResult> IsValidCashDepositOfSalesProceeds(CashTransactionWriteModel transactionInfo)
            => throw new NotImplementedException();

        public virtual async Task<ValidationResult> IsValidCashDepositOfDebtIssueProceeds(CashTransactionWriteModel transactionInfo)
            => await ValidateDepositOfDebtIssueProceeds(transactionInfo);

        public virtual Task<ValidationResult> IsValidCashDepositOfStockIssueProceeds(CashTransactionWriteModel transactionInfo)
        => throw new NotImplementedException();

        public virtual async Task<ValidationResult> IsValidCashDisbursementForLoanPayment(CashTransactionWriteModel transactionInfo)
            => await ValidateDisbursementForLoanPayment(transactionInfo);

        public async Task<ValidationResult> IsValidCreateCashAccountTransferInfo(CashAccountTransferWriteModel transferInfo)
            => await CashAccountTransferValidation.Validate(transferInfo, _cashAcctQrySvc);






        // private stuff
        private async Task<ValidationResult> ValidateDepositOfDebtIssueProceeds(CashTransactionWriteModel model)
        {
            DepositOfDebtIssueProceedsValidation validation = new(model, _cashAcctQrySvc);
            return await validation.Validate();
        }

        private async Task<ValidationResult> ValidateDisbursementForLoanPayment(CashTransactionWriteModel model)
        {
            DisbursementForLoanPaymentValidation validation = new(model, _cashAcctQrySvc);
            return await validation.Validate();
        }

    }
}