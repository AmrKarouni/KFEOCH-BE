using KFEOCH.Models;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IOfficeOwnerService
    {
        OfficeOwner GetById(int id);
        List<OfficeOwner> GetAllOfficeOwnersByOfficeId(int id);
        Task<ResultWithMessage> PostOfficeOwnerAsync(OfficeOwner model);
        Task<ResultWithMessage> PutOfficeOwnerAsync(int id ,OfficeOwner model);
        Task<ResultWithMessage> DeleteOfficeOwnerAsync(int id);
    }
}
