using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Dictionaries;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IOfficeRegistrationService
    {
        ResultWithMessage GetRnewFieldsByOfficeId(int id);
        ResultWithMessage RenewOffice(OfficeRenewBindingModel model);
        ResultWithMessage GetLicenseByOfficeId(int id);

        Task<ResultWithMessage> UploadDocument(FileModel model);
        FilePathModel GetDocumentUrl(int licenseid);
        FileBytesModel GetDocument(int licenseid);
        ResultWithMessage GetAllPendingLicenses();
    }
}
