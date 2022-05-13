using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing;
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

        public virtual async Task<ValidationResult> IsValidCashDisbursement(CreateCashAccountTransactionInfo transactionInfo)
        {
            CashTransactionTypeEnum disbursementType = (CashTransactionTypeEnum)transactionInfo.TransactionType;
            ValidationResult result = new();

            return result = disbursementType switch
            {
                CashTransactionTypeEnum.CashDisbursementLoanPayment
                    => await LoanInstallmentPaymentValidation.Validate(transactionInfo, _cashAcctQrySvc),

                _ => throw new ArgumentException($"Unrecognized cash disbursement type: {disbursementType}."),
            };
        }

        public virtual async Task<ValidationResult> IsValidCashDeposit(CreateCashAccountTransactionInfo transactionInfo)
        {
            CashTransactionTypeEnum depositType = (CashTransactionTypeEnum)transactionInfo.TransactionType;
            ValidationResult result = new();

            return result = depositType switch
            {
                CashTransactionTypeEnum.CashReceiptDebtIssueProceeds
                    => await CashReceiptForDebtIssueValidator.Validate(transactionInfo, _cashAcctQrySvc),

                CashTransactionTypeEnum.CashReceiptStockIssueProceeds
                    => await CashReceiptForDebtIssueValidator.Validate(transactionInfo, _cashAcctQrySvc),

                // CashTransactionTypeEnum.CashReceiptSales => ,

                _ => throw new ArgumentException($"Unrecognized cash deposit type: {depositType}."),
            };
        }

        public virtual Task<ValidationResult> IsValidCashAccount(CashAccount cashAccount)
            => throw new NotImplementedException();

        public virtual async Task<ValidationResult> IsValidCreateCashAccountInfo(CreateCashAccountInfo writeModel)
            => await CreateCashAccountInfoValidation.Validate(writeModel, _cashAcctQrySvc);

        public virtual async Task<ValidationResult> IsValidEditCashAccountInfo(EditCashAccountInfo writeModel)
            => await EditCashAccountInfoValidation.Validate(writeModel, _cashAcctQrySvc);

        public virtual async Task<ValidationResult> IsValidDeleteCashAccountInfo(DeleteCashAccountInfo writeModel)
            => await DeleteCashAccountInfoValidation.Validate(writeModel, _cashAcctQrySvc);

    }
}