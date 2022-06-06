#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules
{
    public class VerifyLoanPymtDateAmountAndStatusRule : BusinessRule<CashTransactionWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public VerifyLoanPymtDateAmountAndStatusRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashTransactionWriteModel transactionInfo)
        {
            ValidationResult validationResult = new();

            GetLoanInstallmentInfoParameters queryParam =
                new() { LoanInstallmentId = transactionInfo.EventId };

            OperationResult<CashDisbursementForLoanInstallmentPaymentInfo> loanPymtResult =
                await _cashAcctQrySvc.GetCashDisbursementForLoanInstallmentPaymentInfo(queryParam);

            if (loanPymtResult.Success)
            {
                // Check that transaction date is between loan date and maturity date
                if (transactionInfo.TransactionDate >= loanPymtResult.Result.LoanDate &&
                    transactionInfo.TransactionDate <= loanPymtResult.Result.MaturityDate)
                {
                    // Check transaction amount against EMI amount
                    if (loanPymtResult.Result.EqualMonthlyInstallment == transactionInfo.TransactionAmount)
                    {
                        if (loanPymtResult.Result.AmountPaid == 0)
                        {
                            validationResult.IsValid = true;

                            if (Next is not null)
                            {
                                validationResult = await Next?.Validate(transactionInfo);
                            }
                        }
                        else
                        {
                            string msg = $"Duplicate payment! This loan installment payment was paid on {loanPymtResult.Result.DatePaid}.";
                            validationResult.Messages.Add(msg);
                        }
                    }
                    else
                    {
                        decimal transAmt = transactionInfo.TransactionAmount;
                        decimal pymtAmt = loanPymtResult.Result.EqualMonthlyInstallment;
                        string msg = $"Invalid disbursement amount ({transAmt})! The disbursement amount should be {pymtAmt} (Equal Monthly Installment).";

                        validationResult.Messages.Add(msg);
                    }
                }
                else
                {
                    string msg = $"Invalid transaction date; the transaction date must be between {loanPymtResult.Result.LoanDate} and {loanPymtResult.Result.MaturityDate}.";
                    validationResult.Messages.Add(msg);
                }
            }
            else
            {
                validationResult.Messages.Add(loanPymtResult.NonSuccessMessage);
            }

            return validationResult; ;
        }
    }
}
