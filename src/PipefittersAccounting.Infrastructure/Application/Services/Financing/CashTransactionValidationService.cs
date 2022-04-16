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

        public async Task<ValidationResult> IsValid(CashTransaction cashTransaction)
        {
            ValidationResult result = new();

            switch (cashTransaction.CashTransactionType)
            {
                case CashTransactionTypeEnum.CashReceiptDebtIssueProceeds:
                    result = await CashReceiptForDebtIssueValidator.Validate(cashTransaction, _cashAcctQrySvc);
                    break;

                case CashTransactionTypeEnum.CashDisbursementLoanPayment:
                    // result = LoanInstallmentPaymentValidator.Validate(cashTransaction, _cashAcctQrySvc);
                    break;
            }

            return result;
        }
    }
}