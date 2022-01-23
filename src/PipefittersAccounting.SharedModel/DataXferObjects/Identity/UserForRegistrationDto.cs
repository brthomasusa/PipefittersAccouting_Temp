using System.ComponentModel.DataAnnotations;
using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.SharedModel.DataXferObjects.Identity
{
    public record UserForRegistrationDto : IWriteDto
    {
        [Required(ErrorMessage = "User Id is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "An email address is required.")]
        [EmailAddress]
        public string? Email { get; init; }

        [Required(ErrorMessage = "A password is required.")]
        public string? Password { get; init; }

        public ICollection<string>? Roles { get; init; }
    }
}