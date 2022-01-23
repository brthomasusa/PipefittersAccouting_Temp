#pragma warning disable CS8602

using Microsoft.AspNetCore.Identity;
using PipefittersAccounting.Infrastructure.Identity;
using PipefittersAccounting.SharedModel.DataXferObjects.Identity;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Identity
{
    public static class UserRegistrationCommand
    {
        public static async Task<IdentityResult> Execute
        (
            UserForRegistrationDto userDto,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager
        )
        {
            ApplicationUser user = new()
            {
                Id = userDto.Id,
                UserName = userDto.UserName,
                Email = userDto.Email
            };

            IdentityResult result = await userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                foreach (var role in userDto.Roles)
                {
                    if (await roleManager.RoleExistsAsync(role))
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
            }

            return result;
        }
    }
}