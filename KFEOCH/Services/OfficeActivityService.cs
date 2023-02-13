using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KFEOCH.Services
{
    public class OfficeActivityService : IOfficeActivityService
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public OfficeActivityService(ApplicationDbContext db, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResultWithMessage> PostOfficeActivityAsync(OfficeActivityBindingModel model)
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
           
            var activity = _db.Activities?.Find(model.ActivityId);
            if (activity == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Activity Not Found !!!" };
            }
            var officeactivity = _db.OfficeActivities?.FirstOrDefault(x => x.OfficeId == model.OfficeId && x.ActivityId == model.ActivityId);
            if (officeactivity != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Activity Already Added !!!" };
            }
            if (office.TypeId != activity.ParentId)
            {
                return new ResultWithMessage { Success = false, Message = $@"Activity Can't be Added !!!" };
            }
            var newofficeactivity = new OfficeActivity(model.OfficeId, model.ActivityId);
            await _db.OfficeActivities.AddAsync(newofficeactivity);
            _db.SaveChanges();
            var viewmodel = new OfficeActivityViewModel(newofficeactivity);
            return new ResultWithMessage { Success = true, Result = viewmodel };
        }

        public async Task<ResultWithMessage> DeleteOfficeActivityAsync(int id)
        {
            var officeactivity = _db.OfficeActivities?.Find(id);
            if (officeactivity == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Activity Not Found !!!" };
            }

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, officeactivity.OfficeId.Value);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }

            _db.OfficeActivities.Remove(officeactivity);
            _db.SaveChanges();
            var viewmodel = new OfficeActivityViewModel(officeactivity);
            return new ResultWithMessage { Success = true, Result = viewmodel };
        }

        public List<OfficeActivityViewModel> GetOfficeActivities(int officeId)
        {
            var result = new List<OfficeActivityViewModel>();
            var office = _db.Offices?.Find(officeId);
            if (office == null)
            {
                return result;
            }
            var officeactivities = _db.OfficeActivities?.Include(x => x.Activity)
                                                            .Where(x => x.OfficeId == officeId && x.IsDeleted == false);
            result = officeactivities?.Select(x => new OfficeActivityViewModel(x)).ToList();
            return result ?? new List<OfficeActivityViewModel>();
        }
    }
}
