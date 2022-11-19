using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;

namespace KFEOCH.Services
{
    public class OfficeOwnerService : IOfficeOwnerService
    {
        public List<OfficeOwnerService> GetAllOfficeOwnersByOfficeId(int id)
        {
            return null;
        }
        public async Task<ResultWithMessage> PostOfficeOwnerAsync(OfficeOwnerService model)
        {
            return null;
        }
        public async Task<ResultWithMessage> DeleteOfficeOwnerAsync(int id)
        {
            return null;
        }
        public OfficeOwnerService GetOfficeOwner(int id)
        {
            return null;
        }
    }
}
