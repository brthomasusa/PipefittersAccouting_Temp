using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
    public class CashTransactionValidationService : ICashTransactionValidationService
    {
        private ICashReceiptForDebtIssueValidator _cashReceiptForDebtIssueValidator;
        private ILoanInstallmentPaymentValidator _loanInstallmentPaymentValidator;

        public CashTransactionValidationService
        (
            ICashReceiptForDebtIssueValidator cashReceiptForDebtIssueValidator,
            ILoanInstallmentPaymentValidator loanInstallmentPaymentValidator
        )
        {
            _cashReceiptForDebtIssueValidator = cashReceiptForDebtIssueValidator;
            _loanInstallmentPaymentValidator = loanInstallmentPaymentValidator;
        }


    }
}