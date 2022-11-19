using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KFEOCH.Services
{
    public class OfficeOwnerService : IOfficeOwnerService
    {
        private readonly ApplicationDbContext _db;
        public OfficeOwnerService(ApplicationDbContext db)
        {
            _db = db;
        }
        public OfficeOwner GetById(int id)
        {
            var officeOwner = _db.OfficeOwners?.Find(id);
            return officeOwner ?? new OfficeOwner();
        }
        public List<OfficeOwner> GetAllOfficeOwnersByOfficeId(int id)
        {
            var list = new List<OfficeOwner>();
            list = _db.OfficeOwners?.Where(a => a.OfficeId == id && a.IsDeleted == false).ToList();
            return list ?? new List<OfficeOwner>();
        }
        public async Task<ResultWithMessage> PostOfficeOwnerAsync(OfficeOwner model)
        {
            var o = _db.OfficeOwners?.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish)
                                   );
            if (o != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Owner {model.NameArabic} | {model.NameEnglish} Already Exist !!!" };
            }
            await _db.OfficeOwners.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> PutOfficeOwnerAsync(int id, OfficeOwner model)
        {
            if (id != model.Id)
            {
                return new ResultWithMessage { Success = false, Message = $@"Bad Request" };
            }
            var owner = _db.OfficeOwners?.Find(id);
            _db.Entry(owner).State = EntityState.Detached;
            if (owner == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Owner Not Found !!!" };
            }
            owner = model;
            _db.Entry(owner).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = owner };
        }
        public async Task<ResultWithMessage> DeleteOfficeOwnerAsync(int id)
        {
            var owner = _db.OfficeOwners?.Find(id);
            if (owner == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Owner Not Found !!!" };
            }
            owner.IsDeleted = true;
            _db.Entry(owner).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Owner {owner.NameArabic} | {owner.NameEnglish} Deleted !!!" };
        }
        


    }
}
