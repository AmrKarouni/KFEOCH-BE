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
        private readonly IConfiguration _configuration;
        public OfficeRegistrationService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, IFileService fileService, IConfiguration configuration)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
            _configuration = configuration;
        }


        public ResultWithMessage GetRnewFieldsByOfficeId(int id)
        {
            var office = _db.Offices.Include(x => x.Type)
                                    .Include(x => x.Licenses)
                                    .FirstOrDefault(x => x.Id == id);
            if (office == null)
            {
                return new ResultWithMessage { Success = false, Message = "Office Not Found !!!" };
            }
            var years = office.Licenses.Count() == 0 ? 3 : office.RenewYears;
            var startDate = office.LicenseEndDate == null ? new DateTime(office.RegistrationDate.Value.Year, 1, 1).ToUniversalTime()
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
            var startDate = office.LicenseEndDate == null ?  new DateTime(office.RegistrationDate.Value.Year, 1, 1).ToUniversalTime()
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
                      .Include(x => x.Office)
                      .Where(x => x.Id == license.Id)
                      .Select(x => new OfficeLicenseViewModel(license));
            return new ResultWithMessage { Success = true, Result = res };

        }

        public ResultWithMessage GetLicenseByOfficeId(int id)
        {
            var licenses = _db.OfficeLicenses.Include(x => x.PaymentType)
                                             .Include(x => x.Office)
                                             .Where(x => x.OfficeId == id)
                                             .Select(x => new OfficeLicenseViewModel(x)).ToList();
            return new ResultWithMessage { Success = true, Result = licenses };
        }

        public async Task<ResultWithMessage> UploadDocument(FileModel model)
        {
            var licenseId = int.Parse(model.FileName);
            var license = _db.OfficeLicenses.FirstOrDefault(x => x.Id == licenseId);
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            if (license == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"License Not Found !!!" };
            }
            var uploadResult = await _fileService.UploadFile(model, "licenses");
            if (!uploadResult.Success)
            {
                return new ResultWithMessage { Success = false, Message = $@"Upload Logo Failed !!!" };
            }
            license.DocumentUrl = uploadResult.Message;
            _db.Entry(license).State = EntityState.Modified;
            _db.SaveChanges();
            var result = new { LogoUrl = hostpath + uploadResult.Message };
            return new ResultWithMessage { Success = true, Result = result };
        }

        public FilePathModel GetDocumentUrl(int licenseid)
        {
            var result = new FilePathModel();
            string docurl;
            var doc = _db.OfficeLicenses?.Find(licenseid);
            if (doc == null)
            {
                return result;
            }
            docurl = doc.DocumentUrl;
            if (docurl != null)
            {
                result = _fileService.GetFilePath(docurl);
            }
            return result;
        }

        public FileBytesModel GetDocument(int licenseid)
        {
            FileBytesModel result = new FileBytesModel();
            try
            {
                var url = GetDocumentUrl(licenseid);
                var host = _configuration.GetValue<string>("FileHostServer");

                if (url == null || url.Path == null)
                {
                    return result;
                }
                result.Bytes = File.ReadAllBytes(host + url.Path);
                result.FileName = url.Path.Substring(url.Path.LastIndexOf('/') + 1).ToLower();
                result.ContentType = url.ContentType;
                return result;
            }
            catch (Exception e)
            {
                return result = new FileBytesModel();
                throw;
            }
        }

        public ResultWithMessage GetAllPendingLicenses()
        {
            var licenses = _db.OfficeLicenses.Include(x => x.PaymentType)
                                             .Include(x => x.Office)
                                             .Where(x => x.IsApproved == false && x.IsCanceled == false && x.IsRejected == false)
                                             .Select(x => new OfficeLicenseViewModel(x)).ToList();
            return new ResultWithMessage { Success = true, Result = licenses };
        }

        //public ResultWithMessage PutLicenseByAdmin(int id, OfficeLicenseViewModel mode)
        //{

        //}

    }
}
