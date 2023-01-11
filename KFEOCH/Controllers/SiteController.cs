using KFEOCH.Models.Views;
using KFEOCH.Services;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KFEOCH.Services;
using KFEOCH.Models;
using KFEOCH.Models.Site;

namespace KFEOCH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly ISiteService _siteService;
        public SiteController(ISiteService siteService)
        {
            _siteService = siteService;
        }
 

        [HttpPost("Offices")]
        public IActionResult GetOffices(FilterModel model)
        {
            var result = _siteService.GetOffices(model);
            if (result == null)
            {
                return BadRequest(new { message = "No Office Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("post-types")]
        public async Task<IActionResult> AddPostTypeAsync(PostType model)
        {
            var result = await _siteService.AddPostTypeAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpGet("post-types")]
        public IActionResult GetAllPostTypes()
        {
            var result = _siteService.GetAllPostTypes();
            
            return Ok(result);
        }

        [HttpDelete("post-types/{id}")]
        public async Task<IActionResult> DeletePostTypeAsync(int id)
        {
            var result = await _siteService.DeletePostTypeAsync(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPost("posts")]
        public async Task<IActionResult> AddPos([FromForm] PostBindingModel model)
        {
            var result = await _siteService.AddPostAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        [HttpGet("posts/{id}")]
        public IActionResult GetPostById(int id)
        {
            var result = _siteService.GetPostById(id);
            if (result == null)
            {
                return BadRequest(new { message = "No Post Found!!!" });
            }
            return Ok(result);
        }

        [HttpGet("all-posts/{id}")]
        public IActionResult GetAllPostsByTypeId(int id)
        {
            var result = _siteService.GetAllPostsByTypeId(id);
            if (result == null)
            {
                return BadRequest(new { message = "No Post Found!!!" });
            }
            return Ok(result);
        }
    }
}
