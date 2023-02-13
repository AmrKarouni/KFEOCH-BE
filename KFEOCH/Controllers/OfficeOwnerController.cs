using KFEOCH.Models;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace KFEOCH.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeOwnerController : ControllerBase
    {
        private readonly IOfficeOwnerService _officeOwnerService;
        private readonly IOwnerDocumentService _ownerDocumentService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string[] roles;
        public OfficeOwnerController(IOfficeOwnerService officeOwnerService,
                                     IOwnerDocumentService ownerDocumentService,
                                     IUserService userService,
                                     IHttpContextAccessor httpContextAccessor)
        {
            _officeOwnerService = officeOwnerService;
            _ownerDocumentService = ownerDocumentService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            roles = new string[] { "SuperUser", "Administrator", "Office", "OfficeManager" };
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _officeOwnerService.GetById(id);
            if (result.Success == false)
            {
                return BadRequest(new { message = "No Owner Found!!!" });
            }
            return Ok(result.Result);
        }

        [HttpPost]
        public async Task<IActionResult> PostOfficeOwnerAsync(OfficeOwner model)
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
            var result = await _officeOwnerService.PostOfficeOwnerAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOfficeOwnerAsync(int id, OfficeOwner model)
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
        public async Task<IActionResult> DeleteOfficeOwnerAsync(int id)
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

        [HttpGet("Document/{ownerid}")]
        public async Task<IActionResult> GetAllDocumentsByOwnerId(int ownerid)
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
            var result = _ownerDocumentService.GetAllDocumentsByOwnerId(ownerid);
            if (result == null)
            {
                return BadRequest(new { message = "No Owner Found!!!" });
            }
            return Ok(result);
        }

        [HttpGet("Document/View/{documentid}")]
        public async Task<IActionResult> ViewDocumentByUrl(int documentid)
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
            var result = _ownerDocumentService.GetDocument(documentid);
            if (result.Bytes == null)
            {
                return BadRequest(new { message = "Bad Request" });
            }
            return File(result.Bytes, result.ContentType,result.FileName);
        }

        [HttpGet("Form/View/{typeid}")]
        public async Task<IActionResult> GetForm(int typeid)
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
            var result = _ownerDocumentService.GetForm(typeid);
            if (result.Bytes == null)
            {
                return BadRequest(new { message = "Bad Request" });
            }
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpPost("Document")]
        public async Task<IActionResult> PostOwnerDocumentAsync([FromForm] OwnerFileModel model)
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
            var result = await _ownerDocumentService.PostOwnerDocumentAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpDelete("Document/{documentid}")]
        public async Task<IActionResult> DeleteDocumentAsync(int documentid)
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
            var result = await _ownerDocumentService.DeleteDocumentAsync(documentid);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Success);
        }
    }
}
