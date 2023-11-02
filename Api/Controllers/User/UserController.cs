using Api.Domain.Users;
using Api.Models;
using Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Users
{
    [ApiController]
    [Authorize]
    [Route("v1/[controller]")]
    [Tags("Users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddAsync(User user)
        {
            var response = new ResponseInfo<User>();
            try
            {
                var userAdded = await _userService.AddAsync(user);
                if (userAdded is not null)
                    userAdded.Password = null;

                response.Data = userAdded;

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }

        [Authorization(AuthorizationType.UserEdit)]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateAsync(User user)
        {
            var response = new ResponseInfo<User>();
            try
            {
                var userUpdated = await _userService.UpdateAsync(user);
                if (userUpdated is not null)
                    userUpdated.Password = null;

                response.Data = userUpdated;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }

        [Authorization(AuthorizationType.UserDelete)]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            var response = new ResponseInfo<object>();
            try
            {
                await _userService.RemoveAsync(id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }

        [Authorization(AuthorizationType.UserView)]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAsync()
        {
            var response = new ResponseInfo<IList<User>>();
            try
            {
                var users = (await _userService.GetAsync()).ToList();
                users.ForEach(u => u.ClearPassword());

                response.Data = users;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }

        [Authorization(AuthorizationType.UserView)]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = new ResponseInfo<User?>();
            try
            {
                var user = await _userService.GetAsync(id);
                user?.ClearPassword();

                response.Data = user;

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