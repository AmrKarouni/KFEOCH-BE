using KFEOCH.Contexts;
using KFEOCH.Models;
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
            var office = _db.Offices?.Find(id);
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            if (office.LogoUrl != null)
            {
                office.LogoUrl = hostpath + office.LogoUrl;
            }
            return office ?? new Office();
        }
        public async Task<ResultWithMessage> PutOfficeAsync(int id, Office model)
        {
            if (id != model.Id)
            {
                return new ResultWithMessage { Success = false, Message = $@"Bad Request" };
            }
            var office = _db.Offices.Find(id);
            var logourl = office.LogoUrl;
            _db.Entry(office).State = EntityState.Detached;
            if (office == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Not Found !!!" };
            } 
            office = model;
            office.LogoUrl = logourl;
            _db.Entry(office).State = EntityState.Modified;
            _db.SaveChanges();
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            office.LogoUrl = hostpath + office.LogoUrl;
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
            await PutOfficeAsync(officeId, office);
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
            await PutOfficeAsync(office.Id, office);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = "Logo Deleted !!!" };
        }

    }
}
