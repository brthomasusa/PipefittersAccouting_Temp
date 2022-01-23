#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;

namespace PipefittersAccounting.SharedModel.DataXferObjects.Identity
{
    public class DomainUserDto
    {
        [Required(ErrorMessage = "User Id is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        public string UserName { get; init; }

        [Required]
        [EmailAddress(ErrorMessage = "An email address is required.")]
        public string Email { get; init; }

        public string Password { get; init; }
    }
}