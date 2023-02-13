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
    public class OfficeActivityController : ControllerBase
    {
        private readonly IOfficeActivityService _officeActivityService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string[] roles;
        public OfficeActivityController(IOfficeActivityService officeActivityService,
                                        IUserService userService,
                                        IHttpContextAccessor httpContextAccessor)
        {
            _officeActivityService = officeActivityService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            roles = new string[] { "SuperUser", "Administrator", "Office", "OfficeManager" };
        }
        [HttpPost]
        public async Task<IActionResult> PostOfficeActivityAsync(OfficeActivityBindingModel model)
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
            var result = await _officeActivityService.PostOfficeActivityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOfficeActivityAsync(int id)
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
            var result = await _officeActivityService.DeleteOfficeActivityAsync(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetOfficeById(int officeId)
        {
            var result = _officeActivityService.GetOfficeActivities(officeId);
            if (result == null)
            {
                return BadRequest(new { message = "No Activity Found!!!" });
            }
            return Ok(result);
        }
    }
}
