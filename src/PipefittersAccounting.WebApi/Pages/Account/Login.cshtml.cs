#pragma warning disable CS8618

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using PipefittersAccounting.Infrastructure.Identity;
using PipefittersAccounting.SharedModel.DataXferObjects.Common;

namespace PipefittersAccounting.WebApi.Pages.Account
{
    public class LoginModel : PageModel
    {
        private SignInManager<ApplicationUser> signInManager;

        public LoginModel(SignInManager<ApplicationUser> signinMgr)
        {
            signInManager = signinMgr;
        }

        [BindProperty]
        [Required]
        public string UserName { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result =
                    await signInManager.PasswordSignInAsync(UserName, Password,
                        false, false);
                if (result.Succeeded)
                {
                    return Redirect(ReturnUrl ?? "/");
                }
                ModelState.AddModelError("", "Invalid username or password");
            }
            return Page();
        }
    }
}
