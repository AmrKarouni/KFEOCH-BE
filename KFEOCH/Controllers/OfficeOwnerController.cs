using KFEOCH.Models;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KFEOCH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeOwnerController : ControllerBase
    {
        private readonly IOfficeOwnerService _officeOwnerService;
        public OfficeOwnerController(IOfficeOwnerService officeOwnerService)
        {
            _officeOwnerService = officeOwnerService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _officeOwnerService.GetById(id);
            if (result == null)
            {
                return BadRequest(new { message = "No Owner Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostOfficeOwnerAsync(OfficeOwner model)
        {
            var result = await _officeOwnerService.PostOfficeOwnerAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        [HttpPut("{id}")]
        public IActionResult PutOfficeOwnerAsync(int id, OfficeOwner model)
        {
            var result = _officeOwnerService.PutOfficeOwnerAsync(id, model);
            if (!result.Result.Success)
            {
                return BadRequest(new
                {
                    message = result.Result.Message
                });
            }
            return Ok(result.Result.Result);
        }
        [HttpDelete("{id}")]
        public IActionResult PutOffice(int id)
        {
            var result = _officeOwnerService.DeleteOfficeOwnerAsync(id);
            if (!result.Result.Success)
            {
                return BadRequest(new
                {
                    message = result.Result.Message
                });
            }
            return Ok(result.Result.Result);
        }
    }
}
