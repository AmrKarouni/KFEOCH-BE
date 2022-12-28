using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Dictionaries;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KFEOCH.Services
{
    public class OfficeRegistrationService : IOfficeRegistrationService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;
        public OfficeRegistrationService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, IFileService fileService)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
        }


        public ResultWithMessage GetRnewFieldsByOfficeId(int id)
        {
            var office = _db.Offices.Include(x=> x.Type)
                                    .Include(x => x.Licenses)
                                    .FirstOrDefault(x => x.Id == id);
            if (office == null)
            {
                return new ResultWithMessage { Success = false, Message = "Office Not Found !!!" };
            }
            var years = office.Licenses.Count() == 0 ? 3 : office.RenewYears;
            var startDate = office.LicenseEndDate == null ? new DateTime(DateTime.UtcNow.Year, 1, 1).ToUniversalTime()
                                                            : office.LicenseEndDate.Value.AddSeconds(1);
            var endDate = startDate.AddYears(years).AddSeconds(-1);
            var fees = years * office.Type.YearlyFees;
            return new ResultWithMessage
            {
                Success = true,
                Result = new
                {
                    Years = years,
                    StartDate = startDate,
                    EndDate = endDate,
                    Fees = fees
                }
            };
        }

        public ResultWithMessage RenewOffice(OfficeRenewBindingModel model)
        {
            var office = _db.Offices.Include(x => x.Type)
                                    .Include(x => x.Licenses)
                                    .ThenInclude(x => x.PaymentType)
                                    .FirstOrDefault(x => x.Id == model.Id);
            if (office == null)
            {
                return new ResultWithMessage { Success = false, Message = "Office Not Found !!!" };
            }
            var years = office.Licenses.Count() == 0 ? 3 : office.RenewYears;
            var startDate = office.LicenseEndDate == null ? new DateTime(DateTime.UtcNow.Year, 1, 1).ToUniversalTime() 
                                                            : office.LicenseEndDate.Value.AddSeconds(1);
            var endDate = startDate.AddYears(years).AddSeconds(-1);
            var fees = years * office.Type.YearlyFees;
            var license = new OfficeLicense
            {
                Id = 0,
                OfficeId = model.Id,
                PaymentTypeId = 2,
                CreateDate = DateTime.UtcNow,
                RegistrationStartDate = startDate,
                RegistrationEndDate = endDate,
                PaymentAmount = fees,
                IsPaid = true,
                PaymentNumber = model.PaymentNumber,
            };
            _db.OfficeLicenses.Add(license);
            office.LicenseEndDate = endDate;
            _db.SaveChanges();
            var res = _db.OfficeLicenses
                      .Include(x => x.PaymentType)
                      .Where(x => x.Id == license.Id)
                      .Select(x => new OfficeLicenseViewModel(license));
            return new ResultWithMessage { Success = true, Result = res };

        }

        //public ResultWithMessage GetLicenseByOfficeId(int id)
        //{
        //    var license = _db.OfficeLicenses.Include(x => x.).FirstOrDefault(x => x.OfficeId == id);
        //}


    }
}
