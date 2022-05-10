#pragma warning disable CS8602

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class DisburesementForLoanPymtValidator : ICashTransactionValidator
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public DisburesementForLoanPymtValidator(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public ICashTransactionValidator? Next { get; set; }

        public async Task<ValidationResult> Validate(CashAccountTransaction deposit)
        {
            ValidationResult validationResult = new();

            DisburesementLoanPymtValidationParams queryParam =
                new() { LoanInstallmentId = (deposit as CashDeposit).GoodsOrServiceSold.Id };  // 

            OperationResult<DisburesementLoanPymtValidationModel> loanPymtResult =
                await _cashAcctQrySvc.GetDisburesementLoanPymtValidationModel(queryParam);

            if (loanPymtResult.Success)
            {
                // Check FinancierId
                if (loanPymtResult.Result.FinancierId == (deposit as CashDeposit).Payor.Id)
                {
                    // Check payment amount against transaction amount
                    if (loanPymtResult.Result.EqualMonthlyInstallment == deposit.TransactionAmount)
                    {
                        // Check that pymt has not already been paid
                        if (loanPymtResult.Result.AmountPaid == 0)
                        {
                            validationResult.IsValid = true;

                            if (Next is not null)
                            {
                                validationResult = await Next?.Validate(deposit);
                            }
                        }
                        else
                        {
                            decimal amtPaid = loanPymtResult.Result.AmountPaid;
                            DateTime datePaid = loanPymtResult.Result.DatePaid;
                            string msg = $"This installment has already been paid. ${amtPaid} was paid on {datePaid}.";

                            validationResult.IsValid = false;
                            validationResult.Messages.Add(msg);
                        }
                    }
                    else
                    {
                        decimal transAmt = deposit.TransactionAmount;
                        decimal pymtAmt = loanPymtResult.Result.EqualMonthlyInstallment;
                        string msg = $"The transaction amount {transAmt} does not match the installment amount {pymtAmt}.";

                        validationResult.IsValid = false;
                        validationResult.Messages.Add(msg);
                    }
                }
                else
                {
                    Guid eventId = (deposit as CashDeposit).GoodsOrServiceSold.Id;
                    Guid financierId = loanPymtResult.Result.FinancierId;
                    string msg = $"The financier in the transaction {eventId} does not match the financier in the loan agreement {financierId}.";

                    validationResult.IsValid = false;
                    validationResult.Messages.Add(msg);
                }
            }
            else
            {
                validationResult.IsValid = false;
                validationResult.Messages.Add(loanPymtResult.NonSuccessMessage);
            }

            return validationResult; ;
        }
    }
}