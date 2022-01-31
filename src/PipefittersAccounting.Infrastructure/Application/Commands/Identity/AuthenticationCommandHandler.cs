#pragma warning disable CS8618
#pragma warning disable CS8604

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PipefittersAccounting.Infrastructure.Identity;
using PipefittersAccounting.SharedModel.DataXferObjects.Identity;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Identity
{
    public class AuthenticationCommandHandler
    {
        private readonly UserManager<ApplicationUser>? _userManager;
        private RoleManager<ApplicationRole>? _roleManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IConfiguration _configuration;

        public AuthenticationCommandHandler
        (
            UserManager<ApplicationUser> usrManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signinMgr,
            IConfiguration configuration
        )
        {
            _userManager = usrManager;
            _roleManager = roleManager;
            _signInManager = signinMgr;
            _configuration = configuration;
        }

        public Task<IdentityResult> HandleUserRegistration(UserForRegistrationDto userDto) =>
            UserRegistrationCommand.Execute(userDto, _userManager, _roleManager);

        public Task<bool> HandleUserForAuthentication(UserForAuthenticationDto userDto) =>
            UserAuthenticationCommand.Execute(userDto, _userManager, _signInManager);

        public Task<string> HandleTokenCreation(UserForAuthenticationDto userDto) =>
            CreateTokenCommand.Execute(userDto, _userManager, _configuration);
    }
}