using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
    public class CashTransactionValidationService : ICashTransactionValidationService
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public CashTransactionValidationService(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public virtual async Task<ValidationResult> IsValidCashDisbursement(CashDisbursement disbursement)
        {
            CashTransactionTypeEnum disbursementType = disbursement.DisbursementType;
            ValidationResult result = new();

            return result = disbursementType switch
            {
                CashTransactionTypeEnum.CashDisbursementLoanPayment
                    => await LoanInstallmentPaymentValidator.Validate(disbursement, _cashAcctQrySvc),

                _ => throw new ArgumentException($"Unrecognized cash disbursement type: {disbursementType}."),
            };
        }

        public virtual async Task<ValidationResult> IsValidCashDeposit(CashDeposit deposit)
        {
            CashTransactionTypeEnum depositType = deposit.DepositType;
            ValidationResult result = new();

            return result = depositType switch
            {
                CashTransactionTypeEnum.CashReceiptDebtIssueProceeds
                    => await CashReceiptForDebtIssueValidator.Validate(deposit, _cashAcctQrySvc),

                CashTransactionTypeEnum.CashReceiptStockIssueProceeds
                    => await CashReceiptForDebtIssueValidator.Validate(deposit, _cashAcctQrySvc),

                // CashTransactionTypeEnum.CashReceiptSales => ,

                _ => throw new ArgumentException($"Unrecognized cash deposit type: {depositType}."),
            };
        }
    }
}