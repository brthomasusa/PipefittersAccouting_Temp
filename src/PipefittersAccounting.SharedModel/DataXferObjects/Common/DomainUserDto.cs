#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;

namespace PipefittersAccounting.SharedModel.DataXferObjects.Common
{
    public class DomainUserDto
    {
        [Required(ErrorMessage = "User Id is required.")]
        // [NoDefaultGuidValidation(ErrorMessage = "Error!! User Id can not be a default Guild.")]
        public Guid Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}