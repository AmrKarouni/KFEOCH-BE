using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IOfficeContactService
    {
        List<OfficeContactViewModel> GetAllOfficeContactsByOfficeId(int officeid);
        Task<ResultWithMessage> PostOfficeContactAsync(OfficeContactBindingModel model);
        Task<ResultWithMessage> PutOfficeContactAsync(int id, OfficeContact model);
        Task<ResultWithMessage> DeleteOfficeContactAsync(int id);
    }
}
