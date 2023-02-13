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
    public class OfficeContactController : ControllerBase
    {
        private readonly IOfficeContactService _officeContactService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string[] roles;
        public OfficeContactController(IOfficeContactService officeContactService,
                                       IUserService userService,
                                       IHttpContextAccessor httpContextAccessor)
        {
            _officeContactService = officeContactService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            roles = new string[] { "SuperUser", "Administrator", "Office", "OfficeManager" };
        }
        [HttpPost]
        public async Task<IActionResult> PostOfficeContactAsync(OfficeContactBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid Model" });
            }

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
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid Model" });
            }
                
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
            var result = await _officeContactService.DeleteOfficeContactAsync(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [AllowAnonymous]
        [HttpGet("{officeid}")]
        public IActionResult GetOfficeById(int officeid)
        {
            var result = _officeContactService.GetAllOfficeContactsByOfficeId(officeid);
            if (result == null)
            {
                return BadRequest(new { message = "No Contact Found!!!" });
            }
            return Ok(result);
        }
    }
}
