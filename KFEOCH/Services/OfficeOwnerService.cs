using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KFEOCH.Services
{
    public class OfficeOwnerService : IOfficeOwnerService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OfficeOwnerService(ApplicationDbContext db, IConfiguration configuration,IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _configuration = configuration;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }
        public ResultWithMessage GetById(int id)
        {
            var result = new OfficeOwnerWithDocuments();
            var officeOwner = _db.OfficeOwners?.Include(x => x.Documents).FirstOrDefault(x => x.Id == id);
            if (officeOwner == null)
            {
                return new ResultWithMessage { Success = false, Message = "No Owner Found !!", Result = result };
            }
            var types = _db.OwnerDocumentTypes.ToList().Select(x => new OfficeOwnerDocumentTypeView
            {
                Id = x.Id,
                NameArabic = x.NameArabic,
                NameEnglish = x.NameEnglish,
                Files = officeOwner.Documents.Where(d => (d.TypeId ?? 0) == x.Id && d.IsActive == true).Select(d => new OfficeOwnerDocumentView
                {
                    Id = d.Id,
                    Name = d.DocumentUrl.Substring(d.DocumentUrl.LastIndexOf('/')+1).ToLower(),
                    DocumentUrl = d.DocumentUrl,
                    AddedDate = d.AddedDate
                }).ToList()
            });
            result = new OfficeOwnerWithDocuments()
            {
                Id = officeOwner.Id,
                OfficeId = officeOwner.OfficeId,
                NameArabic = officeOwner.NameArabic,
                NameEnglish = officeOwner.NameEnglish,
                GenderId = officeOwner.GenderId,
                NationalId = officeOwner.NationalId,
                NationalityId = officeOwner.NationalityId,
                Email = officeOwner.Email,
                PhoneNumber = officeOwner.PhoneNumber,
                PhoneNumberTwo = officeOwner.PhoneNumberTwo,
                PositionId = officeOwner.PositionId,
                SemId = officeOwner.SemId,
                SpecialityId = officeOwner.SpecialityId,
                ExperienceYears = officeOwner.ExperienceYears,
                IsApproved = officeOwner.IsApproved,
                IsDeleted = officeOwner.IsDeleted,
                Documents = types
            };
             return new ResultWithMessage { Success = true, Result = result };
        }
        public List<OfficeOwnerViewModel> GetAllOfficeOwnersByOfficeId(int id)
        {
            var q = _db.OfficeOwners?.Include(g => g.Gender)
                                     .Include(s => s.Speciality)
                                     .Include(p => p.Position)
                                     .Include(n => n.Nationality)
                                     .Where(a => a.OfficeId == id && a.IsDeleted == false)
                                     .Select(x => new OfficeOwnerViewModel(x))
                                     .ToList();
            return q;
        }
        public async Task<ResultWithMessage> PostOfficeOwnerAsync(OfficeOwner model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, model.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
            var o = _db.OfficeOwners?.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish)
                                   );
            if (o != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Owner {model.NameArabic} | {model.NameEnglish} Already Exist !!!" };
            }
            model.IsApproved = true;
            model.IsDeleted = false;
            var office = _db.Offices?.FirstOrDefault(x => x.Id == model.OfficeId);
            if (office?.IsLocal == true)
            {
                var defaultLocalNationality = _db.Nationalities?.FirstOrDefault(x => x.NameEnglish.ToLower() == _configuration.GetValue<string>("DefaultLocalNationality").ToLower());
                model.NationalityId = defaultLocalNationality.Id;
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
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, model.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
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
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, owner.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
            owner.IsApproved = true;
            owner.IsDeleted = true;
            _db.Entry(owner).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Owner {owner.NameArabic} | {owner.NameEnglish} Deleted !!!" };
        }
    }
}
