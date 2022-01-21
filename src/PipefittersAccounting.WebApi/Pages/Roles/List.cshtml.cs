#pragma warning disable CS8618

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PipefittersAccounting.Infrastructure.Identity;

namespace PipefittersAccounting.WebApi.Pages.Roles
{
    public class ListModel : AdminPageModel
    {
        public UserManager<ApplicationUser> UserManager;
        public RoleManager<ApplicationRole> RoleManager;

        public IEnumerable<ApplicationRole> Roles { get; set; }

        public ListModel(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public void OnGet()
        {
            Roles = RoleManager.Roles;
        }

        public async Task<string> GetMembersString(string role)
        {
            IEnumerable<ApplicationUser> users = (await UserManager.GetUsersInRoleAsync(role));
            string result = users.Count() == 0 ? "No members" : string.Join(", ", users.Take(3).Select(u => u.UserName).ToArray());

            return users.Count() > 3 ? $"{result}, (plus others)" : result;
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            await RoleManager.DeleteAsync(role);
            return RedirectToPage();
        }
    }
}
