#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules
{
    public class VerifyDetailsOfDepositOfStockIssueProceedsRule : BusinessRule<CashTransactionWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public VerifyDetailsOfDepositOfStockIssueProceedsRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashTransactionWriteModel transaction)
        {
            ValidationResult validationResult = new();

            GetStockSubscriptionParameter queryParameters = new() { StockId = transaction.EventId };

            OperationResult<VerificationOfCashDepositStockIssueProceeds> miscDetailsResult =
                await _cashAcctQrySvc.VerifyCashDepositOfStockIssueProceeds(queryParameters);

            if (miscDetailsResult.Success)
            {
                if (transaction.TransactionDate >= miscDetailsResult.Result.StockIssueDate)
                {
                    decimal stockValue = (miscDetailsResult.Result.SharesIssured * miscDetailsResult.Result.PricePerShare);

                    if (transaction.TransactionAmount == stockValue)
                    {
                        if (miscDetailsResult.Result.AmountReceived == 0)
                        {
                            validationResult.IsValid = true;

                            if (Next is not null)
                            {
                                validationResult = await Next?.Validate(transaction);
                            }
                        }
                        else
                        {
                            string msg = $"Duplicate transaction!! A deposit of ${miscDetailsResult.Result.AmountReceived} was made on {miscDetailsResult.Result.DateReceived}.";
                            validationResult.Messages.Add(msg);
                        }
                    }
                    else
                    {
                        string msg = $"The deposit amount (${transaction.TransactionAmount}) must equal shares issued * price per share (${stockValue})!";
                        validationResult.Messages.Add(msg);
                    }
                }
                else
                {
                    string msg = $"The transaction date '{transaction.TransactionDate}' cannot be before the stock issue date ({miscDetailsResult.Result.StockIssueDate})!";
                    validationResult.Messages.Add(msg);
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