using Api.Auxiliary;
using Api.Domain.ComplexEntityAndAggregates;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.Controllers.ComplexEntityAndAggregates
{
    [ApiController]
    //[Authorize]
    [EnableRateLimiting("fixed2")]
    [Route("v1/[controller]")]
    [Tags("Complex Principal Entity - Test")]
    public sealed class ComplexPrincipalController(IComplexPrincipalService complexPrincipalService) : ControllerBase
    {
        private readonly IComplexPrincipalService _complexPrincipalService = complexPrincipalService;

        //[Authorization(true)]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddAsync(ComplexPrincipal complexPrincipal)
        {
            var response = new ResponseInfo<ComplexPrincipal>();
            try
            {
                response.Data = await _complexPrincipalService.AddAsync(complexPrincipal);

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        //[Authorization(true)]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateAsync(ComplexPrincipal complexPrincipal)
        {
            var response = new ResponseInfo<ComplexPrincipal>();
            try
            {
                response.Data = await _complexPrincipalService.UpdateAsync(complexPrincipal);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        //[Authorization(true)]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = new ResponseInfo();
            try
            {
                await _complexPrincipalService.DeleteAsync(id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        //[Authorization(true)]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAsync()
        {
            var response = new ResponseInfo<IList<ComplexPrincipal>>();
            try
            {
                response.Data = await _complexPrincipalService.GetAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        //[Authorization(true)]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = new ResponseInfo<ComplexPrincipal?>();
            try
            {
                response.Data = await _complexPrincipalService.GetAsync(id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        //[Authorization(true)]
        [HttpGet]
        [Route("{page:int}/{pageSize:int:max(10)}/{search?}")]
        public async Task<IActionResult> GetAsync(int page, int pageSize, string search = "")
        {
            var response = new ResponseInfo<IList<ComplexPrincipal>>();
            try
            {
                response.Data = await _complexPrincipalService.GetAsync(page, pageSize, search);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        //[Authorization(true)]
        [HttpPost]
        [Route("GenerateRandom/{quantity:int}/{returnOnlyLast:bool?}")]
        public async Task<IActionResult> GenerateRandom(int quantity, bool returnOnlyLast = false)
        {
            var response = new ResponseInfo<IList<ComplexPrincipal>>();
            try
            {
                response.Data = await _complexPrincipalService.GenerateRandom(quantity);
                int totalCount = response.Data.Count + response.Data.Sum(d => d.ComplexAggregates.Count + d.ComplexAggregates.Sum(d2 => d2.ComplexSubAggregates.Count));
                response.Message = $"Generated {response.Data.Count} (considering aggregates = {totalCount}) entity(ies).";

                if (returnOnlyLast)
                {
                    response.Data = response.Data.TakeLast(1).ToList();
                    response.Message += " Returned last.";
                }

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }
    }
}
