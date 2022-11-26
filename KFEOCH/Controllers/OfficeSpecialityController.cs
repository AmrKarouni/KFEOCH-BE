using KFEOCH.Models.Binding;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KFEOCH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeSpecialityController : ControllerBase
    {
        private readonly IOfficeSpecialityService _officeSpecialityService;

        public OfficeSpecialityController(IOfficeSpecialityService officeSpecialityService)
        {
            _officeSpecialityService = officeSpecialityService;
        }
        [HttpPost]
        public async Task<IActionResult> PostOfficeSpecialityAsync(OfficeSpecialityBindingModel model)
        {
            var result = await _officeSpecialityService.PostOfficeSpecialityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOfficeSpecialityAsync(int id)
        {
            var result = await _officeSpecialityService.DeleteOfficeSpecialityAsync(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        [HttpGet]
        public IActionResult GetOfficeById(int officeId)
        {
            var result = _officeSpecialityService.GetOfficeSpecialities(officeId);
            if (result == null)
            {
                return BadRequest(new { message = "No Speciality Found!!!" });
            }
            return Ok(result);
        }
    }
}
