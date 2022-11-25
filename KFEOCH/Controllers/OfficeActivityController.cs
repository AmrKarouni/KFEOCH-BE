using KFEOCH.Models.Binding;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KFEOCH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeActivityController : ControllerBase
    {
        private readonly IOfficeActivityService _officeActivityService;

        public OfficeActivityController(IOfficeActivityService officeActivityService)
        {
            _officeActivityService = officeActivityService;
        }
        [HttpPost]
        public async Task<IActionResult> PostOfficeActivityAsync(OfficeActivityBindingModel model)
        {
            var result = await _officeActivityService.PostOfficeActivityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOfficeActivityAsync(OfficeActivityBindingModel model)
        {
            var result = await _officeActivityService.DeleteOfficeActivityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        [HttpGet]
        public IActionResult GetOfficeById(int officeId)
        {
            var result = _officeActivityService.GetOfficeActivities(officeId);
            if (result == null)
            {
                return BadRequest(new { message = "No Activity Found!!!" });
            }
            return Ok(result);
        }
    }
}
