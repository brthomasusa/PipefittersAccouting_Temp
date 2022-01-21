#pragma warning disable CS8618

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PipefittersAccounting.Infrastructure.Identity;
using PipefittersAccounting.SharedModel.DataXferObjects.Common;

namespace PipefittersAccounting.WebApi.Pages.Roles
{
    public class CreateModel : AdminPageModel
    {
        public RoleManager<ApplicationRole> RoleManager;

        [BindProperty]
        public DomainRoleDto DomainRole { get; set; } = new() { };

        public CreateModel(UserManager<ApplicationUser> userManager,
                RoleManager<ApplicationRole> roleManager)
        {
            RoleManager = roleManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ApplicationRole role = new() { Id = Guid.NewGuid(), Name = DomainRole.Name };

                IdentityResult result = await RoleManager.CreateAsync(role);

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
