#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using PipefittersAccounting.SharedModel.Interfaces;
using PipefittersAccounting.SharedModel.Validation.Common;

namespace PipefittersAccounting.SharedModel.WriteModels.Financing
{
    public class CreateFinancierInfo : IWriteModel
    {
        [Required(ErrorMessage = "Financier Id is required.")]
        [NoDefaultGuidValidation(ErrorMessage = "System error!! Financier Id can not be a default Guild.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Financier name is required.")]
        [StringLength(50, ErrorMessage = "Financier name is too long, max 50 characters.")]
        public string FinancierName { get; set; }

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

        [Required(ErrorMessage = "Contact first name is required.")]
        [StringLength(25, ErrorMessage = "Contact first name is too long, max 25 characters.")]
        public string ContactFirstName { get; set; }

        [Required(ErrorMessage = "Contact last name is required.")]
        [StringLength(25, ErrorMessage = "Contact last name is too long, max 25 characters.")]
        public string ContactLastName { get; set; }

        [MaxLength(1)]
        public string ContactMiddleInitial { get; set; }

        [Required(ErrorMessage = "A valid contact phone number is required.")]
        [RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$", ErrorMessage = "Enter a valid contact telephone number.")]
        public string ContactTelephone { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "User Id is required.")]
        [NoDefaultGuidValidation(ErrorMessage = "System error!! User Id can not be a default Guild.")]
        public Guid UserId { get; set; }
    }

    public class EditFinancierInfo : IWriteModel
    {
        [Required(ErrorMessage = "Financier Id is required.")]
        [NoDefaultGuidValidation(ErrorMessage = "System error!! Financier Id can not be a default Guild.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Financier name is required.")]
        [StringLength(50, ErrorMessage = "Financier name is too long, max 50 characters.")]
        public string FinancierName { get; set; }

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

        [Required(ErrorMessage = "Contact first name is required.")]
        [StringLength(25, ErrorMessage = "Contact first name is too long, max 25 characters.")]
        public string ContactFirstName { get; set; }

        [Required(ErrorMessage = "Contact last name is required.")]
        [StringLength(25, ErrorMessage = "Contact last name is too long, max 25 characters.")]
        public string ContactLastName { get; set; }

        [MaxLength(1)]
        public string ContactMiddleInitial { get; set; }

        [Required(ErrorMessage = "A valid contact phone number is required.")]
        [RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$", ErrorMessage = "Enter a valid contact telephone number.")]
        public string ContactTelephone { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "User Id is required.")]
        [NoDefaultGuidValidation(ErrorMessage = "System error!! User Id can not be a default Guild.")]
        public Guid UserId { get; set; }
    }

    public class DeleteFinancierInfo : IWriteModel
    {
        [Required(ErrorMessage = "Financier Id is required.")]
        [NoDefaultGuidValidation(ErrorMessage = "System error!! Financier Id can not be a default Guild.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User Id is required.")]
        [NoDefaultGuidValidation(ErrorMessage = "System error!! User Id can not be a default Guild.")]
        public Guid UserId { get; set; }
    }
}