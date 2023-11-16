using Api.Domain.Tests;
using Api.Models;
using Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Tests
{
    [ApiController]
    [Authorize]
    [Route("v1/[controller]")]
    [Tags("Tests")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("DeleteAllWithEF")]
        public async Task<IActionResult> DeleteAllWithEFAsync()
        {
            var response = new ResponseInfo<int>();
            try
            {
                response.Data = await _testService.DeleteAllWithEFAsync();
                response.Message = $"Deleted {response.Data} registers.";

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("DeleteAllWithLQ")]
        public async Task<IActionResult> DeleteAllWithLQAsync()
        {
            var response = new ResponseInfo<int>();
            try
            {
                response.Data = await _testService.DeleteAllWithLQAsync();
                response.Message = $"Deleted {response.Data} registers.";

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("AddWithEF")]
        public async Task<IActionResult> AddWithEFAsync(Test test)
        {
            var response = new ResponseInfo<Test>();
            try
            {
                response.Data = await _testService.AddWithEFAsync(test);

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("DeleteWithEF/{testId}")]
        public async Task<IActionResult> DeleteWithEFAsync(int testId)
        {
            var response = new ResponseInfo<object>();
            try
            {
                await _testService.DeleteWithEFAsync(testId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("AddWithLQ")]
        public async Task<IActionResult> AddWithLQAsync(Test test)
        {
            var response = new ResponseInfo<Test>();
            try
            {
                response.Data = await _testService.AddWithLQAsync(test);

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return BadRequest(response);
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("DeleteWithLQ/{testId}")]
        public async Task<IActionResult> DeleteWithLQAsync(int testId)
        {
            var response = new ResponseInfo<object>();
            try
            {
                await _testService.DeleteWithLQAsync(testId);

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
