using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IOfficeService
    {
        Office GetById(int id);
        ResultWithMessage PutOfficeAsync(int id, Office model);
        ResultWithMessage PutOfficeInfo(int id, OfficePutBindingModel model);
        Task<ResultWithMessage> UploadLogo(FileModel model);
        Task<ResultWithMessage> DeleteLogoAsync(int id);
        ResultWithMessage GetOfficesForAdmin(FilterModel model);
    }
}
