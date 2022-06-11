#pragma warning disable CS8602

using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Validation.Financing.LoanAgreementAggregate.BusinessRules
{
    public class VerifyLoanAgreementIsNotDuplicateRule : BusinessRule<LoanAgreementWriteModel>
    {
        private readonly ILoanAgreementQueryService _qrySvc;

        public VerifyLoanAgreementIsNotDuplicateRule(ILoanAgreementQueryService qrySvc)
            => _qrySvc = qrySvc;

        public override async Task<ValidationResult> Validate(LoanAgreementWriteModel loanAgreement)
        {
            ValidationResult validationResult = new();

            GetDuplicateLoanAgreement qryParam = new GetDuplicateLoanAgreement()
            {
                FinancierId = loanAgreement.FinancierId,
                LoanAmount = loanAgreement.LoanAmount,
                InterestRate = loanAgreement.InterestRate,
                LoanDate = loanAgreement.LoanDate,
                MaturityDate = loanAgreement.MaturityDate
            };

            OperationResult<Guid> result =
                await _qrySvc.GetLoanIdOfDuplicationLoanAgreement(qryParam);

            // Is the agent id known to the system?
            if (result.Success)
            {
                // Does the agent id represent a creditor?
                if (result.Result == Guid.Empty || result.Result == loanAgreement.LoanId)
                {
                    validationResult.IsValid = true;

                    if (Next is not null)
                    {
                        validationResult = await Next?.Validate(loanAgreement);
                    }
                }
                else
                {
                    string msg = $"This is a duplicate of an existing loan agreement '{result.Result}'!";
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