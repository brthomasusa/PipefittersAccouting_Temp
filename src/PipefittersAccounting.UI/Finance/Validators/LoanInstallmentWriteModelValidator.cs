using FluentValidation;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.UI.Finance.Validators
{
    public class LoanInstallmentWriteModelValidator : AbstractValidator<LoanInstallmentWriteModel>
    {
        public LoanInstallmentWriteModelValidator()
        {
            RuleFor(install => install.LoanInstallmentId)
                                      .NotEmpty()
                                      .WithMessage("Missing installment Id; this is required.");

            RuleFor(install => install.LoanId)
                                      .NotEmpty()
                                      .WithMessage("Missing loan Id; this is required.");

            RuleFor(install => install.InstallmentNumber)
                                      .GreaterThan(0)
                                      .WithMessage("Missing installment number (can not be zero); this is required.");

            RuleFor(install => install.PaymentAmount)
                                      .GreaterThan(0)
                                      .WithMessage("Payment amount must be greater than zero.");

            RuleFor(install => install.PrincipalPymtAmount)
                                      .GreaterThan(0)
                                      .WithMessage("Principal payment amount must be greater than zero.");

            RuleFor(install => install.InterestPymtAmount)
                                      .GreaterThanOrEqualTo(0)
                                      .WithMessage("Interest payment amount can not be negative.");

            RuleFor(install => install.PrincipalRemaining)
                                      .GreaterThanOrEqualTo(0)
                                      .WithMessage("The remaining principal balance can not be negative.");
        }
    }
}