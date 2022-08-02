using FluentValidation;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.UI.HumanResources.Validators
{
    public class EmployeeWriteModelValidator : AbstractValidator<EmployeeWriteModel>
    {
        private readonly string[] _stateCodes = { "AK", "AL", "AR", "AZ", "CA", "CO", "CT", "DC", "DE", "GA", "HI", "IA",
                                                  "ID", "IL", "IN", "KS", "KY", "LA", "MA", "ME", "MD", "MI", "MN", "MO",
                                                  "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK",
                                                  "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WI",
                                                  "WV", "WY" };

        public EmployeeWriteModelValidator()
        {
            RuleFor(employee => employee.EmployeeId)
                                        .NotEmpty()
                                        .WithMessage("Missing employee Id; this is required.");

            RuleFor(employee => employee.EmployeeType)
                                        .NotEmpty()
                                        .WithMessage("Missing employee type (job title); this is required.");

            RuleFor(employee => employee.SupervisorId)
                                        .NotEmpty()
                                        .WithMessage("Missing supervisor Id; this is required.");

            RuleFor(employee => employee.FirstName)
                                        .NotEmpty().WithMessage("Employee first name; this is required.")
                                        .MaximumLength(25).WithMessage("Employee first name cannot be longer than 25 characters");

            RuleFor(employee => employee.LastName)
                                        .NotEmpty().WithMessage("Employee last name; this is required.")
                                        .MaximumLength(25).WithMessage("Employee last name cannot be longer than 25 characters");

            RuleFor(employee => employee.MiddleInitial)
                                        .MaximumLength(1).WithMessage("Middle initial cannot be longer than 1 character");

            RuleFor(employee => employee.SSN)
                                        .NotEmpty().WithMessage("Missing social security number; this is required.")
                                        .MaximumLength(14).WithMessage("Social security number cannot be longer than 9 characters")
                                        .Matches(@"^(?!219099999|078051120)(?!666|000|9\d{2})\d{3}(?!00)\d{2}(?!0{4})\d{4}$")
                                            .WithMessage("A valid social security number looks like: 123456789 (no dashes)");

            RuleFor(employee => employee.Telephone)
                                        .NotEmpty().WithMessage("Missing employee telephone number; this is required.")
                                        .MaximumLength(14).WithMessage("Telephone number cannot be longer than 14 characters")
                                        .Matches(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$")
                                            .WithMessage("A valid phone number looks like: 123-456-7890");

            RuleFor(employee => employee.AddressLine1)
                                        .NotEmpty().WithMessage("Missing address line; this is required.")
                                        .MaximumLength(30).WithMessage("Address line cannot be longer than 30 characters");

            RuleFor(employee => employee.AddressLine2)
                                        .MaximumLength(30).WithMessage("Address line cannot be longer than 30 characters");

            RuleFor(employee => employee.City)
                                        .NotEmpty().WithMessage("Missing city name; this is required.")
                                        .MaximumLength(30).WithMessage("City name cannot be longer than 30 characters");

            RuleFor(employee => employee.StateCode)
                                        .NotEmpty().WithMessage("Missing state; this is required.")
                                        .Must(BeValidStateCode)
                                            .WithMessage("Invalid state code; a two digit U.S. state code is required."); ;

            RuleFor(employee => employee.Zipcode)
                                        .NotEmpty().WithMessage("Missing zipcode; this is required.")
                                        .Matches(@"^\d{5}(?:[-\s]\d{4})?$")
                                            .WithMessage("A valid zipcode looks like: 12345 or 12345-7890");

            // RuleFor(employee => employee.MaritalStatus)
            //                             .NotEmpty().WithMessage("Marital status is required.")
            //                             .Must(BeValidMaritalStatus)
            //                                 .WithMessage("Invalid marital status; use S for single and M for married.");

            RuleFor(employee => employee.MaritalStatus)
                                        .Must(status => status == "S" || status == "M")
                                        .WithMessage("Invalid marital status; use S for single and M for married.");

            RuleFor(employee => employee.Exemptions)
                                        .InclusiveBetween(0, 11)
                                        .WithMessage("Number of exemptions must be between zero and eleven.");

            RuleFor(employee => employee.PayRate)
                                        .InclusiveBetween(7.50M, 40.00M)
                                        .WithMessage("Invalid pay rate, must be between $7.50 and $40.00 (per hour) inclusive!")
                                        .ScalePrecision(2, 4)
                                        .WithMessage("Invalid pay rate, can not have move than two decimal places!");

            RuleFor(employee => employee.StartDate)
                                        .NotEmpty().WithMessage("Start date is required.")
                                        .GreaterThanOrEqualTo(new DateTime(1998, 1, 1))
                                        .WithMessage("Must be greater than Jan 1st, 1998.");
        }

        protected bool BeValidStateCode(string stateCode)
            => Array.Exists(_stateCodes, element => element == stateCode.ToUpper());

        protected bool BeValidMaritalStatus(string maritalStatus)
        {
            bool retVal = false;

            if (maritalStatus is not null)
                retVal = (maritalStatus.ToUpper() == "M" || maritalStatus.ToUpper() == "S");

            return retVal;
        }

    }
}