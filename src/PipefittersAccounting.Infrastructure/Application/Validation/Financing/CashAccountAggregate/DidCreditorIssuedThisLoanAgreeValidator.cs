#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate
{
    public class DidCreditorIssuedThisLoanAgreeValidator : Validator<CreateCashAccountTransactionInfo>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public DidCreditorIssuedThisLoanAgreeValidator(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CreateCashAccountTransactionInfo transaction)
        {
            ValidationResult validationResult = new();

            CreditorHasLoanAgreeValidationParams loanAgreementParam =
                new() { FinancierId = transaction.AgentId, LoanId = transaction.EventId };

            OperationResult<CreditorHasLoanAgreeValidationModel> loanAgreementResult =
                await _cashAcctQrySvc.GetCreditorHasLoanAgreeValidationModel(loanAgreementParam);

            if (loanAgreementResult.Success)
            {
                validationResult.IsValid = true;

                if (Next is not null)
                {
                    validationResult = await Next?.Validate(transaction);
                }
            }
            else
            {
                validationResult.IsValid = false;
                validationResult.Messages.Add(loanAgreementResult.NonSuccessMessage);
            }

            return validationResult; ;
        }
    }
}