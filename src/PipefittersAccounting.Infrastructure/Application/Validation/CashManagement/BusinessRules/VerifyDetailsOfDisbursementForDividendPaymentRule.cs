#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules
{
    public class VerifyDetailsOfDisbursementForDividendPaymentRule : BusinessRule<CashTransactionWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public VerifyDetailsOfDisbursementForDividendPaymentRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashTransactionWriteModel transaction)
        {
            ValidationResult validationResult = new();

            GetDividendDeclarationParameter queryParameters = new() { DividendId = transaction.EventId };

            OperationResult<DividendDeclarationDetails> detailsResult =
                await _cashAcctQrySvc.GetDividendDeclarationDetails(queryParameters);

            if (detailsResult.Success)
            {
                if (transaction.TransactionDate >= detailsResult.Result.DividendDeclarationDate)
                {
                    decimal dividendPayable = (detailsResult.Result.SharesIssured * detailsResult.Result.DividendPerShare);

                    if (transaction.TransactionAmount == dividendPayable)
                    {
                        if (detailsResult.Result.AmountPaid == 0)
                        {
                            validationResult.IsValid = true;

                            if (Next is not null)
                            {
                                validationResult = await Next?.Validate(transaction);
                            }
                        }
                        else
                        {
                            string msg = $"Duplicate transaction!! A disbursement of ${detailsResult.Result.AmountPaid} was made on {detailsResult.Result.DatePaid}.";
                            validationResult.Messages.Add(msg);
                        }
                    }
                    else
                    {
                        string msg = $"The disbursement amount (${transaction.TransactionAmount}) must equal shares issued ({detailsResult.Result.SharesIssured}) * dividend per share (${dividendPayable})!";
                        validationResult.Messages.Add(msg);
                    }
                }
                else
                {
                    string msg = $"The transaction date '{transaction.TransactionDate}' cannot be before the dividend declaration date ({detailsResult.Result.DividendDeclarationDate})!";
                    validationResult.Messages.Add(msg);
                }
            }
            else
            {
                validationResult.Messages.Add(detailsResult.NonSuccessMessage);
            }

            return validationResult;
        }
    }
}