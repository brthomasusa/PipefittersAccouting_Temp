#pragma warning disable CS8618

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PipefittersAccounting.Infrastructure.Identity;
using PipefittersAccounting.SharedModel.DataXferObjects.Identity;

namespace PipefittersAccounting.WebApi.Pages.Users
{
    public class EditorModel : AdminPageModel
    {
        public UserManager<ApplicationUser> UserManager;

        [BindProperty]
        public DomainUserDto DomainUser { get; set; }

        public EditorModel(UserManager<ApplicationUser> usrManager)
        {
            UserManager = usrManager;
        }

        public async Task OnGetAsync(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            DomainUser = new()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(DomainUser.Id.ToString());
                user.UserName = DomainUser.UserName;
                user.Email = DomainUser.Email;

                IdentityResult result = await UserManager.UpdateAsync(user);

                if (result.Succeeded && !String.IsNullOrEmpty(DomainUser.Password))
                {
                    await UserManager.RemovePasswordAsync(user);
                    result = await UserManager.AddPasswordAsync(user, DomainUser.Password);
                }
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
