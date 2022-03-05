#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using PipefittersAccounting.SharedModel.Interfaces;
using PipefittersAccounting.SharedModel.Validation.Common;
using PipefittersAccounting.SharedModel.Validation.HumanResources;

namespace PipefittersAccounting.SharedModel.WriteModels.HumanResources
{
    public class CreateEmployeeInfo : IWriteModel
    {
        [Required(ErrorMessage = "Employee Id is required.")]
        [NoDefaultGuidValidation(ErrorMessage = "System error!! Employee Id can not be a default Guild.")]
        public Guid Id { get; set; }

        [NoDefaultGuidValidation(ErrorMessage = @"You must select a Manager; '-- Not Selected --' is not valid.")]
        [Required(ErrorMessage = "Supervisor Id is required.")]
        public Guid SupervisorId { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(25, ErrorMessage = "Last name is too long, max 25 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(25, ErrorMessage = "First name is too long, max 25 characters.")]
        public string FirstName { get; set; }

        [MaxLength(1)]
        public string MiddleInitial { get; set; }

        [Required(ErrorMessage = "A valid social security number is required.")]
        [RegularExpression(@"^(?!219099999|078051120)(?!666|000|9\d{2})\d{3}(?!00)\d{2}(?!0{4})\d{4}$", ErrorMessage = "Enter a valid social security number.")]
        public string SSN { get; set; }

        [Required(ErrorMessage = "A valid phone number is required.")]
        [RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$", ErrorMessage = "Enter a valid telephone number.")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Address line 1 is required.")]
        [StringLength(25, ErrorMessage = "Address line 1 is too long, max 30 characters.")]
        public string AddressLine1 { get; set; }

        [StringLength(25, ErrorMessage = "Address line 2 is too long, max 30 characters.")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "City name is required.")]
        [StringLength(25, ErrorMessage = "City name is too long, max 30 characters.")]
        public string City { get; set; }

        [MaxLength(2)]
        [MinLength(2)]
        [Required(ErrorMessage = "The 2-digit state code is required.")]
        public string StateCode { get; set; }

        [Required(ErrorMessage = "Zipcode is required.")]
        [StringLength(25, ErrorMessage = "Zipcode is too long, max 10 characters.")]
        public string Zipcode { get; set; }

        [MaxLength(1)]
        [MinLength(1)]
        [Required(ErrorMessage = "Marital status, M for married or S for single, is required.")]
        public string MaritalStatus { get; set; }

        [EmployeeExemptionsValidation(ErrorMessage = "A valid number of tax exemptions is between 0 and 12.", ValidMinExemptions = 0, ValidMaxExemptions = 12)]
        public int Exemptions { get; set; }

        [Required(ErrorMessage = "A pay rate between $7.50 and $40.00 per hour is required.")]
        [Range(typeof(decimal), "7.50", "40.00", ErrorMessage = "A valid pay rate is between $7.50 and $40.00 per hour.")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PayRate { get; set; }

        [Required(ErrorMessage = @"The employee's date of hire is required.")]
        [EmployeeStartDateValidation(ErrorMessage = "A valid date of hire is between Dec 2, 1998 and today.", MinStartDate = "1998-12-02")]
        public DateTime StartDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsSupervisor { get; set; }
    }

    public class EditEmployeeInfo : IWriteModel
    {
        [Required(ErrorMessage = "Employee Id is required.")]
        [NoDefaultGuidValidation(ErrorMessage = "System error!! Employee Id can not be a default Guild.")]
        public Guid Id { get; set; }

        [NoDefaultGuidValidation(ErrorMessage = @"You must select a Manager; '-- Not Selected --' is not valid.")]
        [Required(ErrorMessage = "Supervisor Id is required.")]
        public Guid SupervisorId { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(25, ErrorMessage = "Last name is too long, max 25 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(25, ErrorMessage = "First name is too long, max 25 characters.")]
        public string FirstName { get; set; }

        [MaxLength(1)]
        public string MiddleInitial { get; set; }

        [Required(ErrorMessage = "A valid social security number is required.")]
        [RegularExpression(@"^(?!219099999|078051120)(?!666|000|9\d{2})\d{3}(?!00)\d{2}(?!0{4})\d{4}$", ErrorMessage = "Enter a valid social security number.")]
        public string SSN { get; set; }

        [Required(ErrorMessage = "A valid phone number is required.")]
        [RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$", ErrorMessage = "Enter a valid telephone number.")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Address line 1 is required.")]
        [StringLength(25, ErrorMessage = "Address line 1 is too long, max 30 characters.")]
        public string AddressLine1 { get; set; }

        [StringLength(25, ErrorMessage = "Address line 2 is too long, max 30 characters.")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "City name is required.")]
        [StringLength(25, ErrorMessage = "City name is too long, max 30 characters.")]
        public string City { get; set; }

        [MaxLength(2)]
        [MinLength(2)]
        [Required(ErrorMessage = "The 2-digit state code is required.")]
        public string StateCode { get; set; }

        [Required(ErrorMessage = "Zipcode is required.")]
        [StringLength(25, ErrorMessage = "Zipcode is too long, max 10 characters.")]
        public string Zipcode { get; set; }

        [MaxLength(1)]
        [MinLength(1)]
        [Required(ErrorMessage = "Marital status, M for married or S for single, is required.")]
        public string MaritalStatus { get; set; }

        [EmployeeExemptionsValidation(ErrorMessage = "A valid number of tax exemptions is between 0 and 12.", ValidMinExemptions = 0, ValidMaxExemptions = 12)]
        public int Exemptions { get; set; }

        [Required(ErrorMessage = "A pay rate between $7.50 and $40.00 per hour is required.")]
        [Range(typeof(decimal), "7.50", "40.00", ErrorMessage = "A valid pay rate is between $7.50 and $40.00 per hour.")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PayRate { get; set; }

        [Required(ErrorMessage = @"The employee's date of hire is required.")]
        [EmployeeStartDateValidation(ErrorMessage = "A valid date of hire is between Dec 2, 1998 and today.", MinStartDate = "1998-12-02")]
        public DateTime StartDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsSupervisor { get; set; }
    }

    public class DeleteEmployeeInfo : IWriteModel
    {
        [Required(ErrorMessage = "Employee Id is required.")]
        [NoDefaultGuidValidation(ErrorMessage = "System error!! Employee Id can not be a default Guild.")]
        public Guid Id { get; set; }
    }

    public class CheckForDuplicateEmployeeName : IWriteModel
    {
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(25, ErrorMessage = "Last name is too long, max 25 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(25, ErrorMessage = "First name is too long, max 25 characters.")]
        public string FirstName { get; set; }

        public string? MiddleInitial { get; set; }
    }

    public class CheckForDuplicateSSN : IWriteModel
    {
        [Required(ErrorMessage = "A valid social security number is required.")]
        [RegularExpression(@"^(?!219099999|078051120)(?!666|000|9\d{2})\d{3}(?!00)\d{2}(?!0{4})\d{4}$", ErrorMessage = "Enter a valid social security number.")]
        public string SSN { get; set; }
    }
}