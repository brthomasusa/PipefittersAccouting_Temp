using FluentValidation;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.SharedModel.Validation.Financing
{
    public class FinancierWriteModelValidator : AbstractValidator<FinancierWriteModel>
    {
        public FinancierWriteModelValidator()
        {
            RuleFor(financier => financier.Id)
                                          .NotEmpty()
                                          .WithMessage("Missing FinancierId; this is required.");

            RuleFor(financier => financier.FinancierName)
                                          .NotEmpty().WithMessage("Missing FinancierId; this is required.")
                                          .MaximumLength(50).WithMessage("Financier name cannot be longer than 50 characters");
        }
    }
}