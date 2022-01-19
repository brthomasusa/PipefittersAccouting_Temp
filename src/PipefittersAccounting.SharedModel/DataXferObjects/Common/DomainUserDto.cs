#pragma warning disable CS8618

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PipefittersAccounting.SharedModel.Validation.Common;

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