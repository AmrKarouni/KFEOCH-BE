using KFEOCH.Models.Binding;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IOfficeSpecialityService
    {
        Task<ResultWithMessage> PostOfficeSpecialityAsync(OfficeSpecialityBindingModel model);
        Task<ResultWithMessage> DeleteOfficeSpecialityAsync(int id);
        List<OfficeSpecialityViewModel> GetOfficeSpecialities(int officeId);
    }
}
