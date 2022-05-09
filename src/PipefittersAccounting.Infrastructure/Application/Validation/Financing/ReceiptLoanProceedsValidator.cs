#pragma warning disable CS8602

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing
{
    public class ReceiptLoanProceedsValidator : ICashTransactionValidator
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public ReceiptLoanProceedsValidator(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public ICashTransactionValidator? Next { get; set; }

        public async Task<ValidationResult> Validate(CashAccountTransaction deposit)
        {
            ValidationResult validationResult = new();

            ReceiptLoanProceedsValidationParams loanAmountParam =
                new() { FinancierId = (deposit as CashDeposit).Payor.Id, LoanId = (deposit as CashDeposit).GoodsOrServiceSold.Id };

            OperationResult<DepositLoanProceedsValidationModel> loanAmountResult =
                await _cashAcctQrySvc.GetReceiptLoanProceedsValidationModel(loanAmountParam);

            if (loanAmountResult.Success)
            {
                if (loanAmountResult.Result.LoanAmount == deposit.TransactionAmount)
                {
                    if (loanAmountResult.Result.AmountReceived == 0)
                    {
                        validationResult.IsValid = true;

                        if (Next is not null)
                        {
                            validationResult = await Next?.Validate(deposit);
                        }
                    }
                    else
                    {
                        var amtRcvd = loanAmountResult.Result.AmountReceived;
                        var dateRcvd = loanAmountResult.Result.DateReceived;
                        string msg = $"The loan proceeds have already been deposited. ${amtRcvd} was deposited on: {dateRcvd}.";

                        validationResult.IsValid = false;
                        validationResult.Messages.Add(msg);
                    }
                }
                else
                {
                    validationResult.IsValid = false;
                    validationResult.Messages.Add("The loan proceed amount and the loan agreement amount do not match.");
                }
            }
            else
            {
                validationResult.IsValid = false;
                validationResult.Messages.Add(loanAmountResult.NonSuccessMessage);
            }

            return validationResult; ;
        }
    }
}