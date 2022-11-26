using KFEOCH.Models.Binding;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IOfficeActivityService
    {
        Task<ResultWithMessage> PostOfficeActivityAsync(OfficeActivityBindingModel model);
        Task<ResultWithMessage> DeleteOfficeActivityAsync(int id);
        List<OfficeActivityViewModel> GetOfficeActivities(int officeId);
    }
}
