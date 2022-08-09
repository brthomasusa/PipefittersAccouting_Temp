#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.LoanAgreementAggregate.BusinessRules
{
    public class VerifyCreditorIsLinkedToLoanAgreementRule : BusinessRule<LoanAgreementWriteModel>
    {
        private readonly ILoanAgreementQueryService _qrySvc;

        public VerifyCreditorIsLinkedToLoanAgreementRule(ILoanAgreementQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(LoanAgreementWriteModel loanAgreement)
        {
            ValidationResult validationResult = new();

            ReceiptLoanProceedsValidationParams qryParam =
                new ReceiptLoanProceedsValidationParams() { FinancierId = loanAgreement.FinancierId, LoanId = loanAgreement.LoanId };

            OperationResult<Guid> agentResult =
                await _qrySvc.VerifyCreditorIsLinkedToLoanAgreement(qryParam);

            // The agentId and eventId have been validated, so is the combonation valid?
            if (agentResult.Success)
            {
                // Guid.Empty means the creditor is not associated with this ageement.
                if (agentResult.Result != Guid.Empty)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(loanAgreement);
                    }
                }
                else
                {
                    string msg = $"Creditor '{loanAgreement.FinancierId}' is not associated with loan agreement '{loanAgreement.LoanId}'!";
                    validationResult.Messages.Add(msg);
                }
            }
            else
            {
                validationResult.Messages.Add(agentResult.NonSuccessMessage);
            }

            return validationResult;
        }
    }
}