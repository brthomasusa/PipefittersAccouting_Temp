#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;

namespace PipefittersAccounting.SharedModel.DataXferObjects.Common
{
    public class DomainRoleDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(maximumLength: 256)]
        public string Name { get; set; }
    }
}