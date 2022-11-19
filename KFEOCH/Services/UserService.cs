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

namespace KFEOCH.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly ApplicationDbContext _db;
        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, ApplicationDbContext db)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
            _db = db;
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
            var maxlicenseUser= _db.Users.Where(u => u.OfficeTypeId == officeTypeId).OrderByDescending(x => x.LicenseId).FirstOrDefault();
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

                await _userManager.AddToRoleAsync(user, Authorization.admin_role.ToString());
                return new ResultWithMessage { Success = true, Message = $@"User {model.UserName} has been registered!" };
            }
            return new ResultWithMessage { Success = false, Message = result.Errors.FirstOrDefault()?.Description };

        }

        private string InitialOfficeAsync(OfficeRegistrationModel model, out Office office)
        {

            var emailExist =  CheckOfficeEmail(model.Email);
            var userIdExist =  CheckOfficeUserId(model.OfficeTypeId.ToString(), model.LicenseId.ToString());
            var nameArabicExist =  CheckOfficeNameArabic(model.NameArabic);
            var nameEglishExist =  CheckOfficeNameEnglish(model.NameEnglish);
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
            office = new Office(model);
            _db.Offices.Add(office);
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
            var office = new Office(model);
            _db.Offices.Add(office);
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
                return new ResultWithMessage { Success = true, Message = $@"User { model.NameEnglish } has been registered!!"};
            }
            return new ResultWithMessage { Success = false, Message = result.Errors.FirstOrDefault().Description };

        }

        public async Task<AuthenticationModel> AdminLoginAsync(AdminLoginModel model)
        {
            var authenticationModel = new AuthenticationModel();
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null || !user.IsActive)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = $@"No accounts registered with {model.UserName}";
                return authenticationModel;
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authenticationModel.IsAuthenticated = true;
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
            authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";
            return authenticationModel;
        }

        public async Task<AuthenticationModel> OfficeLoginAsync(OfficeLoginModel model)
        {
            var authenticationModel = new AuthenticationModel();
            var user = await _userManager.FindByNameAsync("T" + model.OfficeTypeId + "L" + model.LicenseId);
            if (user == null || !user.IsActive)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = $@"Unavailable Or Deactivated Account!!!";
                return authenticationModel;
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var office = _db.Offices?.FirstOrDefault(x => x.LicenseId == model.LicenseId && x.TypeId == model.OfficeTypeId);
                authenticationModel.IsAuthenticated = true;
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
            authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";
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

            var user = await _userManager.FindByNameAsync("T" + model.OfficeTypeId + "L" + model.LicenseId);
            if (user == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unavailable Or Deactivated Account!!!" };
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
                authenticationModel.Message = $"Token did not match any users.";
                return authenticationModel;
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = $"Token Not Active.";
                return authenticationModel;
            }

            //Revoke Current Refresh Token
            refreshToken.Revoked = DateTime.UtcNow;

            //Generate new Refresh Token and save to Database
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            _db.Update(user);
            _db.SaveChanges();

            //Generates new jwt
            authenticationModel.IsAuthenticated = true;
            JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
            authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authenticationModel.Email = user.Email;
            authenticationModel.NameEnglish = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            authenticationModel.Roles = rolesList.ToList();
            authenticationModel.TokenDurationM = _jwt.DurationInMinutes;
            authenticationModel.RefreshToken = newRefreshToken.Token;
            authenticationModel.RefreshTokenDurationM = 10 * 24 * 60;
            authenticationModel.RefreshTokenExpiry = newRefreshToken.Expires;
            return authenticationModel;
        }

        public ResultWithMessage DeactivateAccountAsync(string userId)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return new ResultWithMessage() { Success = false, Message = $@"No accounts registered with {user.UserName}" };
            }
            user.IsActive = false;
            RevokeTokenById(userId);
            _db.Update(user);
            _db.SaveChanges();
            return new ResultWithMessage() { Success = true, Message = $@"Account {user.UserName} Deactivated !!!" };


        }

        public ResultWithMessage RevokeToken(string token)
        {
            var user = _db.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            // return false if no user found with token
            if (user == null)
            {
                new ResultWithMessage() { Success = false, Message = $@"No accounts registered with {user.UserName}" };
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

        public ResultWithMessage RevokeTokenById(string userId)
        {
            var user = _db.Users.Include(u => u.RefreshTokens).FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                new ResultWithMessage() { Success = false, Message = $@"No accounts registered with {user.UserName} !!!" };
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

        public async Task<ResultWithMessage > AddNewRole(string roleName)
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
                return new ResultWithMessage { Success = false};
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
    }
}
