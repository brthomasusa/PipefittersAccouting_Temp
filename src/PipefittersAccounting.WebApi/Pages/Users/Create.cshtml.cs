#pragma warning disable CS8618

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using PipefittersAccounting.Infrastructure.Identity;

namespace PipefittersAccounting.WebApi.Pages.Users
{
    public class CreateModel : AdminPageModel
    {
        public UserManager<ApplicationUser> UserManager;

        public CreateModel(UserManager<ApplicationUser> usrManager)
        {
            UserManager = usrManager;
        }

        [BindProperty]
        [Required]
        public string UserName { get; set; }

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { Id = Guid.NewGuid(), UserName = UserName, Email = Email };
                IdentityResult result = await UserManager.CreateAsync(user, Password);

                if (result.Succeeded)
                {
                    return RedirectToPage("List");
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            return Page();
        }
    }
}
