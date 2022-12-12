using KFEOCH.Models;

namespace KFEOCH.Services.Interfaces
{
    public interface IOfficeRequestService
    {
        Task<OfficeRequest> PostOfficeRequestAsync(OfficeRequest model);
    }
}
