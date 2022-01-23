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
        private readonly AuthenticationCommandHandler? _cmdHdlr;

        public AuthenticationController(AuthenticationCommandHandler handler)
        {
            _cmdHdlr = handler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userDto)
        {
            if (userDto.Id == default)
            {
                userDto.Id = Guid.NewGuid();
            }

            var result = await _cmdHdlr.Handle(userDto);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                    return BadRequest(ModelState);
                }
            }

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateUser([FromBody] UserForAuthenticationDto userDto)
        {
            if (!await _cmdHdlr.Handle(userDto))
            {
                return Unauthorized();
            }

            return Ok();
        }
    }
}