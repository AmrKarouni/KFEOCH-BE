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

        [HttpPost]
        public IActionResult PostLicense(License model)
        {
            var result = _officeRegistrationService.PostLicense(model);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    messageArabic = result.MessageArabic,
                    messageEnglish = result.MessageEnglish
                });
            }
            return Ok(result.Result);
        }

        [HttpPut("ApproveLicense/{id}")]
        public IActionResult ApproveLicense(int id, License model)
        {
            var result = _officeRegistrationService.ApproveLicense(id, model);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    messageArabic = result.MessageArabic,
                    messageEnglish = result.MessageEnglish
                });
            }
            return Ok(result.Result);
        }

        [HttpPut("PutLicense/{id}")]
        public IActionResult PutLicense(int id, License model)
        {
            var result = _officeRegistrationService.PutLicense(id, model);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    messageArabic = result.MessageArabic,
                    messageEnglish = result.MessageEnglish
                });
            }
            return Ok(result.Result);
        }

        [HttpGet("GetLicenseById/{id}")]
        public IActionResult GetLicenseById(int id)
        {
            var result = _officeRegistrationService.GetLicenseById(id);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    messageArabic = result.MessageArabic,
                    messageEnglish = result.MessageEnglish
                });
            }
            return Ok(result.Result);
        }

        [HttpGet("Office/{id}")]
        public IActionResult GetLicenseByOfficeId(int id)
        {
            var result = _officeRegistrationService.GetLicenseByOfficeId(id);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    messageArabic = result.MessageArabic,
                    messageEnglish = result.MessageEnglish
                });
            }
            return Ok(result.Result);
        }

        [HttpPost("upload-document")]
        public async Task<IActionResult> UploadDocument([FromForm] FileModel model)
        {
            var result = await _officeRegistrationService.UploadDocument(model);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    messageArabic = result.MessageArabic,
                    messageEnglish = result.MessageEnglish
                });
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
                return BadRequest(new
                {
                    message = "Bad Request !!!",
                    messageArabic = "Bad Request !!!",
                    messageEnglish = "طلب خاطئ !!!"
                });
            }
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpGet("GetPendingLicenses")]
        public IActionResult GetAllPendingLicenses()
        {
            var result = _officeRegistrationService.GetAllPendingLicenses();
            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    messageArabic = result.MessageArabic,
                    messageEnglish = result.MessageEnglish
                });
            }
            return Ok(result.Result);
        }

        [HttpGet("GetPayments/{id}")]
        public IActionResult CalculationFeesForNewOffice(int id)
        {
            var result = _officeRegistrationService.CalculationFeesForNewOffice(id);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    messageArabic = result.MessageArabic,
                    messageEnglish = result.MessageEnglish
                });
            }
            return Ok(result.Result);
        }

        [HttpPost("GetPaymentsByLicense")]
        public IActionResult CalculationFeesForNewOfficeByLisense(License model)
        {
            var result = _officeRegistrationService.CalculationFeesForNewOfficeByLisense(model);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    messageArabic = result.MessageArabic,
                    messageEnglish = result.MessageEnglish
                });
            }
            return Ok(result.Result);
        }

        [HttpGet("RejectLicense/{id}")]
        public async Task<ActionResult> RejectLicense(int id)
        {
            var result = await _officeRegistrationService.RejectLicense(id);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    messageArabic = result.MessageArabic,
                    messageEnglish = result.MessageEnglish
                });
            }
            return Ok(result.Success);
        }

        [HttpGet("GetRenewPayments/{id}")]
        public IActionResult CalculationFeesForRenew(int id)
        {
            var result = _officeRegistrationService.CalculationFeesForRenew(id, false);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    messageArabic = result.MessageArabic,
                    messageEnglish = result.MessageEnglish
                });
            }
            return Ok(result.Result);
        }

        [HttpPost("PostRenewPayments/{id}")]
        public IActionResult PostFeesForRenew(int id)
        {
            var result = _officeRegistrationService.CalculationFeesForRenew(id, true);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    messageArabic = result.MessageArabic,
                    messageEnglish = result.MessageEnglish
                });
            }
            return Ok(result.Result);
        }

        [HttpGet("GetAllOfficePayments/{id}")]
        public IActionResult GetAllOfficePayments(int id)
        {
            var result = _officeRegistrationService.GetAllOfficePayments(id);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    messageArabic = result.MessageArabic,
                    messageEnglish = result.MessageEnglish
                });
            }
            return Ok(result.Result);
        }
    }
}
