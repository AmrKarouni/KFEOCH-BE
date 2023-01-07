using KFEOCH.Models.Identity;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Management.Service.Models;
using User.Management.Service.Services;

namespace KFEOCH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public AccountController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }
        //[Authorize(Roles = "Admin, SuperUser")]
        [HttpPost("admin-register")]
        public async Task<ActionResult> AdminRegistrationAsync(AdminRegistrationModel model)
        {
            var result = await _userService.AdminRegistrationAsync(model);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message});
            }
            if (result == null)
            {
                return BadRequest(new { message = "Email Already Exist" });
            }
            return Ok(result);
        }
        [HttpPost("office-register")]
        public async Task<ActionResult> OfficeRegistrationAsync(OfficeRegistrationModel model)
        {
            if (!ModelState.IsValid )
            {
                return BadRequest(ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList());
            }
            var result = await _userService.OfficeRegistrationAsync(model);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            if (result == null)
            {
                return BadRequest(new { message = "Email Already Exist" });
            }
            if (result.Success)
            {
                var res = await _userService.GenerateEmailConfirmTokenAsync(model.Email);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token = res.Result, email = model.Email }, Request.Scheme);
                var body = (
                        "Dear Mr. / Ms. " + model.NameEnglish + ", \n" +
                        "You have been sent this email because you created an account on our website.\n" +
                        "Please click on <a href =\"" + confirmationLink + "\"> this link </a> to confirm your email address is correct. ");
                var message =
                            new Message(new string[]
                            { model.Email! }, "Confirmation Email From KFEOCH", body);
                _emailService.SendEmail(message);
            }
            
            return Ok(result);
        }

        [HttpGet("resend-confirm-email")]
        public async Task<ActionResult> ResendConfirmEmail(string email)
        {
            var generator = await _userService.GenerateEmailConfirmTokenAsync(email);
            if (!generator.Success)
            {
                return BadRequest(generator.Message);
            }
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token = generator.Result, email = email }, Request.Scheme);
            var body = (
                    "Dear Mr. / Ms. , \n" +
                    "You have been sent this email because you created an account on our website.\n" +
                    "Please click on <a href =\"" + confirmationLink + "\"> this link </a> to confirm your email address is correct. ");
            var message =
                        new Message(new string[]
                        { email! }, "Confirmation Email From KFEOCH", body);
            _emailService.SendEmail(message);

            return Ok();
        }


        [HttpGet("confirm-email")]
        public async Task<ActionResult> ConfirmEmail(string token,string email)
        {
            var result = await _userService.ConfirmEmail(token,email);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Message);
        }


        [HttpPost("admin-login")]
        public async Task<ActionResult> AdminLoginAsync(AdminLoginModel model)
        {
            var result = await _userService.AdminLoginAsync(model);
            if (result.IsAuthenticated)
            {
                SetRefreshTokenInCookie(result.RefreshToken);
            }
            return Ok(result);
        }

        [HttpPost("office-login")]
        public async Task<ActionResult> OfficeLoginAsync(OfficeLoginModel model)
        {
            var result = await _userService.OfficeLoginAsync(model);
            if (result.IsAuthenticated)
            {
                SetRefreshTokenInCookie(result.RefreshToken);
            }
            return Ok(result);
        }
        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(10)
            };
            Response.Cookies.Append("refreshToken", refreshToken.ToString(), cookieOptions);
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

        [HttpGet("check-userid")]
        public async Task<IActionResult> CheckUserId(string officeTypeId,string licenseId)
        {
            var response = await _userService.CheckOfficeUserId(officeTypeId, licenseId);
            return Ok(response.Success);
        }
        [HttpGet("check-namearabic")]
        public async Task<IActionResult> CheckOfficeNameArabic(string value)
        {
            var response = await _userService.CheckOfficeNameArabic(value);
            return Ok(response.Success);
        }
        [HttpGet("check-nameenglish")]
        public async Task<IActionResult> CheckOfficeNameEnglish(string value)
        {
            var response = await _userService.CheckOfficeNameEnglish(value);
            return Ok(response.Success);
        }
        [HttpGet("check-email")]
        public async Task<IActionResult> CheckUserId(string value)
        {
            var response = await _userService.CheckOfficeEmail(value);
            return Ok(response.Success);
        }

        
    }
}
