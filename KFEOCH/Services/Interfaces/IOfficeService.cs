using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IOfficeService
    {
        Office GetById(int id);
        ResultWithMessage PutOfficeAsync(int id, Office model);
        Task<ResultWithMessage> PutOfficeInfo(int id, OfficePutBindingModel model);
        Task<ResultWithMessage> UploadLogo(FileModel model);
        Task<ResultWithMessage> DeleteLogoAsync(int id);
        ResultWithMessage GetOfficesForAdmin(FilterModel model);
        ResultWithMessage GetOfficesPaymentsReportForAdmin(FilterModel model);
        List<OfficesPaymentsReportViewModel> ExportOfficesPaymentsReportForAdmin(FilterModel model);
        List<OfficeAdminExportViewModel> ExportOfficesForAdmin(FilterModel model);
    }
}
