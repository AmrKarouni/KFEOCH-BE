using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KFEOCH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeContactController : ControllerBase
    {
        private readonly IOfficeContactService _officeContactService;

        public OfficeContactController(IOfficeContactService officeContactService)
        {
            _officeContactService = officeContactService;
        }
        [HttpPost]
        public async Task<IActionResult> PostOfficeContactAsync(OfficeContactBindingModel model)
        {
            var result = await _officeContactService.PostOfficeContactAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOfficeContactAsync(int id,OfficeContact model)
        {
            var result = await _officeContactService.PutOfficeContactAsync(id,model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOfficeContactAsync(int id)
        {
            var result = await _officeContactService.DeleteOfficeContactAsync(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        [HttpGet("{officeid}")]
        public IActionResult GetOfficeById(int officeId)
        {
            var result = _officeContactService.GetAllOfficeContactsByOfficeId(officeId);
            if (result == null)
            {
                return BadRequest(new { message = "No Contact Found!!!" });
            }
            return Ok(result);
        }
    }
}
