using FluentValidation;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.SharedModel.Validation.Financing
{
    public class LoanAgreementWriteModelValidator : AbstractValidator<LoanAgreementWriteModel>
    {
        public LoanAgreementWriteModelValidator()
        {
            RuleFor(agreement => agreement.LoanId)
                                            .NotEmpty()
                                            .WithMessage("Missing LoanId; this is required.");

            RuleFor(agreement => agreement.FinancierId)
                                            .NotEmpty()
                                            .WithMessage("Missing FinancierId; this is required.");

            RuleFor(agreement => agreement.LoanAmount)
                                            .GreaterThan(0)
                                            .ScalePrecision(2, 7)
                                            .WithMessage("The amount borrowed must be greater than zero.");

            RuleFor(agreement => agreement.InterestRate)
                                            .GreaterThan(0)
                                            .ScalePrecision(3, 4)
                                            .WithMessage("The interest rate must be greater than or equal to zero.");

            RuleFor(agreement => agreement.LoanDate)
                                            .NotEmpty()
                                            .WithMessage("A non default payment due date is required.");

            RuleFor(agreement => agreement.MaturityDate)
                                            .NotEmpty()
                                            .WithMessage("A non default payment due date is required.");

            RuleFor(agreement => agreement.NumberOfInstallments)
                                            .GreaterThan(0)
                                            .WithMessage("Negative installment numbers are not allowed.");

            RuleFor(agreement => agreement.MaturityDate).Must((agreement, MaturityDate) => MaturityDate > agreement.LoanDate)
                                                            .WithMessage("Maturity date must be greater than the loan date.");

            RuleFor(agreement => agreement.UserId)
                                             .NotEmpty()
                                             .WithMessage("Missing User Id; this is required.");

            RuleForEach(agreement => agreement.AmortizationSchedule)
                                        .SetValidator(new LoanInstallmentWriteModelValidator());

            string errMsg = "Check amortization schedule: number of installments should match loan agreement must fall between loan date and maturity date.";
            RuleFor(agreement => agreement.AmortizationSchedule).Must((agreement, AmortizationSchedule) => ValidAmortizationSchedule(agreement, AmortizationSchedule))
                                                                .WithMessage(errMsg);
        }

        private bool ValidAmortizationSchedule(LoanAgreementWriteModel agreementInfo, List<LoanInstallmentWriteModel> schedule)
        {
            if (agreementInfo.NumberOfInstallments != schedule.Count)
                return false;

            foreach (var installment in schedule)
            {
                if (installment.PaymentDueDate < agreementInfo.LoanDate || installment.PaymentDueDate > agreementInfo.MaturityDate)
                    return false;

                if (installment.PaymentAmount != (installment.InterestPymtAmount + installment.PrincipalPymtAmount))
                    return false;
            }

            return true;
        }
    }
}