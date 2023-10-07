using Api.Domain.Annotations;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Annotations
{
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("v1/[controller]")]
    [Tags("Annotations")]
    public class AnnotationController : ControllerBase
    {
        private readonly IAnnotationService _annotationService;

        public AnnotationController(IAnnotationService annotationService)
        {
            _annotationService = annotationService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddAsync(Annotation annotation)
        {
            var response = new ResponseInfo<Annotation>();
            try
            {
                response.Data = await _annotationService.AddAsync(annotation);

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
        public async Task<IActionResult> UpdateAsync(Annotation annotation)
        {
            var response = new ResponseInfo<Annotation>();
            try
            {
                response.Data = await _annotationService.UpdateAsync(annotation);

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
            var response = new ResponseInfo<Annotation>();
            try
            {
                await _annotationService.RemoveAsync(id);
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
            var response = new ResponseInfo<IEnumerable<Annotation>>();
            try
            {
                response.Data = await _annotationService.GetAsync();

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
            var response = new ResponseInfo<Annotation>();
            try
            {
                response.Data = await _annotationService.GetAsync(id);

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
