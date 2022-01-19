#pragma warning disable CS8620 
#pragma warning disable CS8603 

using System;
using System.ComponentModel.DataAnnotations;

namespace PipefittersAccounting.SharedModel.Validation.Common
{
    public class NoDefaultGuidValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                throw new System.NullReferenceException("Can not validate a null object.");
            }

            Guid id = (Guid)value;

            if (id != default)
            {
                return null;
            }

            return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
        }
    }
}