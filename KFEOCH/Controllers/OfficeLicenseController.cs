using KFEOCH.Models.Binding;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KFEOCH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeLicenseController : ControllerBase
    {
        private readonly IOfficeRegistrationService _officeRegistrationService;
        public OfficeLicenseController(IOfficeRegistrationService officeRegistrationService)
        {
            _officeRegistrationService = officeRegistrationService;
        }


        [HttpGet("fees/{id}")]
        public IActionResult GetRnewFieldsByOfficeId(int id)
        {
            var result = _officeRegistrationService.GetRnewFieldsByOfficeId(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message});
            }
            return Ok(result.Result);
        }

        [HttpPost("renew-office")]
        public IActionResult RenewOffice(OfficeRenewBindingModel model)
        {
            var result = _officeRegistrationService.RenewOffice(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }


    }
}
