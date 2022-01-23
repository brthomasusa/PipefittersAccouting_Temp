#pragma warning disable CS8618

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PipefittersAccounting.Infrastructure.Identity;
using PipefittersAccounting.SharedModel.DataXferObjects.Identity;

namespace PipefittersAccounting.WebApi.Pages.Users
{
    public class CreateModel : AdminPageModel
    {
        public UserManager<ApplicationUser> UserManager;

        [BindProperty]
        public DomainUserDto DomainUser { get; set; } = new() { };

        public CreateModel(UserManager<ApplicationUser> usrManager)
        {
            UserManager = usrManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    Id = Guid.NewGuid(),
                    UserName = DomainUser.UserName,
                    Email = DomainUser.Email
                };

                IdentityResult result = await UserManager.CreateAsync(user, DomainUser.Password);

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
