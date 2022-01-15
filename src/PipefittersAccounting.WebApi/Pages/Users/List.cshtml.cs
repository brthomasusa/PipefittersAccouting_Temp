#pragma warning disable CS8618

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PipefittersAccounting.Infrastructure.Identity;

namespace PipefittersAccounting.WebApi.Pages.Users
{
    public class ListModel : AdminPageModel
    {
        public UserManager<ApplicationUser> UserManager;
        public IEnumerable<ApplicationUser> Users { get; set; }

        public ListModel(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public void OnGet()
        {
            Users = UserManager.Users;
        }
    }
}
