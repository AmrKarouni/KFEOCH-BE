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
            office.LogoUrl = hostpath + office.LogoUrl;
            return office ?? new Office();
        }
        public async Task<ResultWithMessage> PutOfficeAsync(int id, Office model)
        {
            if (id != model.Id)
            {
                return new ResultWithMessage { Success = false, Message = $@"Bad Request" };
            }
            var office = _db.Offices.Find(id);
            _db.Entry(office).State = EntityState.Detached;
            if (office == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Not Found !!!" };
            } 
            office = model;
            _db.Entry(office).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = office };
        }

        public async Task<ResultWithMessage> UploadLogo(FileModel model)
        {
            var officeId = int.Parse(model.FileName);
            var office = GetById(officeId);
            var hostpath = _httpContextAccessor.HttpContext.Request.Host;
            if (office == null)
            {
                new ResultWithMessage { Success = false, Message = $@"Office Not Found !!!" };
            }
            var uploadResult = _fileService.UploadFile(model, "logos");
            if (!uploadResult.Success)
            {
                new ResultWithMessage { Success = false, Message = $@"Upload Logo Failed !!!" };
            }
            office.LogoUrl = uploadResult.Message;
            await PutOfficeAsync(officeId, office);
            var result = new { logoUrl = hostpath + uploadResult.Message };
            return new ResultWithMessage { Success = true, Result = result };
        }

    }
}
