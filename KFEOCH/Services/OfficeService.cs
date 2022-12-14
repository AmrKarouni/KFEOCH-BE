using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KFEOCH.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;
        public OfficeService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,IFileService fileService)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
        }
        public Office GetById(int id)
        {
            var office = _db.Offices?.FirstOrDefault(x => x.Id == id);
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            if (office != null && office.LogoUrl != null)
            {
                office.LogoUrl = hostpath + office.LogoUrl;
            }
            return office ?? new Office();
        }
        public ResultWithMessage PutOfficeAsync(int id, Office model)
        {
            if (id != model.Id)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Invalid Model !!!",
                    MessageEnglish = $@"Invalid Model !!!",
                    MessageArabic = "نموذج غير صالح !!!"
                };
            }
            var office = _db.Offices?.FirstOrDefault(x => x.Id == id);
            if (office == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Not Found !!!",
                    MessageEnglish = $@"Office Not Found !!!",
                    MessageArabic = $@"المكتب غير مووجود!!!"
                };
            }
            var logourl = office.LogoUrl;
            _db.Entry(office).State = EntityState.Detached;
            office = model;
            office.LogoUrl = logourl;
            _db.Entry(office).State = EntityState.Modified;
            _db.SaveChanges();
            if (office.LogoUrl != null)
            {
                var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                office.LogoUrl = hostpath + office.LogoUrl;
            }
            
            return new ResultWithMessage { Success = true, Result = office };
        }

        public ResultWithMessage PutOfficeInfo(int id, OfficePutBindingModel model)
        {
            if (id != model.Id)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Invalid Model !!!",
                    MessageEnglish = $@"Invalid Model !!!",
                    MessageArabic = "نموذج غير صالح !!!"
                };
            }
            var office = _db.Offices?.FirstOrDefault(x => x.Id == id);
            if (office == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Not Found !!!",
                    MessageEnglish = $@"Office Not Found !!!",
                    MessageArabic = $@"المكتب غير مووجود!!!"
                };
            }
            var logourl = office.LogoUrl;
            _db.Entry(office).State = EntityState.Detached;

            office.NameArabic = model.NameArabic;
            office.NameEnglish = model.NameEnglish;
            office.EmailTwo = model.EmailTwo;
            office.PhoneNumber = model.PhoneNumber;
            office.FaxNumber = model.FaxNumber;
            office.MailBox = model.MailBox;
            office.PostalCode = model.PostalCode;
            office.Address = model.Address;
            office.AreaId = model.AreaId;
            office.GovernorateId = model.GovernorateId;
            office.AutoNumberOne = model.AutoNumberOne;
            office.AutoNumberTwo = model.AutoNumberTwo;
            office.LogoUrl = logourl;
            _db.Entry(office).State = EntityState.Modified;
            _db.SaveChanges();
            if (office.LogoUrl != null)
            {
                var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                office.LogoUrl = hostpath + office.LogoUrl;
            }

            return new ResultWithMessage { Success = true, Result = office };
        }


        public async Task<ResultWithMessage> UploadLogo(FileModel model)
        {
            var officeId = int.Parse(model.FileName);
            var office = GetById(officeId);
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            if (office == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Not Found !!!" };
            }
            var uploadResult = await _fileService.UploadFile(model, "logos");
            if (!uploadResult.Success)
            {
                return new ResultWithMessage { Success = false, Message = $@"Upload Logo Failed !!!" };
            }
            office.LogoUrl = uploadResult.Message;
            PutOfficeAsync(officeId, office);
            var result = new { LogoUrl = hostpath + uploadResult.Message };
            return new ResultWithMessage { Success = true, Result = result };
        }


        public async Task<ResultWithMessage> DeleteLogoAsync(int id)
        {
            var office = _db.Offices.Find(id);
            if (office == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Not Found !!!" };
            }
            var deletedfile = await _fileService.DeleteFile(office.LogoUrl);
            if (deletedfile == null || deletedfile.Success == false)
            {
                return new ResultWithMessage { Success = false, Message = $@"Delete Logo Failed !!!" };
            }
            office.LogoUrl = null;
            _db.Entry(office).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = "Logo Deleted !!!" };
        }

    }
}
