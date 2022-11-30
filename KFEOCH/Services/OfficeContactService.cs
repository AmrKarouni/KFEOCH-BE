using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace KFEOCH.Services
{
    public class OfficeContactService : IOfficeContactService
    {
        private readonly ApplicationDbContext _db;
        public OfficeContactService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<OfficeContactViewModel> GetAllOfficeContactsByOfficeId(int officeid)
        {
            var list = _db.OfficeContacts?.Include(c => c.Contact)
                                     .Where(a => a.OfficeId == officeid && a.IsDeleted == false)
                                     .Select(x => new OfficeContactViewModel(x))
                                    .ToList();
            return list;

        }
        public async Task<ResultWithMessage> PostOfficeContactAsync(OfficeContactBindingModel model)
        {
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
            _db.OfficeContacts?.Remove(contact);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = "Contact Deleted !!!" };
        }
    }
}
