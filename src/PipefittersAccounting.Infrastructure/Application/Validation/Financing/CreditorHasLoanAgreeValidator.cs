#pragma warning disable CS8602

using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing
{
    public class CreditorHasLoanAgreeValidator : ICashTransactionValidator
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public CreditorHasLoanAgreeValidator(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public ICashTransactionValidator? Next { get; set; }

        public async Task<ValidationResult> Validate(CashTransaction transaction)
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