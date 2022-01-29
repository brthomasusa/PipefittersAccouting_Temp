#pragma warning disable CS8604

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using PipefittersAccounting.Infrastructure.Identity;
using PipefittersAccounting.SharedModel.DataXferObjects.Identity;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Identity
{
    public static class CreateTokenCommand
    {
        public static async Task<string> Execute
        (
            UserForAuthenticationDto userDto,
            UserManager<ApplicationUser> usrManager,
            IConfiguration configuration
        )
        {
            var signingCredentials = GetSigningCredentials(configuration);
            var claims = await GetClaims(usrManager, userDto);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims, configuration);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private static SigningCredentials GetSigningCredentials(IConfiguration configuration)
        {
            var key = Encoding.UTF8.GetBytes(configuration.GetSection("JwtApiKey").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private static async Task<List<Claim>> GetClaims(UserManager<ApplicationUser> usrManager, UserForAuthenticationDto userDto)
        {
            ApplicationUser user = await usrManager.FindByNameAsync(userDto.UserName);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await usrManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private static JwtSecurityToken GenerateTokenOptions
        (
            SigningCredentials signingCredentials,
            List<Claim> claims,
            IConfiguration configuration
        )
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }
    }
}