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
        ResultWithMessage PostLicense(License model);
        ResultWithMessage ApproveLicense(int id, License model);
        ResultWithMessage PutLicense(int id, License model);
        ResultWithMessage CalculationFeesForNewOffice(int officeid);
        ResultWithMessage CalculationFeesForNewOfficeByLisense(License model);
        ResultWithMessage GetAllPendingLicenses();
        Task<ResultWithMessage> RejectLicense(int id);
        ResultWithMessage CalculationFeesForRenew(int officeid, bool ispaid);
        ResultWithMessage GetAllOfficePayments(int officeid);
    }
}
