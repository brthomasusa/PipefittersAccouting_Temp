#pragma warning disable CS8602

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PipefittersAccounting.SharedModel.DataXferObjects.Identity;
using PipefittersAccounting.Infrastructure.Application.Commands.Identity;

namespace PipefittersAccounting.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly AuthenticationCommandHandler? _cmdHdlr;

        public AuthenticationController(AuthenticationCommandHandler handler, ILogger<AuthenticationController> logger)
        {
            _cmdHdlr = handler;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userDto)
        {
            if (userDto.Id == default)
            {
                userDto.Id = Guid.NewGuid();
            }

            var result = await _cmdHdlr.HandleUserRegistration(userDto);
            _logger.LogInformation($"Created login for {userDto.UserName} with email: {userDto.Email}.");

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                    _logger.LogError($"Creating login for {userDto.UserName} failed. Error code: {error.Code}, error description: {error.Description}.");
                    return BadRequest(ModelState);
                }
            }

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateUser([FromBody] UserForAuthenticationDto userDto)
        {
            if (!await _cmdHdlr.HandleUserForAuthentication(userDto))
            {
                return Unauthorized();
            }

            return Ok(new
            {
                Token = _cmdHdlr.HandleTokenCreation(userDto)
            });
        }
    }
}