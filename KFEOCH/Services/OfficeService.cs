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
        private readonly IFileService _fileService;
        public OfficeService(ApplicationDbContext db, IFileService fileService)
        {
            _db = db;
            _fileService = fileService;
        }
        public Office GetById(int id)
        {
            var office = _db.Offices?.Find(id);
            return office ?? new Office();
        }
        public async Task<ResultWithMessage> PutOfficeAsync(int id, Office model)
        {
            string logoUrl = null;
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
            //if (logofile != null)
            //{

            //    ResultWithMessage uploadresult = _fileService.UploadFile(logofile);
            //    if (uploadresult.Success)
            //    {
            //        logoUrl = uploadresult.Message;
            //    }

            //}
            office = model;
            office.LogoUrl = logoUrl;
            _db.Entry(office).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = office };
        }
    }
}
