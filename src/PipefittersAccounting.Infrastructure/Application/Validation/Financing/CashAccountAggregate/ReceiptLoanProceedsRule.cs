#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class ReceiptLoanProceedsRule : BusinessRule<CreateCashAccountTransactionInfo>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public ReceiptLoanProceedsRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CreateCashAccountTransactionInfo transactionInfo)
        {
            ValidationResult validationResult = new();

            ReceiptLoanProceedsValidationParams loanAmountParam =
                new() { FinancierId = transactionInfo.AgentId, LoanId = transactionInfo.EventId };

            OperationResult<DepositLoanProceedsValidationModel> loanAmountResult =
                await _cashAcctQrySvc.GetReceiptLoanProceedsValidationModel(loanAmountParam);

            if (loanAmountResult.Success)
            {
                if (loanAmountResult.Result.LoanAmount == transactionInfo.TransactionAmount)
                {
                    if (loanAmountResult.Result.AmountReceived == 0)
                    {
                        validationResult.IsValid = true;

                        if (Next is not null)
                        {
                            validationResult = await Next?.Validate(transactionInfo);
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