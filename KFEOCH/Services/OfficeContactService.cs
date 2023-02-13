using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace KFEOCH.Services
{
    public class OfficeContactService : IOfficeContactService
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OfficeContactService(ApplicationDbContext db,IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<OfficeContactViewModel> GetAllOfficeContactsByOfficeId(int officeid)
        {
            var result = new List<OfficeContactViewModel>();
            var office = _db.Offices?.Find(officeid);
            if (office == null)
            {
                return result;
            }
            var contactlist = _db.OfficeContacts?.Include(c => c.Contact)
                                     .Where(a => a.OfficeId == officeid && a.IsDeleted == false);
            result = contactlist?.Select(x => new OfficeContactViewModel(x)).ToList();
            return result ?? new List<OfficeContactViewModel>();

        }
        public async Task<ResultWithMessage> PostOfficeContactAsync(OfficeContactBindingModel model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, model.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }

            var office = _db.Offices.Find(model.OfficeId);
            if (office == null)
            {
                return new ResultWithMessage { Success = false, Message = "Office Not Found !!!" };
            }
           
            var contacttype = _db.ContactTypes.Find(model.ContactId);
            if (contacttype == null)
            {
                return new ResultWithMessage { Success = false, Message = "Contact Type Not Found !!!" };
            } 
            var contact = new OfficeContact(model);
            await _db.OfficeContacts.AddAsync(contact);
            _db.SaveChanges();
            var viewmode = new OfficeContactViewModel(contact);
            return new ResultWithMessage { Success = true, Result = viewmode };
        }
        public async Task<ResultWithMessage> PutOfficeContactAsync(int id, OfficeContact model)
        {
            if (id != model.Id)
            {
                return new ResultWithMessage { Success = false, Message = $@"Bad Request" };
            }

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, model.OfficeId.Value);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
            var office = _db.Offices.Find(model.OfficeId);
            if (office == null)
            {
                return new ResultWithMessage { Success = false, Message = "Office Not Found !!!" };
            }
            

            var contacttype = _db.ContactTypes.Find(model.ContactId);
            if (contacttype == null)
            {
                return new ResultWithMessage { Success = false, Message = "Contact Type Not Found !!!" };
            }

            var contact = _db.OfficeContacts?.Find(id);
            _db.Entry(contact).State = EntityState.Detached;
            if (contact == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Contact Not Found !!!" };
            }
            contact = model;
            contact.IsApproved = true;
            contact.IsDeleted = false;
            _db.Entry(contact).State = EntityState.Modified;
            _db.SaveChanges(); 
            var viewmode = new OfficeContactViewModel(contact);
            return new ResultWithMessage { Success = true, Result = viewmode };

        }
        public async Task<ResultWithMessage> DeleteOfficeContactAsync(int id)
        {
            var contact = _db.OfficeContacts?.Find(id);
            if (contact == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Contact Not Found !!!" };
            }
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, contact.OfficeId.Value);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
            _db.OfficeContacts?.Remove(contact);
            _db.SaveChanges();
            var viewmode = new OfficeContactViewModel(contact);
            return new ResultWithMessage { Success = true, Result = viewmode };
        }
    }
}
