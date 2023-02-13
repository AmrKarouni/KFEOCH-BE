using KFEOCH.Models;
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
    public class OfficeRequestController : ControllerBase
    {
        private readonly IOfficeRequestService _officeRequestService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string[] roles;
        public OfficeRequestController(IOfficeRequestService officeRequestService,
                                        IUserService userService,
                                        IHttpContextAccessor httpContextAccessor)
        {
            _officeRequestService = officeRequestService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            roles = new string[] { "SuperUser", "Administrator", "Office", "OfficeManager" };
        }

        [HttpPost]
        public async Task<IActionResult> InitialOfficeRequestAsync(OfficeRequestBindingModel model)
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
            var result = await _officeRequestService.InitialOfficeRequestAsync(model);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequestById(int id)
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
            var result = await _officeRequestService.GetRequestById(id);
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

        //// Admin
        //[HttpGet("Admin")]
        //public ActionResult GetAllPendingRequests()
        //{
        //    var result = _officeRequestService.GetAllPendingRequests();
        //    if (!result.Success)
        //    {
        //        return BadRequest(new {
        //                                Message = result.Message,
        //                                MessageEnglish = result.MessageEnglish,
        //                                MessageArabic = result.MessageArabic });
        //    }
        //    return Ok(result.Result);
        //}


        [HttpGet("GetAllOfficeRequests/{id}")]
        public async Task<IActionResult> GetAllRequestsByOfficeId(int id)
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

            var result = await _officeRequestService.GetAllRequestsByOfficeId(id);
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

        [HttpGet("GenerateRequestReceipt/{id}")]
        public async Task<IActionResult> GenerateRequestReceipt(int id)
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
            var result = await _officeRequestService.GenerateRequestReceipt(id);
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


        [HttpGet("GetAllOfficeRequestPayments/{id}")]
        public async Task<IActionResult> GetAllOfficeRequestPayments(int id)
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
            var result = await _officeRequestService.GetAllOfficeRequestPayments(id);
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

        [HttpGet("GetRequestReceipt/{id}")]
        public async Task<IActionResult> GetRequestReceipt(int id)
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
            var result = await _officeRequestService.GetRequestReceipt(id);
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

        [HttpGet("GetPaymentReceipt/{id}")]
        public async Task<IActionResult> GetPaymentReceipt(int id)
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

        [HttpGet("GenerateRequestCertificate/{id}")]
        public async Task<IActionResult> GenerateRequestCertificate(int id)
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
            var result = await _officeRequestService.GenerateRequestCertificate(id);
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


        [HttpGet("GetRequestCertificate/{id}")]
        public async Task<IActionResult> GetRequestCertificate(int id)
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
            var result = await _officeRequestService.GetRequestCertificate(id);
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

        //[HttpPut("{id}")]
        //public ActionResult PutRequest(int id,OfficeRequest model)
        //{
        //    var result = _officeRequestService.PutRequest(id,model);
        //    if (!result.Success)
        //    {
        //        return BadRequest(new
        //        {
        //            Message = result.Message,
        //            MessageEnglish = result.MessageEnglish,
        //            MessageArabic = result.MessageArabic
        //        });
        //    }
        //    return Ok(result.Result);
        //}
    }
}
