#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using PipefittersAccounting.SharedModel.Interfaces;
using PipefittersAccounting.SharedModel.Validation.Common;

namespace PipefittersAccounting.SharedModel.WriteModels.Financing
{
    public class FinancierWriteModel : IWriteModel
    {
        public Guid Id { get; set; }
        public string FinancierName { get; set; }
        public string Telephone { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string Zipcode { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string? ContactMiddleInitial { get; set; }
        public string ContactTelephone { get; set; }
        public bool IsActive { get; set; }
        public Guid UserId { get; set; }
        // [RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$", ErrorMessage = "Enter a valid telephone number.")]
    }
}