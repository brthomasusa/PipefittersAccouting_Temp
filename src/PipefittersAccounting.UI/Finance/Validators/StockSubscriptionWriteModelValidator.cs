using FluentValidation;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.UI.Finance.Validators
{
    public class StockSubscriptionWriteModelValidator : AbstractValidator<StockSubscriptionWriteModel>
    {
        public StockSubscriptionWriteModelValidator()
        {
            RuleFor(subscription => subscription.StockId)
                                    .NotEmpty()
                                    .WithMessage("Missing stock subscription Id; this is required.");

            RuleFor(subscription => subscription.FinancierId)
                                    .NotEmpty()
                                    .WithMessage("Missing investor Id; this is required.");

            RuleFor(subscription => subscription.StockIssueDate)
                                    .NotEmpty()
                                    .WithMessage("Missing stock issue date; this is required.");

            RuleFor(subscription => subscription.SharesIssued)
                                    .GreaterThan(0)
                                    .WithMessage("Number of shares issued must be greater than zero.");

            RuleFor(subscription => subscription.PricePerShare)
                                    .GreaterThan(0)
                                    .WithMessage("Price per share must be greater than zero.");
        }
    }
}