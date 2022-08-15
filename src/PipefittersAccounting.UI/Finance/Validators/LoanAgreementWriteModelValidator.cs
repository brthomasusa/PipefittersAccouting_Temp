using FluentValidation;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.UI.Finance.Validators
{
    public class LoanAgreementWriteModelValidator : AbstractValidator<LoanAgreementWriteModel>
    {
        public LoanAgreementWriteModelValidator()
        {
            RuleFor(loan => loan.LoanId)
                                .NotEmpty()
                                .WithMessage("Missing loan Id; this is required.");

            RuleFor(loan => loan.FinancierId)
                                .NotEmpty()
                                .WithMessage("Missing financier Id; this is required.");

            RuleFor(loan => loan.LoanAmount)
                                .GreaterThan(0M)
                                .WithMessage("Loan amount must be greater than zero.");

            RuleFor(loan => loan.InterestRate)
                                .GreaterThanOrEqualTo(0)
                                .WithMessage("Loan amount must be greater than zero.")
                                .LessThanOrEqualTo(.2M)
                                .WithMessage("Maximum interest rate is 20% (0.20).");

            RuleFor(loan => loan.LoanDate)
                                .NotEmpty()
                                .WithMessage("Missing loan date; this is required.");

            RuleFor(loan => loan.MaturityDate)
                                .NotEmpty()
                                .WithMessage("Missing loan maturity date; this is required.");

            RuleFor(loan => loan).Custom((agreement, context) =>
            {
                if (agreement.LoanDate > agreement.MaturityDate)
                {
                    context.AddFailure("The loan date must be before the maturity date.");
                }
            });

            RuleFor(loan => loan.NumberOfInstallments)
                                .GreaterThan(0)
                                .WithMessage("Number of installments must be greater than zero.");

            RuleFor(loan => loan.AmortizationSchedule).Custom((list, context) =>
            {
                if (list.Count > 0)
                {
                    context.AddFailure("A loan amortization schedule is required.");
                }
            });

            RuleFor(loan => loan.UserId)
                                .NotEmpty()
                                .WithMessage("Missing user Id; this is required.");
        }
    }
}