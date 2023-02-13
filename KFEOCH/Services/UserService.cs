using KFEOCH.Constants;
using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Identity;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using KFEOCH.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using User.Management.Service.Models;
using User.Management.Service.Services;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Web;
using MimeKit;
using Microsoft.AspNetCore.WebUtilities;
using KFEOCH.Models.Binding;

namespace KFEOCH.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<ApplicationUser> userManager,
                           RoleManager<IdentityRole> roleManager,
                           IOptions<JWT> jwt,
                           ApplicationDbContext db,
                           IConfiguration configuration,
                           IEmailService emailService,
                           IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
            _db = db;
            _configuration = configuration;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }
        public ApplicationUser GetById(string id)
        {
            return _db.Users.Find(id);
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            }
            .Union(roleClaims)
            .Union(userClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.UtcNow.AddDays(10),
                    Created = DateTime.UtcNow
                };
            }
        }

        public async Task<ResultWithMessage> AdminRegistrationAsync(AdminRegistrationModel model)
        {
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            var officeTypeId = _db.OfficeTypes.Where(o => o.IsAdmin).Select(x => x.Id).FirstOrDefault();
            var maxlicenseUser = _db.Users.Where(u => u.OfficeTypeId == officeTypeId).OrderByDescending(x => x.LicenseId).FirstOrDefault();
            var licenseId = maxlicenseUser == null ? 0 : maxlicenseUser.LicenseId;
            if (userWithSameEmail != null)
            {
                return new ResultWithMessage { Success = false, Message = "Email Already Exist!!" };
            }
            if (officeTypeId == null)
            {
                return new ResultWithMessage { Success = false, Message = "No Admin Office Found!!!" };
            }
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                LicenseId = licenseId + 1,
                OfficeTypeId = officeTypeId,
                OfficeId = null,
                IsPasswordChanged = true,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {

                await _userManager.AddToRoleAsync(user, KFEOCH.Constants.Authorization.admin_role.ToString());
                return new ResultWithMessage { Success = true, Message = $@"User {model.UserName} has been registered!" };
            }
            else
            {
                return new ResultWithMessage { Success = false, Message = result.Errors.FirstOrDefault()?.Description };
            }


        }

        private string InitialOfficeAsync(OfficeRegistrationModel model, out Office office)
        {

            var emailExist = CheckOfficeEmail(model.Email);
            var userIdExist = CheckOfficeUserId(model.OfficeTypeId.ToString(), model.LicenseId.ToString());
            var nameArabicExist = CheckOfficeNameArabic(model.NameArabic);
            var nameEglishExist = CheckOfficeNameEnglish(model.NameEnglish);
            if (emailExist.Result.Success)
            {
                office = null;
                return "Email Already Exist";
            }
            if (userIdExist.Result.Success)
            {
                office = null;
                return "License Already Exist";
            }
            if (nameArabicExist.Result.Success)
            {
                office = null;
                return "Arabic Name Already Exist";
            }
            if (nameEglishExist.Result.Success)
            {
                office = null;
                return "English Name Already Exist";
            }
            var localcountry = _configuration.GetValue<string>("LocalCountry");
            var type = _db.OfficeTypes?.Find(model.OfficeTypeId);
            var countryid = _db.Countries?.FirstOrDefault(x => x.NameEnglish == localcountry)?.Id;
            office = new Office(model);
            office.IsLocal = type?.IsLocal;
            office.CountryId = countryid;
            _db.Offices?.Add(office);
            _db.SaveChanges();
            return "";
        }

        private async Task<string> CheckOffice(OfficeRegistrationModel model)
        {

            var emailExist = await CheckOfficeEmail(model.Email);
            var userIdExist = await CheckOfficeUserId(model.OfficeTypeId.ToString(), model.LicenseId.ToString());
            var nameArabicExist = await CheckOfficeNameArabic(model.NameArabic);
            var nameEglishExist = await CheckOfficeNameEnglish(model.NameEnglish);
            if (emailExist.Success)
            {
                return "Email Already Exist";
            }
            if (userIdExist.Success)
            {
                return "License Already Exist";
            }
            if (nameArabicExist.Success)
            {
                return "Arabic Name Already Exist";
            }
            if (nameEglishExist.Success)
            {
                return "English Name Already Exist";
            }
            return "";
        }

        public async Task<ResultWithMessage> OfficeRegistrationAsync(OfficeRegistrationModel model)
        {
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userWithSameEmail != null)
            {
                return new ResultWithMessage { Success = false, Message = "Email Already Exist!!" };
            }
            var isOfficeCreated = await CheckOffice(model);
            if (!string.IsNullOrEmpty(isOfficeCreated))
            {
                return new ResultWithMessage { Success = false, Message = isOfficeCreated };
            }

            var type = _db.OfficeTypes?.Find(model.OfficeTypeId);
            var office = new Office(model);
            office.IsLocal = type?.IsLocal;
            office.IsActive = true;
            office.IsVerified = true;
            var defaultEntity = _db.OfficeEntities?.FirstOrDefault(x => x.NameEnglish.ToLower() == _configuration.GetValue<string>("DefaultEntity").ToLower());
            office.EntityId = defaultEntity.Id;
            if ((bool)type?.IsLocal)
            {
                var localcountry = _configuration.GetValue<string>("LocalCountry");
                var countryid = _db.Countries?.FirstOrDefault(x => x.NameEnglish == localcountry)?.Id;
                office.CountryId = countryid;
            }
            _db.Offices?.Add(office);
            _db.SaveChanges();
            var user = new ApplicationUser
            {
                UserName = "T" + model.OfficeTypeId + "L" + model.LicenseId,
                Email = model.Email,
                LicenseId = model.LicenseId,
                OfficeTypeId = model.OfficeTypeId,
                OfficeId = office.Id,
                IsPasswordChanged = true,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {

                await _userManager.AddToRoleAsync(user, Authorization.office_role.ToString());

                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //string tokenHtmlVersion = HttpUtility.UrlEncode(token);
                //var request = _httpContextAccessor.HttpContext.Request;
                //string link = request.Scheme + "://" +
                //    request.Host + "/api/Account/confirm-email?email=" + user.Email +
                //    "&token="+ tokenHtmlVersion;

                //var body = (
                //    "Dear Mr. / Ms. "+  model.NameEnglish +", \n" +
                //    "You have been sent this email because you created an account on our website.\n" +
                //    "Please click on <a href =\"" + link + "\"> this link </a> to confirm your email address is correct. ");
                //var message =
                //            new Message(new string[]
                //            { user.Email! }, "Confirmation Email From KFEOCH", body);
                //            _emailService.SendEmail(message);

                return new ResultWithMessage { Success = true, Message = $@"User {model.NameEnglish} has been registered!!" };
            }
            else
            {
                return new ResultWithMessage { Success = false, Message = result.Errors.FirstOrDefault().Description };
            }

        }
        public async Task<ResultWithMessage> GenerateEmailConfirmTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "User Not Found !!!",
                    MessageEnglish = "User Not Found !!!",
                    MessageArabic = "البريد الالكتروني غير موجود !!!",
                };
            }
            if (user.EmailConfirmed == true)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Already Email Confirmed !!!",
                    MessageEnglish = "Already Email Confirmed !!!",
                    MessageArabic = "البريد الالكتروني موثق سابقاً !!!",
                };
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return new ResultWithMessage { Success = true, Result = token };
        }

        public async Task<ResultWithMessage> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new ResultWithMessage { Success = false, Message = "User Not Found !!!" };

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return new ResultWithMessage { Success = false, Message = result.ToString() };
            }
            return new ResultWithMessage { Success = true, Message = $"Email {email} Has Been Confirmed." };
        }

        public async Task<AuthenticationModel> AdminLoginAsync(AdminLoginModel model)
        {
            var authenticationModel = new AuthenticationModel();
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null || !user.IsActive)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.IsEmailConfirmed = false;
                authenticationModel.Message = $@"No accounts registered with {model.UserName}";
                authenticationModel.Message = $@"لا توجد حسابات مسجلة مع {model.UserName}";
                return authenticationModel;
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authenticationModel.IsAuthenticated = true;
                authenticationModel.IsEmailConfirmed = user.EmailConfirmed;
                authenticationModel.Email = user.Email;
                authenticationModel.UserName = user.UserName;
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationModel.Roles = rolesList.ToList();
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
                authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authenticationModel.TokenDurationM = _jwt.DurationInMinutes;
                authenticationModel.TokenExpiry = jwtSecurityToken.ValidTo;
                if (user.RefreshTokens.Any(a => a.IsActive))
                {
                    var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                    authenticationModel.RefreshToken = activeRefreshToken.Token;
                    //Static Value
                    authenticationModel.RefreshTokenDurationM = 10 * 24 * 60;
                    authenticationModel.RefreshTokenExpiry = activeRefreshToken.Expires;
                }
                else
                {
                    var refreshToken = GenerateRefreshToken();
                    authenticationModel.RefreshToken = refreshToken.Token;
                    //Static Value
                    authenticationModel.RefreshTokenDurationM = 10 * 24 * 60;
                    authenticationModel.RefreshTokenExpiry = refreshToken.Expires;
                    user.RefreshTokens.Add(refreshToken);
                    _db.Update(user);
                    _db.SaveChanges();
                }

                return authenticationModel;

            }
            authenticationModel.IsAuthenticated = false;
            authenticationModel.IsEmailConfirmed = false;
            authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";
            authenticationModel.MessageArabic = $"بيانات اعتماد غير صحيحة للمستخدم {user.Email}.";
            return authenticationModel;
        }

        public async Task<AuthenticationModel> OfficeLoginAsync(OfficeLoginModel model)
        {
            var authenticationModel = new AuthenticationModel();
            var user = await _userManager.FindByNameAsync("T" + model.OfficeTypeId + "L" + model.LicenseId);

            if (user == null || !user.IsActive)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.IsEmailConfirmed = false;
                authenticationModel.Message = $@"Unavailable Or Deactivated Account!!!";
                authenticationModel.MessageArabic = $@"حساب غير متوفر أو معطل !!!";
                return authenticationModel;
            }
            var confimred = await _userManager.IsEmailConfirmedAsync(user);
            if (!confimred)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.IsEmailConfirmed = false;
                authenticationModel.Message = $@"This Account Email Not Confirmed Yet !!!";
                authenticationModel.MessageArabic = "البريد الإلكتروني للحساب لم يتم تأكيده بعد !!!";
                return authenticationModel;
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var office = _db.Offices?.FirstOrDefault(x => x.LicenseId == model.LicenseId && x.TypeId == model.OfficeTypeId);
                authenticationModel.IsAuthenticated = true;
                authenticationModel.IsEmailConfirmed = user.EmailConfirmed;
                authenticationModel.Email = user.Email;
                authenticationModel.UserName = user.UserName;
                authenticationModel.OfficeId = office.Id;
                authenticationModel.NameArabic = office.NameArabic;
                authenticationModel.NameEnglish = office.NameEnglish;
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationModel.Roles = rolesList.ToList();
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
                authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authenticationModel.TokenDurationM = _jwt.DurationInMinutes;
                authenticationModel.TokenExpiry = jwtSecurityToken.ValidTo;
                if (user.RefreshTokens.Any(a => a.IsActive))
                {
                    var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                    authenticationModel.RefreshToken = activeRefreshToken.Token;
                    //Static Value
                    authenticationModel.RefreshTokenDurationM = 10 * 24 * 60;
                    authenticationModel.RefreshTokenExpiry = activeRefreshToken.Expires;
                }
                else
                {
                    var refreshToken = GenerateRefreshToken();
                    authenticationModel.RefreshToken = refreshToken.Token;
                    //Static Value
                    authenticationModel.RefreshTokenDurationM = 10 * 24 * 60;
                    authenticationModel.RefreshTokenExpiry = refreshToken.Expires;
                    user.RefreshTokens.Add(refreshToken);
                    _db.Update(user);
                    _db.SaveChanges();
                }
                return authenticationModel;
            }
            authenticationModel.IsAuthenticated = false;
            authenticationModel.IsEmailConfirmed = false;
            authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";
            authenticationModel.MessageArabic = $"بيانات اعتماد غير صحيحة للمستخدم {user.Email}.";
            return authenticationModel;
        }

        public async Task<ResultWithMessage> AdminChangePasswordAsync(AdminChangePasswordModel model)
        {

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"No accounts registered with {model.UserName}" };
            }
            if (await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
            {
                user.IsPasswordChanged = true;
                await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                return new ResultWithMessage { Success = true, Message = $@"Password Changed for {model.UserName}" };
            }
            return new ResultWithMessage { Success = false, Message = $"Incorrect Credentials for user {user.Email}." };
        }

        public async Task<ResultWithMessage> OfficeChangePasswordAsync(OfficeChangePasswordModel model)
        {

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unavailable Or Deactivated Account!!!" };
            }
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await CanManipulateOffice(principal, user.OfficeId.Value);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
            if (await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
            {
                user.IsPasswordChanged = true;
                await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                return new ResultWithMessage { Success = true, Message = $@"Password Changed for {user.Email}" };
            }
            return new ResultWithMessage { Success = false, Message = $"Incorrect Credentials for user {user.Email}." };
        }

        public async Task<ResultWithMessage> OfficeResetPasswordAsync(OfficeResetPasswordModel model)
        {
            var user = await _userManager.FindByNameAsync("T" + model.OfficeTypeId + "L" + model.LicenseId);
            if (user == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"No accounts registered with {"T" + model.OfficeTypeId + "L" + model.LicenseId}" };
            }
            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
            user.IsPasswordChanged = false;
            if (result.Succeeded)
            {
                return new ResultWithMessage { Success = true, Message = $@"Password Reset for {user.UserName}" };
            }
            return new ResultWithMessage { Success = false, Message = result.Errors.FirstOrDefault()?.Description };
        }

        public async Task<AuthenticationModel> RefreshTokenAsync(string token)
        {
            var authenticationModel = new AuthenticationModel();
            var user = _db.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.IsEmailConfirmed = false;
                authenticationModel.Message = $"Token did not match any user !!!";
                authenticationModel.MessageArabic = "الرمز المميز لا يتطابق مع أي مستخدم !!!";
                return authenticationModel;
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.IsEmailConfirmed = false;
                authenticationModel.Message = $"Token Not Active !!!";
                authenticationModel.MessageArabic = "الرمز غير نشط !!!";
                return authenticationModel;
            }

            //Revoke Current Refresh Token
            refreshToken.Revoked = DateTime.UtcNow;

            //Generate new Refresh Token and save to Database
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            _db.Update(user);
            _db.SaveChanges();
            var office = _db.Offices?.FirstOrDefault(x => x.Id == user.OfficeId);
            //Generates new jwt
            authenticationModel.IsAuthenticated = true;
            authenticationModel.IsAuthenticated = user.EmailConfirmed;
            JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
            authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authenticationModel.Email = user.Email;
            authenticationModel.UserName = user.UserName;
            authenticationModel.NameEnglish = user.UserName;
            authenticationModel.OfficeId = office.Id;
            authenticationModel.NameArabic = office.NameArabic;
            authenticationModel.NameEnglish = office.NameEnglish;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            authenticationModel.Roles = rolesList.ToList();
            authenticationModel.TokenDurationM = _jwt.DurationInMinutes;
            authenticationModel.RefreshToken = newRefreshToken.Token;
            authenticationModel.RefreshTokenDurationM = 10 * 24 * 60;
            authenticationModel.RefreshTokenExpiry = newRefreshToken.Expires;

            return authenticationModel;
        }

        public async Task<ResultWithMessage> DeactivateAccountAsync(string userId)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return new ResultWithMessage() { Success = false, Message = $@"No accounts registered with {user.UserName}" };
            }
            var isSuperuser = await IsSuperuser(user.Id);
            if (isSuperuser)
            {
                return new ResultWithMessage() { Success = false, Message = $@"Unauthorized" };
            }
            user.IsActive = false;
            RevokeTokenById(userId);
            _db.Update(user);
            _db.SaveChanges();
            return new ResultWithMessage() { Success = true, Message = $@"Account {user.UserName} Deactivated !!!" };
        }

        public async Task<ResultWithMessage> ActivateAccountAsync(string userId)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return new ResultWithMessage() { Success = false, Message = $@"No accounts registered with {user.UserName}" };
            }
            var isSuperuser = await IsSuperuser(user.Id);
            if (isSuperuser)
            {
                return new ResultWithMessage() { Success = false, Message = $@"Unauthorized" };
            }
            user.IsActive = true;
            _db.Update(user);
            _db.SaveChanges();
            return new ResultWithMessage() { Success = true, Message = $@"Account {user.UserName} Deactivated !!!" };
        }

        public async Task<ResultWithMessage> RevokeToken(string token)
        {
            var user = _db.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            // return false if no user found with token
            if (user == null)
            {
                new ResultWithMessage() { Success = false, Message = $@"No accounts registered with {user.UserName}" };
            }

            var isSuperuser = await IsSuperuser(user.Id);
            if (isSuperuser)
            {
                return new ResultWithMessage() { Success = false, Message = $@"Unauthorized" };
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            // return false if token is not active
            if (!refreshToken.IsActive)
            {
                new ResultWithMessage() { Success = false, Message = $@"Refresh Token ins not acive!!" };
            }
            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            _db.Update(user);
            _db.SaveChanges();
            return new ResultWithMessage() { Success = true, Message = $@"Revoke Token Succeeded!!" };
        }

        public async Task<ResultWithMessage> RevokeTokenById(string userId)
        {
            var user = _db.Users.Include(u => u.RefreshTokens).FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                new ResultWithMessage() { Success = false, Message = $@"No accounts registered with {user.UserName} !!!" };
            }
            var isSuperuser = await IsSuperuser(user.Id);
            if (isSuperuser)
            {
                return new ResultWithMessage() { Success = false, Message = $@"Unauthorized" };
            }
            foreach (var refreshToken in user.RefreshTokens)
            {
                if (refreshToken.IsActive)
                {
                    refreshToken.Revoked = DateTime.UtcNow;
                }
            }
            _db.Update(user);
            _db.SaveChanges();
            return new ResultWithMessage() { Success = true, Message = $@"Revoke Token Succeeded !!!" };
        }

        public async Task<ResultWithMessage> AddNewRole(string roleName)
        {
            var existRole = await _roleManager.FindByNameAsync(roleName);
            if (existRole != null)
            {
                return new ResultWithMessage() { Success = false, Message = $@"Role {roleName} Is Not Exist !!!" };
            }
            var role = new IdentityRole(roleName);
            var identityRole = await _roleManager.CreateAsync(role);
            if (identityRole != null)
            {
                return new ResultWithMessage() { Success = true, Message = $@"Role {roleName} Has Been Created !!!" };
            }
            return new ResultWithMessage() { Success = false, Message = $@"Role {roleName} Is Not Created !!!" };
        }

        public async Task<ResultWithMessage> CheckOfficeUserId(string officeTypeId, string LicenseId)
        {
            var user = await _userManager.FindByNameAsync("T" + officeTypeId + "L" + LicenseId);
            if (user == null)
            {
                return new ResultWithMessage { Success = false };
            }
            return new ResultWithMessage { Success = true };
        }

        public async Task<ResultWithMessage> CheckUserName(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return new ResultWithMessage { Success = false };
            }
            return new ResultWithMessage { Success = true };
        }

        public async Task<ResultWithMessage> CheckOfficeNameArabic(string nameArabic)
        {
            var user = _db.Offices?.FirstOrDefault(x => x.NameArabic.ToLower() == nameArabic.ToLower());
            if (user == null)
            {
                return new ResultWithMessage { Success = false };
            }
            return new ResultWithMessage { Success = true };
        }

        public async Task<ResultWithMessage> CheckOfficeNameEnglish(string nameEnglish)
        {
            var user = _db.Offices?.FirstOrDefault(x => x.NameEnglish.ToLower() == nameEnglish.ToLower());
            if (user == null)
            {
                return new ResultWithMessage { Success = false };
            }
            return new ResultWithMessage { Success = true };
        }

        public async Task<ResultWithMessage> CheckOfficeEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var office = _db.Offices?.FirstOrDefault(x => x.Email == email);
            if (user == null && office == null)
            {
                return new ResultWithMessage { Success = false };
            }
            return new ResultWithMessage { Success = true };
        }

        public async Task<bool> CanManipulateOffice(ClaimsPrincipal user, int officeId)
        {
            if (user.IsInRole("Administrator") || user.IsInRole("SuperUser") || user.IsInRole("OfficeManager"))
            {
                return true;
            }
            var appUser = await _userManager.FindByNameAsync(user.Identity.Name);
            return appUser.OfficeId == officeId;
        }
       
        public async Task<ResultWithMessage> ForgetPassword(ForgetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "User Not Found",
                    MessageEnglish = "User Not Found",
                    MessageArabic = "المستخدم غير موجود"
                };
            }

            var isSuperuser = await IsSuperuser(user.Id);
            if (isSuperuser)
            {
                return new ResultWithMessage() { Success = false, Message = $@"Unauthorized" };
            }

            var office = _db.Offices.FirstOrDefault(x => x.Id == user.OfficeId);
            if (office == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Office Not Found",
                    MessageEnglish = "Office Not Found",
                    MessageArabic = "المكتب غير موجود"
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (token == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "You Can't Reset Your Password Now, Please Try Again Later",
                    MessageEnglish = "You Can't Reset Your Password Now, Please Try Again Later",
                    MessageArabic = "لا يمكنك إعادة تعيين كلمة المرور الخاصة بك الآن ، يرجى المحاولة مرة أخرى لاحقًا"
                };
            }

            var param = new Dictionary<string, string?>
                                {
                                    {"token", token },
                                    {"email", model.Email }
                                };
            var callback = QueryHelpers.AddQueryString(model.ClientUri, param);

            var body = @$"<table style='border-collapse: collapse; width: 100%;'>
                            <tbody>
                            <tr>
                            <td colspan='2'><img style='width: 30%;' src='https://kfeoch-api.techteec.net/logos/logo-horizontal.png' alt='' /></td>
                            </tr>
                            <tr>
                            <td colspan='2'>
                            <h4>Dear Mr. / Ms. {office.NameEnglish},&nbsp;</h4>
                            </td>
                            </tr>
                            <tr>
                            <td colspan='2'>You have been sent this email because you request Forget Password with this email account.<br />Please click on <a href='{callback}'>this link </a>to reset your account password.</td>
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
                        new Message(new string[]
                        { model.Email! }, "Reset Password From Kfeoch", bodybuilder.ToMessageBody());
            _emailService.SendEmail(message);
            return new ResultWithMessage
            {
                Success = true
            };

        }

        public async Task<ResultWithMessage> ResetPassword(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "User Not Found",
                    MessageEnglish = "User Not Found",
                    MessageArabic = "المستخدم غير موجود"
                };
            }

            var isSuperuser = await IsSuperuser(user.Id);
            if (isSuperuser)
            {
                return new ResultWithMessage() { Success = false, Message = $@"Unauthorized" };
            }
            var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!resetPassResult.Succeeded)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "You Can't Reset Your Password Now, Please Try Again Later",
                    MessageEnglish = "You Can't Reset Your Password Now, Please Try Again Later",
                    MessageArabic = "لا يمكنك إعادة تعيين كلمة المرور الخاصة بك الآن ، يرجى المحاولة مرة أخرى لاحقًا"
                };
            }
            return new ResultWithMessage
            {
                Success = true
            };
        }

        public async Task<ResultWithMessage> GetAllUsers(ClaimsPrincipal user)
        {
            var list = new List<UserViewModel>();
            var officeTypeId = _db.OfficeTypes.Where(o => o.IsAdmin).Select(x => x.Id).FirstOrDefault();
            var users = _db.Users.Where(x => x.OfficeTypeId == officeTypeId && x.UserName != user.Identity.Name).ToList();
            foreach (var u in users)
            {
                
                var v = new UserViewModel();
                v.Id = u.Id;
                v.UserName = u.UserName;
                v.Email = u.Email;
                v.IsActive = u.IsActive;
                v.Roles = (await _userManager.GetRolesAsync(u));
                list.Add(v);
                var isSuperuser = await IsSuperuser(u.Id);
                if (isSuperuser)
                {
                    list.Remove(v);
                }
            }
            return new ResultWithMessage { Success = true, Result = list };

        }

        public ResultWithMessage GetAllRoles()
        {
            var roles = _db.Roles.Where(x => x.Name != "SuperUser" && x.Name != "Office").ToList();
            return new ResultWithMessage
            {
                Success = true,
                Result = roles
            };
        }


        public async Task<ResultWithMessage> GetUserInfo(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "User Not Found",
                    MessageEnglish = "User Not Found",
                    MessageArabic = "مستخدم غير موجود",

                };
            }

            var isSuperuser = await IsSuperuser(user.Id);
            if (isSuperuser)
            {
                return new ResultWithMessage() { Success = false, Message = $@"Unauthorized" };
            }
            var userview = new UserViewModel();
            userview.Id = user.Id;
            userview.UserName = user.UserName;
            userview.Email = user.Email;
            userview.IsActive = user.IsActive;
            userview.Roles = (await _userManager.GetRolesAsync(user));
            return new ResultWithMessage { Success = true, Result = userview };

        }

        public async Task<ResultWithMessage> PutUserRoles(string id,UserWithRoles model)
        {
            if (id != model.Id)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Invalid Model",
                    MessageEnglish = "Invalid Model",
                    MessageArabic = "نموذج غير صالح",

                };
            }
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "User Not Found",
                    MessageEnglish = "User Not Found",
                    MessageArabic = "مستخدم غير موجود",

                };
            }

            var isSuperuser = await IsSuperuser(user.Id);
            if (isSuperuser)
            {
                return new ResultWithMessage() { Success = false, Message = $@"Unauthorized" };
            }

            var oldroles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, oldroles);
            await _userManager.AddToRolesAsync(user, model.Roles);
            var userview = new UserViewModel();
            userview.Id = user.Id;
            userview.UserName = user.UserName;
            userview.Email = user.Email;
            userview.Roles = (await _userManager.GetRolesAsync(user));
            return new ResultWithMessage { Success = true, Result = userview };
        }

        public async Task<ResultWithMessage> UserResetPasswordAsync(UserResetPasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "User Not Found",
                    MessageEnglish = "User Not Found",
                    MessageArabic = "مستخدم غير موجود",

                };
            }

            var isSuperuser = await IsSuperuser(user.Id);
            if (isSuperuser)
            {
                return new ResultWithMessage() { Success = false, Message = $@"Unauthorized" };
            }
            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
            user.IsPasswordChanged = false;
            if (result.Succeeded)
            {
                return new ResultWithMessage { Success = true, Message = $@"Password Reset for {user.UserName}" };
            }
            return new ResultWithMessage { Success = false, Message = result.Errors.FirstOrDefault()?.Description };
        }

        public async Task<ResultWithMessage> AddUserWithRolesAsync(AdminRegistrationModel model)
        {
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            var officeTypeId = _db.OfficeTypes.Where(o => o.IsAdmin).Select(x => x.Id).FirstOrDefault();
            var maxlicenseUser = _db.Users.Where(u => u.OfficeTypeId == officeTypeId).OrderByDescending(x => x.LicenseId).FirstOrDefault();
            var licenseId = maxlicenseUser == null ? 0 : maxlicenseUser.LicenseId;
            if (userWithSameEmail != null)
            {
                return new ResultWithMessage { Success = false, Message = "Email Already Exist!!" };
            }
            if (officeTypeId == null)
            {
                return new ResultWithMessage { Success = false, Message = "No Admin Office Found!!!" };
            }

            if (model.Roles.Contains("SuperUser"))
            {
                return new ResultWithMessage { Success = false, Message = "Unauthorized" };
            }
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                LicenseId = licenseId + 1,
                OfficeTypeId = officeTypeId,
                OfficeId = null,
                IsPasswordChanged = true,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {

                await _userManager.AddToRolesAsync(user, model.Roles);
                return new ResultWithMessage { Success = true, Message = $@"User {model.UserName} has been registered!" };
            }
            else
            {
                return new ResultWithMessage { Success = false, Message = result.Errors.FirstOrDefault()?.Description };
            }


        }

        public async Task<bool> IsAuthorized(ClaimsPrincipal user, string[] roles)
        {
            var dbuser = await _userManager.FindByNameAsync(user.Identity.Name);
            if (!dbuser.IsActive)
            {
                return false;
            }
            if (roles.FirstOrDefault(x => user.IsInRole(x)) == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsSuperuser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("SuperUser"))
            {
                return true;
            }
           
            return false;
        }
    }
}
