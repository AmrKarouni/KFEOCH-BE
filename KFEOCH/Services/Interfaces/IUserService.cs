using KFEOCH.Models;
using KFEOCH.Models.Identity;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IUserService
    {
        ApplicationUser GetById(string id);
        Task<ResultWithMessage> AdminRegistrationAsync(AdminRegistrationModel model);
        Task<ResultWithMessage> OfficeRegistrationAsync(OfficeRegistrationModel model);
        Task<AuthenticationModel> AdminLoginAsync(AdminLoginModel model);
        Task<AuthenticationModel> OfficeLoginAsync(OfficeLoginModel model);
        Task<ResultWithMessage> AdminChangePasswordAsync(AdminChangePasswordModel model);
        Task<ResultWithMessage> OfficeChangePasswordAsync(OfficeChangePasswordModel model);
        Task<ResultWithMessage> OfficeResetPasswordAsync(OfficeResetPasswordModel model);
        Task<AuthenticationModel> RefreshTokenAsync(string token);
        ResultWithMessage DeactivateAccountAsync(string userId);
        ResultWithMessage RevokeToken(string token);
        ResultWithMessage RevokeTokenById(string userId);
        Task<ResultWithMessage> AddNewRole(string roleName);
        Task<ResultWithMessage> CheckOfficeUserId(string officeTypeId, string LicenseId);
        Task<ResultWithMessage> CheckOfficeNameArabic(string nameArbic);
        Task<ResultWithMessage> CheckOfficeNameEnglish(string nameEnglish);
        Task<ResultWithMessage> CheckOfficeEmail(string email);
        Task<ResultWithMessage> ConfirmEmail(string token, string email);
    } 
}
