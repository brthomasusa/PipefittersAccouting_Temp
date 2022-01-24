#pragma warning disable CS8618
#pragma warning disable CS8604

using Microsoft.AspNetCore.Identity;
using PipefittersAccounting.Infrastructure.Identity;
using PipefittersAccounting.SharedModel.DataXferObjects.Identity;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Identity
{
    public class AuthenticationCommandHandler
    {
        private UserManager<ApplicationUser>? _userManager;
        private RoleManager<ApplicationRole>? _roleManager;
        private SignInManager<ApplicationUser> _signInManager;

        public AuthenticationCommandHandler
        (
            UserManager<ApplicationUser> usrManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signinMgr
        )
        {
            _userManager = usrManager;
            _roleManager = roleManager;
            _signInManager = signinMgr;
        }

        public Task<IdentityResult> Handle(UserForRegistrationDto userDto) =>
            UserRegistrationCommand.Execute(userDto, _userManager, _roleManager);

        public Task<bool> Handle(UserForAuthenticationDto userDto) =>
            UserAuthenticationCommand.Execute(userDto, _userManager, _signInManager);
    }
}