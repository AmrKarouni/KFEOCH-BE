using KFEOCH.Models;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IOfficeOwnerService
    {
        List<OfficeOwnerService> GetAllOfficeOwnersByOfficeId(int id);
        Task<ResultWithMessage> PostOfficeOwnerAsync(OfficeOwnerService model);
        Task<ResultWithMessage> DeleteOfficeOwnerAsync(int id);
        OfficeOwnerService GetOfficeOwner(int id);
    }
}
