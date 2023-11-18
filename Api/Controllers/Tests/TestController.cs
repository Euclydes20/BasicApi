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

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("AddWithEF/{quantity}/{multipleAdd?}")]
        public async Task<IActionResult> AddRandomWithEFAsync(int quantity, bool multipleAdd = false)
        {
            var response = new ResponseInfo<IList<Test>>();
            try
            {
                response.Message = "50 primeiros e últimos registros adicionados.";
                response.Data = await _testService.AddRandomWithEFAsync(quantity, multipleAdd);
                response.Data = response.Data.Take(50).Concat(response.Data.TakeLast(50)).ToList();

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
        [Route("AddWithLQ/{quantity}/{multipleAdd?}")]
        public async Task<IActionResult> AddRandomWithLQAsync(int quantity, bool multipleAdd = false)
        {
            var response = new ResponseInfo<IList<Test>>();
            try
            {
                response.Message = "50 primeiros e últimos registros adicionados.";
                response.Data = await _testService.AddRandomWithLQAsync(quantity, multipleAdd);
                response.Data = response.Data.Take(50).Concat(response.Data.TakeLast(50)).ToList();

                return StatusCode(StatusCodes.Status201Created, response);
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
