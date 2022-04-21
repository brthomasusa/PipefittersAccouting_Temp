using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing
{
    public class CashTransactionValidationService : ICashTransactionValidationService
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public CashTransactionValidationService(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public virtual async Task<ValidationResult> IsValid(CashTransaction cashTransaction)
        {
            ValidationResult result = new();

            switch (cashTransaction.CashTransactionType)
            {
                case CashTransactionTypeEnum.CashReceiptDebtIssueProceeds:
                    result = await CashReceiptForDebtIssueValidator.Validate(cashTransaction, _cashAcctQrySvc);
                    break;

                case CashTransactionTypeEnum.CashDisbursementLoanPayment:
                    result = await LoanInstallmentPaymentValidator.Validate(cashTransaction, _cashAcctQrySvc);
                    break;
            }

            return result;
        }

        public virtual Task<ValidationResult> IsValidCashDisbursement
        (
            CashTransactionTypeEnum transactionType,
            EconomicEvent goodsOrServiceReceived,
            ExternalAgent soldBy,
            CashTransactionAmount transactionAmount
        )
        {
            throw new NotImplementedException();
        }

        public virtual Task<ValidationResult> IsValidCashDeposit
        (
            CashTransactionTypeEnum transactionType,
            EconomicEvent goodsOrServiceProvided,
            ExternalAgent purchasedBy,
            CashTransactionAmount transactionAmount
        )
        {
            // transactionType switch
            // {
            //     CashTransactionTypeEnum.CashReceiptDebtIssueProceeds 
            //         => await CashReceiptForDebtIssueValidator.Validate(goodsOrServiceProvided,),
            //     _ => throw new ArgumentOutOfRangeException($"Not expected direction value: {transactionType}"),
            // };

            throw new NotImplementedException();
        }
    }
}