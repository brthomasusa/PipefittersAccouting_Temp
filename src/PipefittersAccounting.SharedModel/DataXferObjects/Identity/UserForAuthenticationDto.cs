using System.ComponentModel.DataAnnotations;
using PipefittersAccounting.SharedModel.Interfaces;

namespace PipefittersAccounting.SharedModel.DataXferObjects.Identity
{
    public class UserForAuthenticationDto : IWriteDto
    {
        [Required(ErrorMessage = "User name is required.")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "A password is required.")]
        public string? Password { get; init; }
    }
}