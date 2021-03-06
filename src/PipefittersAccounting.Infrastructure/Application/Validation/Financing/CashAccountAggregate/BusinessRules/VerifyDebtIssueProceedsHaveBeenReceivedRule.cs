#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate.BusinessRules
{
    public class VerifyDebtIssueProceedsHaveBeenReceivedRule : BusinessRule<CashTransactionWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public VerifyDebtIssueProceedsHaveBeenReceivedRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashTransactionWriteModel transaction)
        {
            ValidationResult validationResult = new();

            GetLoanInstallmentInfoParameters parameters = new() { LoanInstallmentId = transaction.EventId };
            OperationResult<CreditorIsOwedThisLoanInstallmentValidationInfo> eventResult =
                await _cashAcctQrySvc.GetCreditorIsOwedThisLoanInstallmentValidationInfo(parameters);

            if (eventResult.Success)
            {
                CreditorLoanAgreementValidationParameters queryParameters =
                    new() { FinancierId = transaction.AgentId, LoanId = eventResult.Result.LoanId };

                OperationResult<CashReceiptOfDebtIssueProceedsInfo> miscDetailsResult =
                    await _cashAcctQrySvc.GetCashReceiptOfDebtIssueProceedsInfo(queryParameters);

                if (miscDetailsResult.Success)
                {
                    // Before making a loan payment, ensure that debt issue proceeds have been received
                    if (miscDetailsResult.Result.AmountReceived > 0)
                    {
                        validationResult.IsValid = true;

                        if (Next is not null)
                        {
                            validationResult = await Next?.Validate(transaction);
                        }
                    }
                    else
                    {
                        string msg = $"Unable to complete disbursement for loan payment, the debt issue proceeds have not yet been received.";
                        validationResult.Messages.Add(msg);
                    }
                }
                else
                {
                    validationResult.Messages.Add(miscDetailsResult.NonSuccessMessage);
                }
            }
            else
            {
                validationResult.Messages.Add(eventResult.NonSuccessMessage);
            }

            return validationResult;
        }
    }
}