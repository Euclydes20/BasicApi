using Api.Domain.Users;
using Api.Models;
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

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddAsync(User user)
        {
            var response = new ResponseInfo<User>();
            try
            {
                response.Data = await _userService.AddAsync(user);

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateAsync(User user)
        {
            var response = new ResponseInfo<User>();
            try
            {
                response.Data = await _userService.UpdateAsync(user);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }

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

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAsync()
        {
            var response = new ResponseInfo<IEnumerable<User>>();
            try
            {
                response.Data = await _userService.GetAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = new ResponseInfo<User?>();
            try
            {
                response.Data = await _userService.GetAsync(id);

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