#pragma warning disable CS8602

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
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

        public async Task<ValidationResult> Validate(CashTransaction transaction)
        {
            ValidationResult validationResult = new();

            ReceiptLoanProceedsValidationParams loanAmountParam =
                new() { FinancierId = transaction.AgentId, LoanId = transaction.EventId };

            OperationResult<ReceiptLoanProceedsValidationModel> loanAmountResult =
                await _cashAcctQrySvc.GetReceiptLoanProceedsValidationModel(loanAmountParam);

            if (loanAmountResult.Success)
            {
                if (loanAmountResult.Result.LoanAmount == transaction.CashTransactionAmount)
                {
                    if (loanAmountResult.Result.AmountReceived == 0)
                    {
                        validationResult.IsValid = true;

                        if (Next is not null)
                        {
                            validationResult = await Next?.Validate(transaction);
                        }
                    }
                    else
                    {
                        var amtRcvd = loanAmountResult.Result.AmountReceived;
                        var dateRcvd = loanAmountResult.Result.DateReceived;
                        string msg = $"The loan proceeds have already been deposited. Received on: {dateRcvd}, Amount deposited: {amtRcvd}.";

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