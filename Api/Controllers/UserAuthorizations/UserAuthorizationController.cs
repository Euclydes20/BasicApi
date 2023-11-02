using Api.Domain.UserAuthorizations;
using Api.Models;
using Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.UserAuthorizations
{
    [ApiController]
    [Authorize]
    [Route("v1/[controller]")]
    [Tags("User Authorizations")]
    public class UserAuthorizationController : ControllerBase
    {
        private readonly IUserAuthorizationService _userAuthorizationService;

        public UserAuthorizationController(IUserAuthorizationService userAuthorizationService)
        {
            _userAuthorizationService = userAuthorizationService;
        }

        [Authorization(AuthorizationType.UserAuthorizationEdit)]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateAsync(UserAuthorization userAuthorization)
        {
            var response = new ResponseInfo<UserAuthorization>();
            try
            {
                response.Data = await _userAuthorizationService.UpdateAsync(userAuthorization);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }

        [Authorization(AuthorizationType.UserAuthorizationView)]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = new ResponseInfo<UserAuthorization?>();
            try
            {
                response.Data = await _userAuthorizationService.GetAsync(id);

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