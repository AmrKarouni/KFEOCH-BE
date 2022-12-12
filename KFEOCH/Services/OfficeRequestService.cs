using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Services.Interfaces;

namespace KFEOCH.Services
{
    public class OfficeRequestService : IOfficeRequestService
    {
        private readonly ApplicationDbContext _db;
        public OfficeRequestService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OfficeRequest> PostOfficeRequestAsync(OfficeRequest model)
        {
            return null;
        }
    }
}
