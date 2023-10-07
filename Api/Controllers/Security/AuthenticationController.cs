using Api.Domain.Secutiry;
using Api.Models;
using Api.Models.Security;
using Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Security
{
    [AllowAnonymous]
    [ApiController]
    [Route("v1/[controller]")]
    [Tags("Authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate(Authentication authentication)
        {
            var response = new ResponseInfo<TokenInfo>();
            try
            {
                response.Data = await _authenticationService.Authenticate(authentication);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            var response = new ResponseInfo<Authentication>();
            try
            {
                await _authenticationService.Logout();

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }
    }
}
