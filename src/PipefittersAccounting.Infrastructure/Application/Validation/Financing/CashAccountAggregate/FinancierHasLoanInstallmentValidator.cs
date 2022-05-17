#pragma warning disable CS8602

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class FinancierHasLoanInstallmentValidator : Validator<CreateCashAccountTransactionInfo>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public FinancierHasLoanInstallmentValidator(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CreateCashAccountTransactionInfo transactionInfo)
        {
            ValidationResult validationResult = new();

            GetLoanInstallmentInfoParameters queryParameters = new() { LoanInstallmentId = transactionInfo.EventId };
            OperationResult<FinancierToLoanInstallmentValidationInfo> eventResult =
                await _cashAcctQrySvc.GetFinancierToLoanInstallmentValidationInfo(queryParameters);

            // Is the event id known to the system?
            if (eventResult.Success)
            {
                // Is the loan installment associated with the financier? Or, should
                // a payment be disbursed to this financier for this loan installment?
                if (eventResult.Result.FinancierId == transactionInfo.AgentId)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(transactionInfo);
                    }
                }
                else
                {
                    string msg = $"This financier '{transactionInfo.AgentId}' is not the correct payee for this loan installment!";
                    validationResult.Messages.Add(msg);
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