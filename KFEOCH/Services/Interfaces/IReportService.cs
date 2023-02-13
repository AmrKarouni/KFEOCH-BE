using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IReportService
    {
        List<OfficeAdminExportViewModel> ExportOfficesForAdmin();
        List<OfficeOwnerReportViewModel> ExportOfficeOwnersForAdmin();
        List<OfficeSpecialityReportViewModel> ExportOfficeSpecialitiesForAdmin();
        List<OfficeContactReportViewModel> ExportOfficeContactsForAdmin();
        List<OfficeLicenseReportViewModel> ExportOfficeLicensesForAdmin();
    }
}
