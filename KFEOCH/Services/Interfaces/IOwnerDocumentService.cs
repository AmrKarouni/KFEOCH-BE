using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IOwnerDocumentService
    {
        OfficeOwnerWithDocuments GetAllDocumentsByOwnerId(int ownerid);
        Task<ResultWithMessage> PostOwnerDocumentAsync(OwnerFileModel model);
        FilePathModel GetDocumentUrl(int documentid);
    }
}
