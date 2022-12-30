using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KFEOCH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeLicensesController : ControllerBase
    {
        private readonly IOfficeRegistrationService _officeRegistrationService;
        public OfficeLicensesController(IOfficeRegistrationService officeRegistrationService)
        {
            _officeRegistrationService = officeRegistrationService;
        }


        //[HttpGet("fees/{id}")]
        //public IActionResult GetRnewFieldsByOfficeId(int id)
        //{
        //    var result = _officeRegistrationService.GetRnewFieldsByOfficeId(id);
        //    if (!result.Success)
        //    {
        //        return BadRequest(new { message = result.Message});
        //    }
        //    return Ok(result.Result);
        //}

        [HttpPost]
        public IActionResult PostLicense(License model)
        {
            var result = _officeRegistrationService.PostLicense(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("{id}")]
        public IActionResult PutLicense(int id,License model)
        {
            var result = _officeRegistrationService.PutLicense(id,model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpGet("{id}")]
        public IActionResult GetLicenseById(int id)
        {
            var result = _officeRegistrationService.GetLicenseById(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpGet("Office/{id}")]
        public IActionResult GetLicenseByOfficeId(int id)
        {
            var result = _officeRegistrationService.GetLicenseByOfficeId(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPost("upload-document")]
        public async Task<IActionResult> UploadDocument([FromForm] FileModel model)
        {
            var result = await _officeRegistrationService.UploadDocument(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpGet("Document/View/{id}")]
        public IActionResult ViewDocumentByUrl(int id)
        {
            var result = _officeRegistrationService.GetDocument(id);
            if (result.Bytes == null)
            {
                return BadRequest(new { message = "Bad Request" });
            }
            return File(result.Bytes, result.ContentType, result.FileName);
        }
        [HttpGet("admin-licenses")]
        public IActionResult GetAllPendingLicenses()
        {
            var result = _officeRegistrationService.GetAllPendingLicenses();
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpGet("first-fees")]
        public IActionResult CalculationFeesForNewOffice(int officeid)
        {
            var result = _officeRegistrationService.CalculationFeesForNewOffice(officeid);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
    }
}
