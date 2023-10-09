using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Environment
{
    [ApiController]
    [AllowAnonymous]
    [Route("v1/[controller]")]
    [Tags("Environment")]
    public class EnvironmentController : ControllerBase
    {
        public EnvironmentController()
        {
            
        }

        [HttpGet]
        [Route("Test")]
        public async Task<IActionResult> Test()
        {
            var response = new ResponseInfo<string>();
            try
            {
                response.Data = "Connected";

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
