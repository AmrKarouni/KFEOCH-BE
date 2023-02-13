using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Identity;
using KFEOCH.Models.Views;
using System.Security.Claims;

namespace KFEOCH.Services.Interfaces
{
    public interface IUserService
    {
        ApplicationUser GetById(string id);
        Task<ResultWithMessage> AdminRegistrationAsync(AdminRegistrationModel model);
        Task<ResultWithMessage> OfficeRegistrationAsync(OfficeRegistrationModel model);
        Task<ResultWithMessage> GenerateEmailConfirmTokenAsync(string email);
        Task<AuthenticationModel> AdminLoginAsync(AdminLoginModel model);
        Task<AuthenticationModel> OfficeLoginAsync(OfficeLoginModel model);
        Task<ResultWithMessage> AdminChangePasswordAsync(AdminChangePasswordModel model);
        Task<ResultWithMessage> OfficeChangePasswordAsync(OfficeChangePasswordModel model);
        Task<ResultWithMessage> OfficeResetPasswordAsync(OfficeResetPasswordModel model);
        Task<AuthenticationModel> RefreshTokenAsync(string token);
        Task<ResultWithMessage> DeactivateAccountAsync(string userId);
        Task<ResultWithMessage> ActivateAccountAsync(string userId);
        Task<ResultWithMessage> RevokeToken(string token);
        Task<ResultWithMessage> RevokeTokenById(string userId);
        Task<ResultWithMessage> AddNewRole(string roleName);
        Task<ResultWithMessage> CheckOfficeUserId(string officeTypeId, string LicenseId);
        Task<ResultWithMessage> CheckUserName(string username);
        Task<ResultWithMessage> CheckOfficeNameArabic(string nameArbic);
        Task<ResultWithMessage> CheckOfficeNameEnglish(string nameEnglish);
        Task<ResultWithMessage> CheckOfficeEmail(string email);
        Task<ResultWithMessage> ConfirmEmail(string token, string email);
        Task<bool> CanManipulateOffice(ClaimsPrincipal user, int officeId);
        Task<ResultWithMessage> ForgetPassword(ForgetPasswordModel model);
        Task<ResultWithMessage> ResetPassword(ResetPasswordModel model);
        Task<ResultWithMessage> GetAllUsers(ClaimsPrincipal user);
        ResultWithMessage GetAllRoles();
        Task<ResultWithMessage> GetUserInfo(string id);
        Task<ResultWithMessage> PutUserRoles(string id, UserWithRoles model);
        Task<ResultWithMessage> UserResetPasswordAsync(UserResetPasswordModel model);
        Task<ResultWithMessage> AddUserWithRolesAsync(AdminRegistrationModel model);
        Task<bool> IsAuthorized(ClaimsPrincipal user, string[] roles);
        Task<bool> IsSuperuser(string id);
    } 
}
