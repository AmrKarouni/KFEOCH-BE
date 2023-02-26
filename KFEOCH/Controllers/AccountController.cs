using KFEOCH.Models.Binding;
using KFEOCH.Models.Identity;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Web;
using User.Management.Service.Models;
using User.Management.Service.Services;

namespace KFEOCH.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "SuperUser,Administrator")]
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

        [AllowAnonymous]
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
                
                //var body = (
                //        "<h3>Dear Mr. / Ms. " + model.NameEnglish + ", </h3><br>" +
                //        "You have been sent this email because you created an account on our website.<br>" +
                //        "Please click on <a href =\"" + confirmationLink + "\"> this link </a> to confirm your email address is correct.<br> Thank You, ");


                var body = @$"<table style='border-collapse: collapse; width: 100%;'>
                            <tbody>
                            <tr>
                            <td colspan='2'><img style='width: 30%;' src='https://kfeoch-api.techteec.net/logos/logo-horizontal.png' alt='' /></td>
                            </tr>
                            <tr>
                            <td colspan='2'>
                            <h4>Dear Mr. / Ms. {model.NameEnglish},&nbsp;</h4>
                            </td>
                            </tr>
                            <tr>
                            <td colspan='2'>You have been sent this email because you created an account on our website.<br />Please click on <a href='{confirmationLink}'>this link </a>to confirm your email address is correct.</td>
                            </tr>
                            <tr>
                            <td colspan='2'>
                            <p>Thank You ,</p>
                            <h4>KFEOCH Team</h4>
                            </td>
                            </tr>
                            </tbody>
                            </table>";

                var bodybuilder = new BodyBuilder();
                bodybuilder.HtmlBody = body;
                var ms = bodybuilder.ToMessageBody();
                var message =
                            new Message (new string[]
                            { model.Email! }, "Confirmation Email From KFEOCH", bodybuilder.ToMessageBody());
                _emailService.SendEmail(message);
            }
            
            return Ok(result);
        }

        [AllowAnonymous]
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
            var bodybuilder = new BodyBuilder();
            bodybuilder.HtmlBody = body;
            var ms = bodybuilder.ToMessageBody();
            var message =
                             new Message(new string[]
                             { email! }, "Confirmation Email From KFEOCH", bodybuilder.ToMessageBody());
            _emailService.SendEmail(message);

            return Ok();
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("office-login")]
        public async Task<ActionResult> OfficeLoginAsync(OfficeLoginModel model)
        {
            var result = await _userService.OfficeLoginAsync(model);
            if (result.IsAuthenticated && result.IsEmailConfirmed)
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

        [Authorize(Roles = "Administrator,OfficeManager,DictionaryManager,SiteManager,ReportManager,BillingManager")]
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

        [Authorize(Roles = "SuperUser,Administrator,Office")]
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

        [Authorize(Roles = "SuperUser,Administrator")]
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

        [AllowAnonymous]
        [HttpPost("refresh-token-cookies")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _userService.RefreshTokenAsync(refreshToken);
            if (!string.IsNullOrEmpty(response.RefreshToken))
                SetRefreshTokenInCookie(response.RefreshToken);
            return Ok(response);
        }
        
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenParam(RefreshTokenBindingModel refreshToken)
        {
            var response = await _userService.RefreshTokenAsync(refreshToken.Token.ToString());
            if (!string.IsNullOrEmpty(response.RefreshToken))
                SetRefreshTokenInCookie(response.RefreshToken);
            return Ok(response);
        }

        [Authorize(Roles = "SuperUser,Administrator")]
        [HttpPost("tokens/{id}")]
        public IActionResult GetRefreshTokens(string id)
        {
            var user = _userService.GetById(id);
            return Ok(user.RefreshTokens);
        }

        [Authorize(Roles = "SuperUser,Administrator")]
        [HttpGet("deactivate-account/{id}")]
        public async Task<IActionResult> DeactivateAccountAsync(string id)
        {
            var user = _userService.GetById(id);
            if(user == null)
            {
                return BadRequest(new { message = "User Id is required" });
            }
            var response = await _userService.DeactivateAccountAsync(id);
            if (!response.Success)
            {
                return BadRequest(response.Success);
               
            }
            return Ok(response.Success);

        }

        [Authorize(Roles = "SuperUser,Administrator")]
        [HttpGet("activate-account/{id}")]
        public async Task<IActionResult> ActivateAccountAsync(string id)
        {
            var user = _userService.GetById(id);
            if (user == null)
            {
                return BadRequest(new { message = "User Id is required" });
            }
            var response = await _userService.ActivateAccountAsync(id);
            if (!response.Success)
            {
                return BadRequest(response.Success);

            }
            return Ok(response.Success);
        }


        [Authorize(Roles = "SuperUser,Administrator")]
        [HttpGet("revoke-token/{id}")]
        public async Task<IActionResult> RevokeToken(string id)
        {
            // accept token from request body or cookie
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { message = "Id is required" });
            var response = await _userService.RevokeTokenById(id);
            if (!response.Success)
                return NotFound(new { message = "User not found" });
            return Ok(response);
        }

        [Authorize(Roles = "SuperUser,Administrator")]
        [HttpGet("addnewrole/{roleName}")]
        public async Task<IActionResult> AddNewRole(string roleName)
        {
            return Ok(await _userService.AddNewRole(roleName));
        }

        [AllowAnonymous]
        [HttpGet("check-userid")]
        public async Task<IActionResult> CheckUserId(string officeTypeId,string licenseId)
        {
            var response = await _userService.CheckOfficeUserId(officeTypeId, licenseId);
            return Ok(response.Success);
        }

        [AllowAnonymous]
        [HttpGet("check-username")]
        public async Task<IActionResult> CheckUserName(string username)
        {
            var response = await _userService.CheckUserName(username);
            return Ok(response.Success);
        }

        [AllowAnonymous]
        [HttpGet("check-namearabic")]
        public async Task<IActionResult> CheckOfficeNameArabic(string value)
        {
            var response = await _userService.CheckOfficeNameArabic(value);
            return Ok(response.Success);
        }
        
        [AllowAnonymous]
        [HttpGet("check-nameenglish")]
        public async Task<IActionResult> CheckOfficeNameEnglish(string value)
        {
            var response = await _userService.CheckOfficeNameEnglish(value);
            return Ok(response.Success);
        }
        
        [AllowAnonymous]
        [HttpGet("check-email")]
        public async Task<IActionResult> CheckUserId(string value)
        {
            var response = await _userService.CheckOfficeEmail(value);
            return Ok(response.Success);
        }

        [AllowAnonymous]
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Model");
            var result = await _userService.ForgetPassword(model);

            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Success);
        }


        [Authorize(Roles = "Administrator,OfficeManager,DictionaryManager,SiteManager,ReportManager,BillingManager,Office")]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Model");
            var result = await _userService.ResetPassword(model);

            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Success);
        }


        [Authorize(Roles = "SuperUser,Administrator")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers(User);
            return Ok(result.Result);
        }

        [Authorize(Roles = "SuperUser,Administrator")]
        [HttpGet("roles")]
        public IActionResult GetAllRoles()
        {
            var result = _userService.GetAllRoles();
            return Ok(result.Result);
        }

        [Authorize(Roles = "SuperUser,Administrator")]
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserInfo(string id)
        {
            var result = await _userService.GetUserInfo(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [Authorize(Roles = "SuperUser,Administrator")]
        [HttpPut("users/{id}")]
        public async Task<IActionResult> PutUserRoles(string id, UserWithRoles model)
        {
            var result = await _userService.PutUserRoles(id,model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [Authorize(Roles = "SuperUser,Administrator")]
        [HttpPost("user-resetpassword")]
        public async Task<ActionResult> UserResetPasswordAsync(UserResetPasswordModel model)
        {
            var result = await _userService.UserResetPasswordAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message});
            }
            return Ok(result.Success);
        }

        [Authorize(Roles = "SuperUser,Administrator")]
        [HttpPost("add-user")]
        public async Task<ActionResult> AddUserWithRolesAsync(AdminRegistrationModel model)
        {
            var result = await _userService.AddUserWithRolesAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            
            return Ok(result.Success);
        }

    }
}
