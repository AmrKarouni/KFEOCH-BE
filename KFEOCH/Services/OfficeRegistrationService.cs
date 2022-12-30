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


        //public ResultWithMessage GetRnewFieldsByOfficeId(int id)
        //{
        //    var office = _db.Offices.Include(x => x.Type)
        //                            .Include(x => x.Licenses)
        //                            .FirstOrDefault(x => x.Id == id);
        //    if (office == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = "Office Not Found !!!" };
        //    }
        //    var years = office.Licenses.Count() == 0 ? 3 : office.RenewYears;
        //    var startDate = office.LicenseEndDate == null ? new DateTime(office.RegistrationDate.Value.Year, 1, 1).ToUniversalTime()
        //                                                    : office.LicenseEndDate.Value.AddSeconds(1);
        //    var endDate = startDate.AddYears(years).AddSeconds(-1);
        //    var fees = years * office.Type.YearlyFees;
        //    return new ResultWithMessage
        //    {
        //        Success = true,
        //        Result = new
        //        {
        //            Years = years,
        //            StartDate = startDate,
        //            EndDate = endDate,
        //            Fees = fees
        //        }
        //    };
        //}

        //public ResultWithMessage RenewOffice(OfficeRenewBindingModel model)
        //{
        //    var office = _db.Offices.Include(x => x.Type)
        //                            .Include(x => x.Licenses)
        //                            .ThenInclude(x => x.PaymentType)
        //                            .FirstOrDefault(x => x.Id == model.Id);
        //    if (office == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = "Office Not Found !!!" };
        //    }
        //    var years = office.Licenses.Count() == 0 ? 3 : office.RenewYears;
        //    var startDate = office.LicenseEndDate == null ?  new DateTime(office.RegistrationDate.Value.Year, 1, 1).ToUniversalTime()
        //                                                    : office.LicenseEndDate.Value.AddSeconds(1);
        //    var endDate = startDate.AddYears(years).AddSeconds(-1);
        //    var fees = years * office.Type.YearlyFees;
        //    var license = new OfficeLicense
        //    {
        //        Id = 0,
        //        OfficeId = model.Id,
        //        PaymentTypeId = 2,
        //        CreateDate = DateTime.UtcNow,
        //        RegistrationStartDate = startDate,
        //        RegistrationEndDate = endDate,
        //        PaymentAmount = fees,
        //        IsPaid = true,
        //        PaymentNumber = model.PaymentNumber,
        //    };
        //    _db.OfficeLicenses.Add(license);
        //    office.LicenseEndDate = endDate;
        //    _db.SaveChanges();
        //    var res = _db.OfficeLicenses
        //              .Include(x => x.PaymentType)
        //              .Include(x => x.Office)
        //              .Where(x => x.Id == license.Id)
        //              .Select(x => new OfficeLicenseViewModel(license));
        //    return new ResultWithMessage { Success = true, Result = res };

        //}

        public ResultWithMessage GetLicenseByOfficeId(int id)
        {
            var licenses = _db.Licenses?.Include(x => x.Specialities)
                                             .Include(x => x.Office)
                                             .Where(x => x.OfficeId == id)
                                             .ToList();
            return new ResultWithMessage { Success = true, Result = licenses };
        }



        public async Task<ResultWithMessage> UploadDocument(FileModel model)
        {
            var licenseId = int.Parse(model.FileName);
            var license = _db.Licenses.FirstOrDefault(x => x.Id == licenseId);
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

        private FilePathModel GetDocumentUrl(int licenseid)
        {
            var result = new FilePathModel();
            string docurl;
            var doc = _db.Licenses?.Find(licenseid);
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
            var licenses = _db.Licenses.Include(x => x.Specialities)
                                             .Include(x => x.Office)
                                             .Where(x => x.IsApproved == false && x.IsRejected == false).ToList();
            return new ResultWithMessage { Success = true, Result = licenses };
        }

        public ResultWithMessage GetLicenseById(int id)
        {

            var license = _db.Licenses?.Include(x => x.Specialities)
                                             .Include(x => x.Office)
                                             .FirstOrDefault(x => x.Id == id);
            if (license == null)
            {
                return new ResultWithMessage { Success = false, Message = "License Not Found !!!" };
            }
            return new ResultWithMessage { Success = true, Result = license };
        }


        public ResultWithMessage PostLicense(License model)
        {
            var office = _db.Offices.FirstOrDefault(x => x.Id == model.OfficeId);
            if (office == null)
            {
                return new ResultWithMessage { Success = false, Message = "Office Not Found !!!" };
            }
            var entity = _db.OfficeEntities.FirstOrDefault(x => x.Id == model.OfficeEntityId);
            if (entity == null)
            {
                return new ResultWithMessage { Success = false, Message = "Entity Not Found !!!" };
            }

            if (model.StartDate >= model.EndDate)
            {
                return new ResultWithMessage { Success = false, Message = "End Date Must Be Greater Than Start Date !!!" };
            }

            var sp = _db.Specialities.Where(x => model.Specialities.Select(x => x.Id).ToList().Contains(x.Id)).ToList();
            if (sp.Select(x => x.ParentId).Distinct().Count() != 1 || sp.Select(x => x.ParentId).FirstOrDefault() != office.TypeId)
            {
                return new ResultWithMessage { Success = false, Message = "Not Eligible Speicality !!!" };
            }
            var firstlicense = _db.Licenses.FirstOrDefault(x => x.OfficeId == model.OfficeId);
            if (firstlicense != null)
            {
                model.IsFirst = true;
            }
            else
            {
                model.IsFirst = false;
            }

            model.Specialities = sp;
            model.IsPending = true;
            _db.Licenses.Add(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }

        public ResultWithMessage PutLicense(int id, License model)
        {
            if (id != model.Id)
            {
                return new ResultWithMessage { Success = false, Message = "Invalid Model !!!" };
            }
            var license = _db.Licenses.Include(x => x.Specialities).FirstOrDefault(x => x.Id == id);
            //_db.Entry(license).State = EntityState.Detached;
            if (license == null)
            {
                return new ResultWithMessage { Success = false, Message = "License Not Found !!!" };
            }
            var office = _db.Offices.FirstOrDefault(x => x.Id == model.OfficeId);
            if (office == null)
            {
                return new ResultWithMessage { Success = false, Message = "Office Not Found !!!" };
            }
            var entity = _db.OfficeEntities.FirstOrDefault(x => x.Id == model.OfficeEntityId);
            if (entity == null)
            {
                return new ResultWithMessage { Success = false, Message = "Entity Not Found !!!" };
            }

            if (model.StartDate >= model.EndDate)
            {
                return new ResultWithMessage { Success = false, Message = "End Date Must Be Greater Than Start Date !!!" };
            }

            //////////
            var sp = _db.Specialities.Where(x => model.Specialities.Select(x => x.Id).ToList().Contains(x.Id)).ToList();
            if (sp.Select(x => x.ParentId).Distinct().Count() != 1 || sp.Select(x => x.ParentId).FirstOrDefault() != office.TypeId)
            {
                return new ResultWithMessage { Success = false, Message = "Not Eligible Speicality !!!" };
            }
            foreach (var speciality in license.Specialities)
            {
                license.Specialities.Remove(speciality);
            }

            foreach (var newsp in sp)
            {
                license.Specialities.Add(newsp);
            }


            //////////
            var officeSpecialities = _db.OfficeSpecialities.Where(x => x.OfficeId == model.OfficeId).ToList();
            foreach (var speciality in officeSpecialities)
            {
                _db.OfficeSpecialities.Remove(speciality);
            }

            var newSpecialities = new List<OfficeSpeciality>();
            foreach (var newsp in sp)
            {
                var newofficesp = new OfficeSpeciality
                {
                    OfficeId = office.Id,
                    SpecialityId = newsp.Id,
                    AddedDate = DateTime.UtcNow,
                    IsApproved = true,
                    IsDeleted = false
                };
                newSpecialities.Add(newofficesp);
            }
            _db.OfficeSpecialities.AddRange(newSpecialities);


            /////////
            office.EntityId = model.OfficeEntityId;

            /////////
            _db.Entry(license).CurrentValues.SetValues(model);
            _db.Entry(license).State = EntityState.Modified;
            _db.Entry(office).State = EntityState.Modified;
            //_db.Entry(officeSpecialities).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = license };
        }

        public ResultWithMessage CalculationFeesForNewOffice(int officeid)
        {
            var office = _db.Offices.FirstOrDefault(x => x.Id == officeid);
            if (office == null)
            {
                return new ResultWithMessage { Success = false, Message = "Office Not Found !!!" };
            }
            var firstlicense = _db.Licenses.Include(x => x.OfficeEntity)
                                            .FirstOrDefault(x => x.OfficeId == officeid
                                                            && x.IsFirst == true
                                                            && x.IsPending == false
                                                            && x.IsApproved == true
                                                            && x.IsRejected == false);
            if (firstlicense == null)
            {
                return new ResultWithMessage { Success = false, Message = "First License Not Found !!!" };
            }
            var notfirstlicense = _db.Licenses.FirstOrDefault(x => x.OfficeId == officeid
                                                            && x.IsFirst == false
                                                            && x.IsPending == false
                                                            && x.IsApproved == true
                                                            && x.IsRejected == false);
            if (notfirstlicense != null)
            {
                return new ResultWithMessage { Success = false, Message = "Frist Fees Not Eligible !!!" };
            }
            var fees = new List<OfficePayment>();
            fees.Add(new OfficePayment
            {
                OfficeId = office.Id,
                TypeId = 1,
                RequestNameArabic = "رسم انتساب ",
                RequestNameEnglish = "Registration Fees",
                PaymentDate = DateTime.UtcNow,
                Amount = 100,
                IsPaid = false,
            });
            var YearlyFees = firstlicense.OfficeEntity.YearlyFees;
            var fee = (firstlicense.StartDate.Month < 7 ? YearlyFees : (YearlyFees * 0.5)) + ((YearlyFees) * 2);
            fees.Add(new OfficePayment
            {
                OfficeId = office.Id,
                TypeId = 1,
                RequestNameArabic = "رسم انتساب ",
                RequestNameEnglish = "Registration Fees",
                PaymentDate = DateTime.UtcNow,
                Amount = fee,
                IsPaid = false,
            });
            return new ResultWithMessage { Success = true, Result = fees };
        }
    }
}
