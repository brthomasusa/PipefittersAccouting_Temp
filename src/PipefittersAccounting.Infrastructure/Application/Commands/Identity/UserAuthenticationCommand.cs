#pragma warning disable CS8602

using Microsoft.AspNetCore.Identity;
using PipefittersAccounting.Infrastructure.Identity;
using PipefittersAccounting.SharedModel.DataXferObjects.Identity;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Identity
{
    public static class UserAuthenticationCommand
    {
        public static async Task<bool> Execute
        (
            UserForAuthenticationDto userDto,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signinMgr
        )
        {
            ApplicationUser user = await userManager.FindByNameAsync(userDto.UserName);

            bool result = (user != null && await userManager.CheckPasswordAsync(user, userDto.Password));

            return result;
        }
    }
}