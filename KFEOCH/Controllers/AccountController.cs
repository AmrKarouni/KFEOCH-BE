using KFEOCH.Models.Identity;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KFEOCH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        //[Authorize(Roles = "Admin, SuperUser")]
        [HttpPost("admin-register")]
        public async Task<ActionResult> AdminRegistrationAsync(AdminRegistrationModel model)
        {
            var result = await _userService.AdminRegistrationAsync(model);
            if (result == null)
            {
                return BadRequest(new { message = "Email Already Exist" });
            }
            return Ok(result);
        }
        [HttpPost("office-register")]
        public async Task<ActionResult> OfficeRegistrationAsync(OfficeRegistrationModel model)
        {
            var result = await _userService.OfficeRegistrationAsync(model);
            if (result == null)
            {
                return BadRequest(new { message = "Email Already Exist" });
            }
            return Ok(result);
        }
        [HttpPost("admin-login")]
        public async Task<ActionResult> AdminLoginAsync(AdminLoginModel model)
        {
            var result = await _userService.AdminLoginAsync(model);
            SetRefreshTokenInCookie(result.RefreshToken);
            return Ok(result);
        }

        [HttpPost("office-login")]
        public async Task<ActionResult> OfficeLoginAsync(OfficeLoginModel model)
        {
            var result = await _userService.OfficeLoginAsync(model);
            SetRefreshTokenInCookie(result.RefreshToken);
            return Ok(result);
        }
        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(10)
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        [HttpPost("admin-changepassword")]
        public async Task<ActionResult> AdminChangePasswordAsync(AdminChangePasswordModel model)
        {
            var result = await _userService.AdminChangePasswordAsync(model);
            if (result == null)
            {
                return BadRequest(new { message = "Change Password Failed!!!" });
            }
            return Ok(result);
        }

        [HttpPost("office-changepassword")]
        public async Task<ActionResult> OfficeChangePasswordAsync(OfficeChangePasswordModel model)
        {
            var result = await _userService.OfficeChangePasswordAsync(model);
            if (result == null)
            {
                return BadRequest(new { message = "Change Password Failed!!!" });
            }
            return Ok(result);
        }
        //[Authorize(Roles = "Admin, SuperUser")]
        [HttpPost("office-resetpassword")]
        public async Task<ActionResult> OfficeResetPasswordAsync(OfficeResetPasswordModel model)
        {
            var result = await _userService.OfficeResetPasswordAsync(model);
            if (result == null)
            {
                return BadRequest(new { message = "Reset Password Failed!!!" });
            }
            return Ok(result);
        }

        [HttpPost("refresh-token-cookies")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _userService.RefreshTokenAsync(refreshToken);
            if (!string.IsNullOrEmpty(response.RefreshToken))
                SetRefreshTokenInCookie(response.RefreshToken);
            return Ok(response);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenParam(RefreshTokenBindingModel refreshToken)
        {
            var response = await _userService.RefreshTokenAsync(refreshToken.Token.ToString());
            if (!string.IsNullOrEmpty(response.RefreshToken))
                SetRefreshTokenInCookie(response.RefreshToken);
            return Ok(response);
        }

        //[Authorize(Roles = "Admin, Developer")]
        [HttpPost("tokens/{id}")]
        public IActionResult GetRefreshTokens(string id)
        {
            var user = _userService.GetById(id);
            return Ok(user.RefreshTokens);
        }

        //[Authorize(Roles = "Admin, Developer")]
        [HttpPost("deactivate-account")]
        public IActionResult DeactivateAccountAsync(string userId)
        {
            var user = _userService.GetById(userId);
            if(user == null)
            {
                return BadRequest(new { message = "User Id is required" });
            }
            var response = _userService.DeactivateAccountAsync(userId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(new { message = "Bad Request" });
        }


        //[Authorize(Roles = "Admin, Developer")]
        [HttpPost("revoke-token")]
        public IActionResult RevokeToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            if (string.IsNullOrEmpty(model.Id))
                return BadRequest(new { message = "Id is required" });
            var response = _userService.RevokeTokenById(model.Id);
            if (!response.Success)
                return NotFound(new { message = "User not found" });
            return Ok(response);
        }

        //[Authorize(Roles = "Admin, SuperUser")]
        [HttpGet("addnewrole/{roleName}")]
        public async Task<IActionResult> AddNewRole(string roleName)
        {
            return Ok(await _userService.AddNewRole(roleName));
        }
    }
}
