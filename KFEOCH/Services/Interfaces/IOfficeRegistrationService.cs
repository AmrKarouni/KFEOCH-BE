using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Dictionaries;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IOfficeRegistrationService
    {
        ResultWithMessage GetLicenseByOfficeId(int id);
        Task<ResultWithMessage> UploadDocument(FileModel model);
        FileBytesModel GetDocument(int licenseid);
        ResultWithMessage GetLicenseById(int id);
        Task<ResultWithMessage> PostLicense(License model);
        ResultWithMessage ApproveLicense(int id, License model);
        Task<ResultWithMessage> PutLicense(int id, License model);
        ResultWithMessage CalculationFeesForNewOffice(int officeid);
        Task<ResultWithMessage> CalculationFeesForNewOfficeByLisense(License model);
        ResultWithMessage GetAllPendingLicenses();
        Task<ResultWithMessage> RejectLicense(int id);
        Task<ResultWithMessage> CalculationFeesForRenew(int officeid, bool ispaid, string lang, string returnUrl);
        ResultWithMessage GetAllOfficePayments(int officeid);
        ResultWithMessage GetAllOfficeRenewPayments(int officeid);
        Task<ResultWithMessage> GenerateRenewReceipt(int id);
    }
}
