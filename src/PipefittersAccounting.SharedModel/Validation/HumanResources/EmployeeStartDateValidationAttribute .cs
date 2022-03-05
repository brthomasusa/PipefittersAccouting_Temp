#pragma warning disable CS8603
#pragma warning disable CS8604
#pragma warning disable CS8620

using System.ComponentModel.DataAnnotations;

namespace PipefittersAccounting.SharedModel.Validation.HumanResources
{
    public class EmployeeStartDateValidationAttribute : ValidationAttribute
    {
        public string? MinStartDate { get; set; }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                throw new System.NullReferenceException("Can not validate a null object.");
            }

            DateTime startDate = (DateTime)value;

            if (startDate >= DateTime.Parse(MinStartDate) && startDate <= DateTime.Today)
            {
                return null;
            }

            return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
        }
    }
}