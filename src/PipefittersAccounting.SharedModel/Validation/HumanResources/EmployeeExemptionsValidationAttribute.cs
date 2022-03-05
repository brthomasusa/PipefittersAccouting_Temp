#pragma warning disable CS8603
#pragma warning disable CS8604
#pragma warning disable CS8620

using System.ComponentModel.DataAnnotations;

namespace PipefittersAccounting.SharedModel.Validation.HumanResources
{
    public class EmployeeExemptionsValidationAttribute : ValidationAttribute
    {
        public int ValidMinExemptions { get; set; }
        public int ValidMaxExemptions { get; set; }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                throw new System.NullReferenceException("Can not validate a null object.");
            }

            int exemptions = (int)value;

            if (exemptions >= ValidMinExemptions && exemptions <= ValidMaxExemptions)
            {
                return null;
            }

            return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
        }
    }
}