using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using PipefittersAccounting.Infrastructure.Identity;

namespace PipefittersAccounting.WebApi.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private SignInManager<ApplicationUser> signInManager;

        public LogoutModel(SignInManager<ApplicationUser> signInMgr)
        {
            signInManager = signInMgr;
        }

        public async Task OnGetAsync()
        {
            await signInManager.SignOutAsync();
        }
    }
}
