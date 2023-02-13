using KFEOCH.Models.Views;
using KFEOCH.Services;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KFEOCH.Services;
using KFEOCH.Models;
using KFEOCH.Models.Site;
using System.Data;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace KFEOCH.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly ISiteService _siteService;
        private readonly ISiteMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string[] roles;
        public SiteController(ISiteService siteService,
                              ISiteMessageService messageService,
                              IUserService userService,
                              IHttpContextAccessor httpContextAccessor)
        {
            _siteService = siteService;
            _messageService = messageService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            roles = new string[] { "SuperUser", "Administrator", "SiteManager" };
        }

        [AllowAnonymous]
        [HttpPost("Offices")]
        public IActionResult GetOffices(FilterModel model)
        {
            var result = _siteService.GetOffices(model);
            if (result == null)
            {
                return BadRequest(new { message = "No Office Found!!!" });
            }
            return Ok(result.Result);
        }

        [AllowAnonymous]
        [HttpGet("Office/{id}")]
        public IActionResult GetOffices(int id)
        {
            var result = _siteService.GetOfficeForGuest(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [AllowAnonymous]
        [HttpGet("pages")]
        public IActionResult GetAllPages()
        {
            var result = _siteService.GetAllPages();
            if (result == null)
            {
                return BadRequest(new { message = "No Page Found!!!" });
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("pages/{url}")]
        public IActionResult GetPageByUrl(string url)
        {
            var result = _siteService.GetPageByUrl(url);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }


        [HttpPut("pages/{id}")]
        public async Task<IActionResult> PutPage(int id, PageBindingModel model)
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


            var result = _siteService.PutPage(id, model);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPost("pages/upload-image")]
        public async Task<IActionResult> UploadPageImage([FromForm] ImageModel model)
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


            var result = await _siteService.UploadPageImage(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [AllowAnonymous]
        [HttpGet("posts/{id}")]
        public IActionResult GetPostById(int id)
        {
            var result = _siteService.GetPostById(id);
            if (result.Success == false)
            {
                return BadRequest(new { message = "No Post Found!!!" });
            }
            return Ok(result.Result);
        }

        [HttpPost("posts")]
        public async Task<IActionResult> AddPostAsync(PostBindingModel model)
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


            var result = await _siteService.AddPostAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPost("post-title")]
        public async Task<IActionResult> AddPostTitleAsync(PostTitleBindingModel model)
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

            var result = await _siteService.AddPostTitleAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("posts/{id}")]
        public async Task<IActionResult> PutPost(int id, PostBindingModel model)
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


            var result = _siteService.PutPost(id, model);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpDelete("posts/{id}")]
        public async Task<IActionResult> DeletePost(int id)
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

            var result = await _siteService.DeletePost(id);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPost("posts/upload-image")]
        public async Task<IActionResult> UploadPostImage([FromForm] ImageModel model)
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


            var result = await _siteService.UploadPostImage(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpGet("posts/reorder-post")]
        public async Task<IActionResult> ReorderPost(int pageId, int previousIndex, int currentIndex)
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


            var result = _siteService.ReorderPost(pageId, previousIndex, currentIndex);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [AllowAnonymous]
        [HttpGet("sections/{id}")]
        public IActionResult GetSectionById(int id)
        {
            var result = _siteService.GetSectionById(id);
            if (result.Success == false)
            {
                return BadRequest(new { message = "No Section Found!!!" });
            }
            return Ok(result.Result);
        }

        [HttpPost("sections")]
        public async Task<IActionResult> AddSectionAsync(SectionBindingModel model)
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


            var result = await _siteService.AddSectionAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        [HttpPost("sections-with-image")]
        public async Task<IActionResult> AddSectionWithImageAsync([FromForm] SectionWithImageBindingModel model)
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


            var result = await _siteService.AddSectionWithImageAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        //[HttpPut("sections/{id}")]
        //public IActionResult PutSection(int id, SectionBindingModel model)
        //{
        //    var result = _siteService.PutSection(id, model);
        //    if (result.Success == false)
        //    {
        //        return BadRequest(new { message = result.Message });
        //    }
        //    return Ok(result.Result);
        //}

        [HttpDelete("sections/{id}")]
        public async Task<IActionResult> DeleteSection(int id)
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

            var result = await _siteService.DeleteSection(id);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPost("sections/upload-image")]
        public async Task<IActionResult> UploadSectionImage([FromForm] ImageModel model)
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


            var result = await _siteService.UploadSectionImage(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpGet("sections/reorder-section")]
        public async Task<IActionResult> ReorderSection(int postId, int previousIndex, int currentIndex)
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


            var result = _siteService.ReorderSection(postId, previousIndex, currentIndex);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPost("posts/filter")]
        public async Task<IActionResult> GetPostsByFilter(FilterModel model)
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


            var result = _siteService.GetPostsByFilter(model);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [AllowAnonymous]
        [HttpGet("last-news")]
        public IActionResult GetLastNews()
        {
            var result = _siteService.GetLastNews();
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        //////////////////////////////////

        [HttpPost("messages/filter")]
        public async Task<IActionResult> GetAllMessagesWithFilter(FilterModel model)
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


            var result = _messageService.GetAllMessagesWithFilter(model);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);

        }

        [HttpGet("unread-messages")]
        public async Task<IActionResult> GetAllUnredMessages()
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


            var result = _messageService.GetAllUnredMessages();
            if (result == null)
            {
                return BadRequest(new { message = "No Unread Message Found!!!" });
            }
            return Ok(result);
        }

        [HttpGet("messages/{id}")]
        public async Task<IActionResult> GetMessageById(int id)
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


            var result = _messageService.GetMessageById(id);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [AllowAnonymous]
        [HttpPost("messages")]
        public async Task<IActionResult> AddMessageAsync(SiteMessageBindingModel model)
        {
            var result = await _messageService.AddMessageAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpGet("messages/read/{id}")]
        public async Task<IActionResult> MarkMessageAsRead(int id)
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


            var result = _messageService.MarkMessageAsRead(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpGet("messages/unread/{id}")]
        public async Task<IActionResult> MarkMessageAsUnread(int id)
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


            var result = _messageService.MarkMessageAsUnread(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
    }
}
