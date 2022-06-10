using FluentValidation;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.SharedModel.Validation.Financing
{
    public class LoanInstallmentWriteModelValidator : AbstractValidator<LoanInstallmentWriteModel>
    {
        public LoanInstallmentWriteModelValidator()
        {
            RuleFor(installment => installment.LoanInstallmentId)
                                                .NotEmpty()
                                                .WithMessage("Missing LoanInstallmentId; this is required.");

            RuleFor(installment => installment.LoanId)
                                                .NotEmpty()
                                                .WithMessage("Missing LoanId; this is required.");

            RuleFor(installment => installment.InstallmentNumber)
                                                .GreaterThan(0)
                                                .WithMessage("Negative installment numbers are not allowed.");

            RuleFor(installment => installment.PaymentDueDate)
                                                .NotEmpty()
                                                .WithMessage("A non default payment due date is required.");

            RuleFor(installment => installment.EqualMonthlyInstallment)
                                                .GreaterThan(0)
                                                .LessThanOrEqualTo(115000M)
                                                .ScalePrecision(2, 7)
                                                .WithMessage("Monthly payment amount (EMI) can not be negative or greater thant $115,000.00.");

            RuleFor(installment => installment.LoanPrincipalAmount)
                                                .GreaterThan(0)
                                                .LessThanOrEqualTo(100000M).WithMessage("Principal portion of payment can not be negative or greater thant $100,000.00.");

            RuleFor(installment => installment.LoanInterestAmount)
                                                .GreaterThanOrEqualTo(0)
                                                .WithMessage("Interest portion of payment can not be negative.");

            RuleFor(installment => installment.LoanPrincipalRemaining)
                                                .GreaterThanOrEqualTo(0)
                                                .LessThanOrEqualTo(115000M)
                                                .WithMessage("Loan balance remaining can not be negative or greater thant $115,000.00.");

            RuleFor(installment => installment.UserId)
                                                .NotEmpty()
                                                .WithMessage("Missing User Id; this is required.");

            RuleFor(installment => installment).Must(installment => ValidPrincipalInterestEMI(installment))
                                               .WithMessage("Principal + interest must equal EMI.");
        }

        private bool ValidPrincipalInterestEMI(LoanInstallmentWriteModel installmentInfo)
        {
            if (installmentInfo.EqualMonthlyInstallment != (installmentInfo.LoanInterestAmount + installmentInfo.LoanPrincipalAmount))
                return false;

            return true;
        }
    }
}