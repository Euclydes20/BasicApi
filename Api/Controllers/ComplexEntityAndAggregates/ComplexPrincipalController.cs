using Api.Auxiliary;
using Api.Domain.ComplexEntityAndAggregates;
using Api.Domain.Users;
using Api.Models;
using Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers.ComplexEntityAndAggregates
{
    [ApiController]
    //[Authorize]
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
    }
}
