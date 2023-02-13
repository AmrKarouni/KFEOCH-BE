using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KFEOCH.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeLicensesController : ControllerBase
    {
        private readonly IOfficeRegistrationService _officeRegistrationService;
        private readonly IOfficeRequestService _officeRequestService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string[] roles;
        private readonly string[] adminroles;
        public OfficeLicensesController(IOfficeRegistrationService officeRegistrationService,
                                        IOfficeRequestService officeRequestService,
                                        IUserService userService,
                                        IHttpContextAccessor httpContextAccessor)
        {
            _officeRegistrationService = officeRegistrationService;
            _officeRequestService = officeRequestService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            roles = new string[] { "SuperUser", "Administrator", "Office", "OfficeManager" };
            adminroles = new string[] { "SuperUser", "Administrator", "OfficeManager" };
        }

        [HttpPost]
        public async Task<IActionResult> PostLicense(License model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _officeRegistrationService.PostLicense(model);
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
        public async Task<IActionResult> ApproveLicense(int id, License model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, adminroles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }

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
        public async Task<IActionResult> PutLicense(int id, License model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }

            var result = await _officeRegistrationService.PutLicense(id, model);
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
        public async Task<IActionResult> GetLicenseById(int id)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
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
        public async Task<IActionResult> GetLicenseByOfficeId(int id)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
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
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
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
        public async Task<IActionResult> ViewDocumentByUrl(int id)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _officeRegistrationService.GetDocument(id);
            if (result.Bytes == null)
            {
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
        public async Task<IActionResult> GetAllPendingLicenses()
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, adminroles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
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
        public async Task<IActionResult> CalculationFeesForNewOffice(int id)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
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
        public async Task<IActionResult> CalculationFeesForNewOfficeByLisense(License model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _officeRegistrationService.CalculationFeesForNewOfficeByLisense(model);
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
        public async Task<IActionResult> RejectLicense(int id)
        {

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, adminroles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
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
        public async Task<IActionResult> CalculationFeesForRenew(int id)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _officeRegistrationService.CalculationFeesForRenew(id,false,"en","");
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

        [HttpPost("PostRenewPayments")]
        public async Task<IActionResult> PostFeesForRenew(RenewPaymentBindingModel model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _officeRegistrationService.CalculationFeesForRenew(model.id, true, model.lang, model.returnUrl);
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
        public async Task<IActionResult> GetAllOfficePayments(int id)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
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

        [HttpGet("GetAllOfficeRenewPayments/{id}")]
        public async Task<IActionResult> GetAllOfficeRenewPayments(int id)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _officeRegistrationService.GetAllOfficeRenewPayments(id);
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


        [HttpGet("GenerateRenewReceipt/{id}")]
        public async Task<IActionResult> GenerateRenewReceipt(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _officeRegistrationService.GenerateRenewReceipt(id);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    Message = result.Message,
                    MessageEnglish = result.MessageEnglish,
                    MessageArabic = result.MessageArabic
                });
            }
            return Ok(result.Result);
        }

        [HttpGet("GetPaymentReceipt/{id}")]
        public async Task<IActionResult> GetPaymentReceipt(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _officeRequestService.GetPaymentReceipt(id);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    Message = result.Message,
                    MessageEnglish = result.MessageEnglish,
                    MessageArabic = result.MessageArabic
                });
            }
            return Ok(result.Result);
        }
    }
}
