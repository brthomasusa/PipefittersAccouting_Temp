#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules
{
    public class VerifyDetailsOfDepositOfDebtIssueProceedsRule : BusinessRule<CashTransactionWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public VerifyDetailsOfDepositOfDebtIssueProceedsRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashTransactionWriteModel transaction)
        {
            ValidationResult validationResult = new();

            CreditorLoanAgreementValidationParameters queryParameters = new() { FinancierId = transaction.AgentId, LoanId = transaction.EventId };

            OperationResult<CashReceiptOfDebtIssueProceedsInfo> miscDetailsResult =
                await _cashAcctQrySvc.GetCashReceiptOfDebtIssueProceedsInfo(queryParameters);

            if (miscDetailsResult.Success)
            {
                // Check that transaction date is between loan date and maturity date
                if (transaction.TransactionDate < miscDetailsResult.Result.LoanDate ||
                    transaction.TransactionDate > miscDetailsResult.Result.MaturityDate)
                {
                    string msg = $"The transaction '{transaction.TransactionDate}' is outside the range of the loan date ({miscDetailsResult.Result.LoanDate}) and maturity date({miscDetailsResult.Result.MaturityDate})!";
                    validationResult.Messages.Add(msg);
                }
                else if (transaction.TransactionAmount != miscDetailsResult.Result.LoanAmount) // Check that transaction amount equals loan amount
                {
                    string msg = $"The deposit amount ({transaction.TransactionAmount}) does not match the loan agreement amount ({miscDetailsResult.Result.LoanAmount})!";
                    validationResult.Messages.Add(msg);
                }
                else if (miscDetailsResult.Result.AmountReceived > 0 && miscDetailsResult.Result.DateReceived is not null) // Verify that deposit of debt issue proceeds for this loan agreement has already been made
                {
                    string msg = $"Duplicate transaction!! A deposit of {miscDetailsResult.Result.AmountReceived} was made on {miscDetailsResult.Result.DateReceived}.";
                    validationResult.Messages.Add(msg);
                }
                else
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(transaction);
                    }
                }
            }
            else
            {
                validationResult.Messages.Add(miscDetailsResult.NonSuccessMessage);
            }

            return validationResult;
        }
    }
}