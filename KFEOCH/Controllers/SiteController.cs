using KFEOCH.Models.Views;
using KFEOCH.Services;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KFEOCH.Services;
using KFEOCH.Models;


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
        [HttpPost("Articles")]
        public IActionResult GetPublishedArticles(FilterModel model)
        {
            var result = _siteService.GetPublishedArticles(model);
            if (result == null)
            {
                return BadRequest(new { message = "No Article Found!!!" });
            }
            return Ok(result);
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
    }
}
