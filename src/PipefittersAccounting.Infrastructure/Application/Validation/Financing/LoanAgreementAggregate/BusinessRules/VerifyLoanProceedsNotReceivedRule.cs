#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.LoanAgreementAggregate.BusinessRules
{
    public class VerifyLoanProceedsNotReceivedRule : BusinessRule<LoanAgreementWriteModel>
    {
        private readonly ILoanAgreementQueryService _qrySvc;

        public VerifyLoanProceedsNotReceivedRule(ILoanAgreementQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(LoanAgreementWriteModel loanAgreement)
        {
            ValidationResult validationResult = new();

            ReceiptLoanProceedsValidationParams qryParam =
                new ReceiptLoanProceedsValidationParams() { FinancierId = loanAgreement.FinancierId, LoanId = loanAgreement.LoanId };

            OperationResult<decimal> result = await _qrySvc.VerifyCashDepositForDebtIssueProceeds(qryParam);

            // The agentId and eventId have been validated, so is the combonation valid?
            if (result.Success)
            {
                // Guid.Empty means the creditor is not associated with this ageement.
                if (result.Result == 0)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(loanAgreement);
                    }
                }
                else
                {
                    string msg = $"Delete loan agreement failed! Cannot delete loan agreement after loan proceeds have been deposited.";
                    validationResult.Messages.Add(msg);
                }
            }
            else
            {
                validationResult.Messages.Add(result.NonSuccessMessage);
            }

            return validationResult;
        }
    }
}