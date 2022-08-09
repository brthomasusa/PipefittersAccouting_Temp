#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.CashManagement;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.CashManagement.BusinessRules
{
    // Verify that a financier and a loan agreement are known to the system  
    // and that the financier is associated with this particular loan agreement.

    public class VerifyCreditorHasLoanAgreementRule : BusinessRule<CashTransactionWriteModel>
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;

        public VerifyCreditorHasLoanAgreementRule(ICashAccountQueryService cashAcctQrySvc)
            => _cashAcctQrySvc = cashAcctQrySvc;

        public override async Task<ValidationResult> Validate(CashTransactionWriteModel transaction)
        {
            ValidationResult validationResult = new();

            CreditorLoanAgreementValidationParameters queryParameters = new() { LoanId = transaction.EventId, FinancierId = transaction.AgentId };

            OperationResult<CreditorIssuedLoanAgreementValidationInfo> result =
                await _cashAcctQrySvc.GetCreditorIssuedLoanAgreementValidationInfo(queryParameters);

            if (result.Success)
            {
                validationResult.IsValid = true;

                if (Next is not null)
                {
                    validationResult = await Next?.Validate(transaction);
                }
            }
            else
            // The loan id and financier id have been validated.
            // So, to be here means the loan ageement was not issued
            // by this financier.
            {
                validationResult.Messages.Add(result.NonSuccessMessage);
            }

            return validationResult; ;
        }
    }
}