using FluentValidation;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.UI.Finance.Validators
{
    public class FinancierWriteModelValidator : AbstractValidator<FinancierWriteModel>
    {
        private readonly string[] _stateCodes = { "AK", "AL", "AR", "AZ", "CA", "CO", "CT", "DC", "DE", "GA", "HI", "IA",
                                                  "ID", "IL", "IN", "KS", "KY", "LA", "MA", "ME", "MD", "MI", "MN", "MO",
                                                  "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK",
                                                  "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WI",
                                                  "WV", "WY" };

        public FinancierWriteModelValidator()
        {
            RuleFor(financier => financier.Id)
                                          .NotEmpty()
                                          .WithMessage("Missing Financier Id; this is required.");

            RuleFor(financier => financier.FinancierName)
                                          .NotEmpty().WithMessage("Missing FinancierId; this is required.")
                                          .MaximumLength(50).WithMessage("Financier name cannot be longer than 50 characters");

            RuleFor(financier => financier.Telephone)
                                          .NotEmpty().WithMessage("Missing FinancierId; this is required.")
                                          .MaximumLength(14).WithMessage("Telephone number cannot be longer than 15 characters")
                                          .Matches(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$")
                                            .WithMessage("A valid phone number looks like: 123-456-7890");

            RuleFor(financier => financier.AddressLine1)
                                          .NotEmpty().WithMessage("Missing address line; this is required.")
                                          .MaximumLength(30).WithMessage("Address line cannot be longer than 30 characters");

            RuleFor(financier => financier.AddressLine2)
                                          .MaximumLength(30).WithMessage("Address line cannot be longer than 30 characters");

            RuleFor(financier => financier.City)
                                          .NotEmpty().WithMessage("Missing city name; this is required.")
                                          .MaximumLength(30).WithMessage("City name cannot be longer than 30 characters");

            RuleFor(financier => financier.StateCode)
                                          .NotEmpty().WithMessage("Missing state; this is required.")
                                          .Must(BeValidStateCode)
                                          .WithMessage("Invalid state code; a two digit U.S. state code is required."); ;

            RuleFor(financier => financier.Zipcode)
                                          .NotEmpty().WithMessage("Missing zipcode; this is required.")
                                          .Matches(@"^\d{5}(?:[-\s]\d{4})?$")
                                          .WithMessage("A valid zipcode looks like: 12345 or 12345-7890");

            RuleFor(financier => financier.ContactFirstName)
                                          .NotEmpty().WithMessage("Contact first name; this is required.")
                                          .MaximumLength(30).WithMessage("Contact first name cannot be longer than 30 characters");

            RuleFor(financier => financier.ContactLastName)
                                          .NotEmpty().WithMessage("Contact last name; this is required.")
                                          .MaximumLength(30).WithMessage("Contact last name cannot be longer than 30 characters");

            RuleFor(financier => financier.ContactMiddleInitial)
                                          .MaximumLength(1).WithMessage("Middle initial cannot be longer than 1 character");

            RuleFor(financier => financier.ContactTelephone)
                                          .NotEmpty().WithMessage("Missing contact telephone number; this is required.")
                                          .MaximumLength(14).WithMessage("Telephone number cannot be longer than 14 characters")
                                          .Matches(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$")
                                            .WithMessage("A valid phone number looks like: 123-456-7890");

            RuleFor(financier => financier.IsActive).NotNull();

            RuleFor(financier => financier.UserId)
                                          .NotEmpty()
                                          .WithMessage("Missing user id; this is required.");
        }

        protected bool BeValidStateCode(string stateCode)
            => Array.Exists(_stateCodes, element => element == stateCode.ToUpper());
    }
}