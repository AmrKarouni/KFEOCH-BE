using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KFEOCH.Services
{
    public class OfficeSpecialityService : IOfficeSpecialityService
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OfficeSpecialityService(ApplicationDbContext db,IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResultWithMessage> PostOfficeSpecialityAsync(OfficeSpecialityBindingModel model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, model.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
            var office = _db.Offices?.Find(model.OfficeId);
            if (office == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Not Found !!!" };
            }
            
            var speciality = _db.Specialities?.Find(model.SpecialityId);
            if (speciality == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Speciality Not Found !!!" };
            }
            var officespeciality = _db.OfficeSpecialities?.FirstOrDefault(x => x.OfficeId == model.OfficeId && x.SpecialityId == model.SpecialityId);
            if (officespeciality != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Speciality Already Added !!!" };
            }
            if (office.TypeId != speciality.ParentId)
            {
                return new ResultWithMessage { Success = false, Message = $@"Speciality Can't be Added !!!" };
            }
            var newofficespeciality = new OfficeSpeciality(model.OfficeId, model.SpecialityId);
            await _db.OfficeSpecialities.AddAsync(newofficespeciality);
            _db.SaveChanges();
            var viewmodel = new OfficeSpecialityViewModel(newofficespeciality);
            return new ResultWithMessage { Success = true, Result = viewmodel };
        }

        public async Task<ResultWithMessage> DeleteOfficeSpecialityAsync(int id)
        {
            var officespeciality = _db.OfficeSpecialities?.Find(id);
            if (officespeciality == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Speciality Not Found !!!" };
            }

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, officespeciality.OfficeId.Value);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }

            _db.OfficeSpecialities.Remove(officespeciality);
            _db.SaveChanges();
            var viewmodel = new OfficeSpecialityViewModel(officespeciality);
            return new ResultWithMessage { Success = true, Result = viewmodel };
        }

        public List<OfficeSpecialityViewModel> GetOfficeSpecialities(int officeId)
        {
            var result = new List<OfficeSpecialityViewModel>();
            var office = _db.Offices?.Find(officeId);
            if (office == null)
            {
                return result;
            }
            var officespecialities = _db.OfficeSpecialities?.Include(x => x.Speciality)
                                                            .Where(x => x.OfficeId == officeId && x.IsDeleted == false);
            result = officespecialities?.Select(x => new OfficeSpecialityViewModel(x)).ToList();
            return result ?? new List<OfficeSpecialityViewModel>();
        }
    }
}
