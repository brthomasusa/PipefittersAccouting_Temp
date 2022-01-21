#pragma warning disable CS8618

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PipefittersAccounting.Infrastructure.Identity;
using PipefittersAccounting.SharedModel.DataXferObjects.Common;

namespace PipefittersAccounting.WebApi.Pages.Roles
{
    public class EditorModel : AdminPageModel
    {
        public UserManager<ApplicationUser> UserManager;
        public RoleManager<ApplicationRole> RoleManager;
        public ApplicationRole Role { get; set; }

        public EditorModel(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public Task<IList<ApplicationUser>> Members() =>
            UserManager.GetUsersInRoleAsync(Role.Name);

        public async Task<IEnumerable<ApplicationUser>> NonMembers() =>
            UserManager.Users.ToList().Except(await Members());

        public async Task OnGetAsync(string id)
        {
            Role = await RoleManager.FindByIdAsync(id);
        }

        public async Task<IActionResult> OnPostAsync(string userid, string rolename)
        {
            Role = await RoleManager.FindByNameAsync(rolename);

            ApplicationUser user = await UserManager.FindByIdAsync(userid);

            IdentityResult result;

            if (await UserManager.IsInRoleAsync(user, rolename))
            {
                result = await UserManager.RemoveFromRoleAsync(user, rolename);
            }
            else
            {
                result = await UserManager.AddToRoleAsync(user, rolename);
            }

            if (result.Succeeded)
            {
                return RedirectToPage(new { id = Role.Id });
            }
            else
            {
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

                return Page();
            }
        }
    }
}
