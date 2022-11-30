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
        public OfficeOwnerWithDocuments GetById(int id)
        {
            var officeOwner = _db.OfficeOwners?.Include(x => x.Documents).FirstOrDefault(x => x.Id == id);
            var types = _db.OwnerDocumentTypes.ToList().Select(x => new OfficeOwnerDocumentTypeView
            {
                Id = x.Id,
                NameArabic = x.NameArabic,
                NameEnglish = x.NameEnglish,
                Files = officeOwner.Documents.Where(d => (d.TypeId ?? 0) == x.Id && d.IsActive == true).Select(d => new OfficeOwnerDocumentView
                {
                    Id = d.Id,
                    DocumentUrl = d.DocumentUrl,
                    AddedDate = d.AddedDate
                }).ToList()
            });
            var result = new OfficeOwnerWithDocuments()
            {
                Id = officeOwner.Id,
                NameArabic = officeOwner.NameArabic,
                NameEnglish = officeOwner.NameEnglish,
                Documents = types
            };
            return result ?? new OfficeOwnerWithDocuments();
        }
        public List<OfficeOwnerViewModel> GetAllOfficeOwnersByOfficeId(int id)
        {
            var q = _db.OfficeOwners?.Include(g => g.Gender)
                                      .Include(s => s.Speciality)
                                     .Where(a => a.OfficeId == id && a.IsDeleted == false)
                                     .Select(x => new OfficeOwnerViewModel(x))
                                    .ToList();
            return q;
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
            model.IsApproved = true;
            model.IsDeleted = false;
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
            owner.IsApproved = true;
            owner.IsDeleted = false;
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
            owner.IsApproved = true;
            owner.IsDeleted = true;
            _db.Entry(owner).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Owner {owner.NameArabic} | {owner.NameEnglish} Deleted !!!" };
        }
    }
}
