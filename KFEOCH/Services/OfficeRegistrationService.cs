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
        private readonly TimeZoneInfo timezone;
        public OfficeRegistrationService(ApplicationDbContext db,
                                         IHttpContextAccessor httpContextAccessor,
                                         IFileService fileService,
                                         IConfiguration configuration)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
            _configuration = configuration;
            timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
        }

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
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "License Not Found !!!",
                    MessageEnglish = "License Not Found !!!",
                    MessageArabic = "الترخيص غير موجود !!!"
                };
            }
            var uploadResult = await _fileService.UploadPdfFile(model, "licenses");
            if (!uploadResult.Success)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Upload Document Failed !!!",
                    MessageEnglish = "Upload Document Failed !!!",
                    MessageArabic = "فشل في تحميل المستند "
                };
                _db.Licenses.Remove(license);
            }
            else
            {
                license.DocumentUrl = uploadResult.Message;
                _db.Entry(license).State = EntityState.Modified;
            }

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

        public ResultWithMessage GetLicenseById(int id)
        {

            var license = _db.Licenses?.Include(x => x.Specialities)
                                             .Include(x => x.Office)
                                             .FirstOrDefault(x => x.Id == id);
            if (license == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "License Not Found !!!",
                    MessageEnglish = "License Not Found !!!",
                    MessageArabic = "الترخيص غير موجود !!!"
                };
            }
            return new ResultWithMessage { Success = true, Result = license };
        }

        public ResultWithMessage PostLicense(License model)
        {
            if (!CheckLicense(model).Success)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = CheckLicense(model).Message,
                    MessageArabic = CheckLicense(model).MessageArabic,
                    MessageEnglish = CheckLicense(model).MessageEnglish,
                };
            }
            var pending = _db.Licenses.FirstOrDefault(x => x.OfficeId == model.OfficeId
                                                            && (x.IsPending == true || DateTime.UtcNow > x.EndDate && (x.IsApproved || x.IsPending))
                                                            && x.IsApproved == false
                                                            && x.IsRejected == false);
            if (pending != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Office Has Pending License !!!",
                    MessageEnglish = "Office Has Pending License !!!",
                    MessageArabic = "المكتب لديه ترخيص معلق !!!"
                };
            }
            var sp = _db.Specialities.Where(x => model.Specialities.Select(x => x.Id).ToList().Contains(x.Id)).ToList();
            var licenses = _db.Licenses.Where(x => x.OfficeId == model.OfficeId
                                              && x.IsApproved == true).ToList();
            if (licenses.Count() == 0)
            {
                model.IsFirst = true;
            }
            else
            {
                model.IsFirst = false;
            }
            foreach (var l in licenses)
            {
                l.IsLast = false;
                _db.Entry(l).State = EntityState.Modified;
            }
            model.Specialities = sp;
            model.IsPending = true;
            model.IsApproved = false;
            model.IsRejected = false;
            model.IsLast = true;
            _db.Licenses.Add(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }

        private ResultWithMessage CheckLicense(License model)
        {
            var office = _db.Offices.FirstOrDefault(x => x.Id == model.OfficeId);
            if (office == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Office Not Found !!!",
                    MessageEnglish = "Office Not Found !!!",
                    MessageArabic = "المكتب غير موجود !!!"
                };
            }
            var entity = _db.OfficeEntities.FirstOrDefault(x => x.Id == model.OfficeEntityId);
            if (entity == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Entity Not Found !!!",
                    MessageEnglish = "Entity Not Found !!!",
                    MessageArabic = "الكيان غير موجود !!!"
                };
            }

            if (model.StartDate >= model.EndDate)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "End Date Must Be Greater Than Start Date !!!",
                    MessageEnglish = "End Date Must Be Greater Than Start Date !!!",
                    MessageArabic = "يجب أن يكون تاريخ بداية الترخيص قبل تاريخ المهاية !!!"
                };
            }

            var sp = _db.Specialities.Where(x => model.Specialities.Select(x => x.Id).ToList().Contains(x.Id)).ToList();
            if (sp.Select(x => x.ParentId).Distinct().Count() != 1 || sp.Select(x => x.ParentId).FirstOrDefault() != office.TypeId)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Not Eligible Speicality !!!",
                    MessageEnglish = "Not Eligible Speicality !!!",
                    MessageArabic = "تخصص غير مؤهل !!!"
                };
            }

            var lastdate = _db.Licenses?.OrderByDescending(x => x.CreatedDate)
                                        .FirstOrDefault(x => x.OfficeId == model.OfficeId
                                                            && x.IsRejected == false
                                                            && x.IsPending == false
                                                            && x.IsApproved == false);
            if (lastdate != null && lastdate.CreatedDate >= model.StartDate)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Dates Overlap !!!",
                    MessageEnglish = "Dates Overlap !!!",
                    MessageArabic = "تداخل التواريخ !!!"
                };
            }

            return new ResultWithMessage { Success = true };
        }
        // admin
        public ResultWithMessage ApproveLicense(int id, License model)
        {

            if (id != model.Id)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Invalid Model !!!",
                    MessageEnglish = "Invalid Model !!!",
                    MessageArabic = "نموذج غير صالح !!!"
                };
            }
            var license = _db.Licenses?.Include(x => x.Specialities)
                                       .Include(x => x.Office)
                                       .ThenInclude(x => x.OfficeSpecialities)
                                       .Include(x => x.Office)
                                       .ThenInclude(x => x.OfficePayments).FirstOrDefault(x => x.Id == id);

            if (license == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "License Not Found !!!",
                    MessageEnglish = "License Not Found !!!",
                    MessageArabic = "الترخيص غير موجود !!!"
                };
            }

            if (!CheckLicense(model).Success)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = CheckLicense(model).Message,
                    MessageArabic = CheckLicense(model).MessageArabic,
                    MessageEnglish = CheckLicense(model).MessageEnglish,
                };
            }
            var office = license?.Office;
            ////////
            var sp = _db.Specialities.Where(x => model.Specialities.Select(x => x.Id).ToList().Contains(x.Id)).ToList();
            foreach (var speciality in license.Specialities)
            {
                license.Specialities.Remove(speciality);
            }

            foreach (var newsp in sp)
            {
                license.Specialities.Add(newsp);
            }
            license.StartDate = model.StartDate;
            license.EndDate = model.EndDate;
            license.OfficeEntityId = model.OfficeEntityId;
            license.IsApproved = true;
            license.IsPending = false;
            license.IsRejected = false;
            _db.Entry(license).State = EntityState.Modified;
            ///////////////
            ///
            if (license.IsFirst == true)
            {
                PostFeesForNewOffice(office, license);
                office.IsActive = true;
                office.EstablishmentDate = model.StartDate;
                office.MembershipEndDate = new DateTime(model.StartDate.Year, 12, 31).AddYears(2);

            }

            ///////////////
            office.EntityId = model.OfficeEntityId;
            office.LicenseEndDate = model.EndDate;
            office.IsVerified = true;
            var officeSpecialities = _db.OfficeSpecialities.Where(x => x.OfficeId == office.Id).ToList();
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
            _db.Entry(office).State = EntityState.Modified;
            _db.SaveChanges();

            return new ResultWithMessage { Success = true, Result = license };
        }

        public ResultWithMessage PutLicense(int id, License model)
        {

            if (id != model.Id)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Invalid Model !!!",
                    MessageEnglish = "Invalid Model !!!",
                    MessageArabic = "نموذج غير صالح !!!"
                };
            }
            var license = _db.Licenses?.Include(x => x.Specialities).Include(x => x.Office).FirstOrDefault(x => x.Id == id);

            if (license == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "License Not Found !!!",
                    MessageEnglish = "License Not Found !!!",
                    MessageArabic = "الترخيص غير موجود !!!"
                };
            }

            if (!CheckLicense(model).Success)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = CheckLicense(model).Message,
                    MessageArabic = CheckLicense(model).MessageArabic,
                    MessageEnglish = CheckLicense(model).MessageEnglish
                };
            }
            var office = license?.Office;
            ////////
            var sp = _db.Specialities.Where(x => model.Specialities.Select(x => x.Id).ToList().Contains(x.Id)).ToList();
            foreach (var speciality in license.Specialities)
            {
                license.Specialities.Remove(speciality);
            }

            foreach (var newsp in sp)
            {
                license.Specialities.Add(newsp);
            }
            license.StartDate = model.StartDate;
            license.EndDate = model.EndDate;
            license.OfficeEntityId = model.OfficeEntityId;
            license.IsApproved = true;
            license.IsPending = false;
            license.IsRejected = false;
            _db.Entry(license).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = license };
        }

        public ResultWithMessage CalculationFeesForNewOffice(int officeid)
        {
            var office = _db.Offices.FirstOrDefault(x => x.Id == officeid);
            if (office == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Office Not Found !!!",
                    MessageEnglish = "Office Not Found !!!",
                    MessageArabic = "المكتب غير موجود !!!"
                };
            }
            if (office.MembershipEndDate == null)
            {
                var firstlicense = _db.Licenses.Include(x => x.OfficeEntity)
                                            .FirstOrDefault(x => x.OfficeId == officeid
                                                            && x.IsFirst == true
                                                            && x.IsRejected == false);
                if (firstlicense == null)
                {
                    return new ResultWithMessage
                    {
                        Success = false,
                        Message = "First License Not Found !!!",
                        MessageEnglish = "First License Not Found !!!",
                        MessageArabic = "الترخيص الأول غير موجود !!!"
                    };
                }
                var notfirstlicense = _db.Licenses.FirstOrDefault(x => x.OfficeId == officeid
                                                                && x.IsFirst == false
                                                                && x.IsPending == false
                                                                && x.IsApproved == true
                                                                && x.IsRejected == false);
                if (notfirstlicense != null)
                {
                    return new ResultWithMessage
                    {
                        Success = false,
                        Message = "Frist Fees Not Eligible !!!",
                        MessageEnglish = "Frist Fees Not Eligible !!!",
                        MessageArabic = "الرسوم الأولى غير مؤهلة !!!"
                    };
                }
                var DefaultFirstRegistrationYears = int.Parse(_configuration.GetValue<string>("DefaultFirstRegistrationYears"));
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
                    YearsCount = 0,
                    MembershipEndDate = null,
                });
                var YearlyFees = firstlicense.OfficeEntity.YearlyFees;
                var period = TimeZoneInfo.ConvertTimeFromUtc(firstlicense.StartDate, timezone).Month < 7 ? DefaultFirstRegistrationYears : DefaultFirstRegistrationYears - 0.5;
                var fee = period * YearlyFees;
                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 1,
                    RequestNameArabic = "رسوم الاشتراك الأول لمدة (" + period + ") سنوات",
                    RequestNameEnglish = "First Registration Fees For (" + period + ") Years",
                    PaymentDate = DateTime.UtcNow,
                    Amount = fee,
                    IsPaid = false,
                    YearsCount = period,
                    MembershipEndDate = new DateTime(firstlicense.StartDate.Year, 12, 31).AddYears(DefaultFirstRegistrationYears - 1),
                });
                return new ResultWithMessage
                {
                    Success = true,
                    Result = new OfficePaymentViewModel
                    {
                        StatusNameEnglish = "New",
                        StatusNameArabic = "مكتب جديد",
                        CurrentMembershipEndDate = office.MembershipEndDate,
                        NextMembershipEndDate = new DateTime(firstlicense.StartDate.Year, 12, 31).AddYears(DefaultFirstRegistrationYears - 1),
                        TotalAmount = fees.Sum(x => x.Amount),
                        Payments = fees
                    }
                };
            }
            else
            {
                return new ResultWithMessage
                {
                    Success = true,
                    Result = new OfficePaymentViewModel
                    {
                        StatusNameEnglish = office.MembershipEndDate < DateTime.UtcNow ? "In Active" : "Active",
                        StatusNameArabic = office.MembershipEndDate < DateTime.UtcNow ? "غير فعال" : "فعال",
                        CurrentMembershipEndDate = office.MembershipEndDate,
                        NextMembershipEndDate = null,
                        TotalAmount = 0,
                        Payments = new List<OfficePayment>()
                    }
                };
            }

        }

        public ResultWithMessage CalculationFeesForNewOfficeByLisense(License model)
        {
            var office = _db.Offices.FirstOrDefault(x => x.Id == model.OfficeId);
            var entity = _db.OfficeEntities.FirstOrDefault(x => x.Id == model.OfficeEntityId);
            if (office == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Office Not Found !!!",
                    MessageEnglish = "Office Not Found !!!",
                    MessageArabic = "المكتب غير موجود !!!"
                };
            }
            if (entity == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Entity Not Found !!!",
                    MessageEnglish = "Entity Not Found !!!",
                    MessageArabic = "الكيان غير موجود !!!"
                };
            }
            if (office.MembershipEndDate == null)
            {
                var DefaultFirstRegistrationYears = int.Parse(_configuration.GetValue<string>("DefaultFirstRegistrationYears"));
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
                    YearsCount = 0,
                    MembershipEndDate = null,
                });
                var YearlyFees = entity.YearlyFees;
                var period = TimeZoneInfo.ConvertTimeFromUtc(model.StartDate, timezone).Month < 7 ? DefaultFirstRegistrationYears : DefaultFirstRegistrationYears - 0.5;
                var fee = period * YearlyFees;
                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 1,
                    RequestNameArabic = "رسوم الاشتراك الأول لمدة (" + period + ") سنوات",
                    RequestNameEnglish = "First Registration Fees For (" + period + ") Years",
                    PaymentDate = DateTime.UtcNow,
                    Amount = fee,
                    IsPaid = false,
                    YearsCount = period,
                    MembershipEndDate = new DateTime(model.StartDate.Year, 12, 31).AddYears(DefaultFirstRegistrationYears - 1),
                });
                return new ResultWithMessage
                {
                    Success = true,
                    Result = new OfficePaymentViewModel
                    {
                        StatusNameEnglish = "New",
                        StatusNameArabic = "مكتب جديد",
                        CurrentMembershipEndDate = office.MembershipEndDate,
                        NextMembershipEndDate = new DateTime(model.StartDate.Year, 12, 31).AddYears(DefaultFirstRegistrationYears - 1),
                        TotalAmount = fees.Sum(x => x.Amount),
                        Payments = fees
                    }
                };
            }
            else
            {
                return new ResultWithMessage
                {
                    Success = true,
                    Result = new OfficePaymentViewModel
                    {
                        StatusNameEnglish = office.MembershipEndDate < DateTime.UtcNow ? "In Active" : "Active",
                        StatusNameArabic = office.MembershipEndDate < DateTime.UtcNow ? "غير فعال" : "فعال",
                        CurrentMembershipEndDate = office.MembershipEndDate,
                        NextMembershipEndDate = null,
                        TotalAmount = 0,
                        Payments = new List<OfficePayment>()
                    }
                };
            }

        }

        private ResultWithMessage PostFeesForNewOffice(Office office, License license)
        {
            if (office.MembershipEndDate == null)
            {
                var firstlicense = license.IsFirst == true
                                                            && license.IsPending == false
                                                            && license.IsApproved == true
                                                            && license.IsRejected == false;
                if (!firstlicense)
                {
                    return new ResultWithMessage
                    {
                        Success = false,
                        Message = "First License Not Found !!!",
                        MessageEnglish = "First License Not Found !!!",
                        MessageArabic = "الترخيص الأول غير موجود !!!"
                    };
                }
                var notfirstlicense = license.IsFirst == false
                                                                && license.IsPending == false
                                                                && license.IsApproved == true
                                                                && license.IsRejected == false;
                if (notfirstlicense)
                {
                    return new ResultWithMessage
                    {
                        Success = false,
                        Message = "First License Not Found !!!",
                        MessageEnglish = "First License Not Found !!!",
                        MessageArabic = "الترخيص الأول غير موجود !!!"
                    };
                }
                var DefaultFirstRegistrationYears = int.Parse(_configuration.GetValue<string>("DefaultFirstRegistrationYears"));
                var fees = new List<OfficePayment>();
                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 1,
                    RequestNameArabic = "رسم انتساب ",
                    RequestNameEnglish = "Registration Fees",
                    PaymentDate = DateTime.UtcNow,
                    Amount = 100,
                    YearsCount = 0,
                    MembershipEndDate = null,
                    IsPaid = true,
                });
                var YearlyFees = license.OfficeEntity.YearlyFees;
                var period = TimeZoneInfo.ConvertTimeFromUtc(license.StartDate, timezone).Month < 7 ? DefaultFirstRegistrationYears : DefaultFirstRegistrationYears - 0.5;
                var fee = period * YearlyFees;
                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 1,
                    RequestNameArabic = "رسوم الاشتراك الأول لمدة (" + period + ") سنوات",
                    RequestNameEnglish = "First Registration Fees For (" + period + ") Years",
                    PaymentDate = DateTime.UtcNow,
                    Amount = fee,
                    YearsCount = period,
                    MembershipEndDate = new DateTime(license.StartDate.Year, 12, 31).AddYears(DefaultFirstRegistrationYears - 1),
                    IsPaid = true,

                });
                office.OfficePayments = fees;
            }
            else
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "No Fees Posted !!!",
                    MessageEnglish = "No Fees Posted !!!",
                    MessageArabic = "لم يتم إضافة أي رسوم !!!"
                };
            }
            return new ResultWithMessage { Success = true, Result = office };
        }

        public ResultWithMessage GetAllPendingLicenses()
        {
            var licenses = _db.Licenses.Include(x => x.Office)
                                        .Include(x => x.OfficeEntity)
                                        .Where(x => x.IsPending == true && x.IsRejected == false && x.IsApproved == false)
                                        .OrderBy(x => x.CreatedDate)
                                        .Select(x => new LicenseViewModel(x)).ToList();
            return new ResultWithMessage { Success = true, Result = licenses };

        }

        public async Task<ResultWithMessage> RejectLicense(int id)
        {
            var license = _db.Licenses.Include(x => x.Specialities).FirstOrDefault(x => x.Id == id);
            if (license == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "License Not Found !!!",
                    MessageEnglish = "License Not Found !!!",
                    MessageArabic = "الترخيص غير موجود !!!"
                };
            }
            if (license.IsPending == false)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Can't Reject License !!!",
                    MessageEnglish = "Can't Reject License !!!",
                    MessageArabic = "لا يمكن رفض الترخيص !!!"
                };
            }
            license.Specialities.Clear();
            license.IsRejected = true;
            license.IsPending = false;
            license.IsApproved = false;
            if (!string.IsNullOrEmpty(license.DocumentUrl))
            {
                var deletedfile = await _fileService.DeleteFile(license.DocumentUrl);
            }

            _db.Entry(license).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true };
        }

        public ResultWithMessage CalculationFeesForRenew(int officeid, bool ispaid)
        {
            var office = _db.Offices?.Include(x => x.Entity).FirstOrDefault(x => x.Id == officeid);
            if (office == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Office Not Found !!!",
                    MessageEnglish = "Office Not Found !!!",
                    MessageArabic = "المكتب غير موجود "

                };
            }
            if (office.IsActive == false ||
                office.IsVerified == false)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Office Not Active Or Not IsVerified !!!",
                    MessageEnglish = "Office Not Active Or Not IsVerified !!!",
                    MessageArabic = "المكتب غير فعال أو غير موثق  "

                };
            }
            if (office.LicenseEndDate == null ||
                office.MembershipEndDate == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Office License Date Or Membership Date Not Found !!!",
                    MessageEnglish = "Office License Date Or Membership Date Not Found !!!",
                    MessageArabic = "خطأ في تاريخ نهاية الرخصة أو تاريخ نهاية الاشتراك"

                };
            }

            var lastLicense = _db.Licenses?.FirstOrDefault(x => x.OfficeId == officeid && x.IsLast == true && x.IsApproved == true);
            if (lastLicense == null)
            {
                return new ResultWithMessage { Success = false, Message = "Office Last Approved License Not Found !!!" };
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Office Last Approved License Not Found !!!",
                    MessageEnglish = "Office Last Approved License Not Found !!!",
                    MessageArabic = "أخر رخصة موافق عليها غير موجودة"

                };
            }
            var fees = new List<OfficePayment>();


            var membershipenddate = office.MembershipEndDate.Value;
            var membershipenddateutc = TimeZoneInfo.ConvertTimeFromUtc(office.MembershipEndDate.Value, timezone);
            var lastLicenseenddate = lastLicense.EndDate;
            var currentdate = DateTime.UtcNow;
            var newmembershipenddate = new DateTime();

            if (membershipenddate.Date < lastLicenseenddate.Date
                && membershipenddateutc.Year >= currentdate.Year)
            {

                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 2,
                    RequestNameArabic = "رسوم تجديد اشتراك لفترة الانقطاع لمدة (" + (lastLicenseenddate.Year - membershipenddate.Year) + ") سنوات",
                    RequestNameEnglish = "Missing Period Registration Fees For (" + (lastLicenseenddate.Year - membershipenddate.Year) + ") Years",
                    PaymentDate = DateTime.UtcNow,
                    Amount = office.Entity.YearlyFees * (lastLicenseenddate.Year - membershipenddate.Year),
                    YearsCount = 0,
                    MembershipEndDate = null,
                    IsPaid = ispaid,
                });


                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 2,
                    RequestNameArabic = "رسوم تجديد اشتراك لمدة (" + office.RenewYears + ") سنوات",
                    RequestNameEnglish = "Renew Registration Fees For (" + office.RenewYears + ") Years",
                    PaymentDate = DateTime.UtcNow,
                    Amount = office.Entity.YearlyFees * office.RenewYears,
                    YearsCount = office.RenewYears,
                    MembershipEndDate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (lastLicenseenddate.Year - membershipenddate.Year)),
                    IsPaid = ispaid,
                });
                newmembershipenddate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (lastLicenseenddate.Year - membershipenddate.Year));
            }
            else if (membershipenddate.Date < lastLicenseenddate.Date
               && membershipenddateutc.Year < currentdate.Year)
            {
                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 2,
                    RequestNameArabic = "رسوم تجديد اشتراك لفترة الانقطاع لمدة (" + (currentdate.Year - membershipenddateutc.Year) + ") سنوات",
                    RequestNameEnglish = "Missing Period Registration Fees For (" + (currentdate.Year - membershipenddateutc.Year) + ") Years",
                    PaymentDate = DateTime.UtcNow,
                    Amount = office.Entity.YearlyFees * (currentdate.Year - membershipenddateutc.Year),
                    YearsCount = 0,
                    MembershipEndDate = null,
                    IsPaid = ispaid,
                });


                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 2,
                    RequestNameArabic = "رسوم تجديد اشتراك لمدة (" + office.RenewYears + ") سنوات",
                    RequestNameEnglish = "Renew Registration Fees For (" + office.RenewYears + ") Years",
                    PaymentDate = DateTime.UtcNow,
                    Amount = office.Entity.YearlyFees * office.RenewYears,
                    YearsCount = office.RenewYears,
                    MembershipEndDate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (currentdate.Year - membershipenddateutc.Year)),
                    IsPaid = ispaid,
                });
                newmembershipenddate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (currentdate.Year - membershipenddateutc.Year));
            }
            if(membershipenddate.Date >= lastLicenseenddate.Date
                && membershipenddateutc.Year >= currentdate.Year)
            {
                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 2,
                    RequestNameArabic = "رسوم تجديد اشتراك لمدة (" + office.RenewYears + ") سنوات",
                    RequestNameEnglish = "Renew Registration Fees For (" + office.RenewYears + ") Years",
                    PaymentDate = DateTime.UtcNow,
                    Amount = office.Entity.YearlyFees * office.RenewYears,
                    YearsCount = office.RenewYears,
                    MembershipEndDate = office.MembershipEndDate.Value.AddYears(office.RenewYears),
                    IsPaid = ispaid,
                });
                newmembershipenddate = office.MembershipEndDate.Value.AddYears(office.RenewYears);
            }
            else if(membershipenddate.Date >= lastLicenseenddate.Date
                && membershipenddateutc.Year < currentdate.Year)
            {
                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 2,
                    RequestNameArabic = "رسوم تجديد اشتراك لفترة الانقطاع لمدة (" + (currentdate.Year - membershipenddateutc.Year) + ") سنوات",
                    RequestNameEnglish = "Missing Period Registration Fees For (" + (currentdate.Year - membershipenddateutc.Year) + ") Years",
                    PaymentDate = DateTime.UtcNow,
                    Amount = office.Entity.YearlyFees * (currentdate.Year - membershipenddateutc.Year),
                    YearsCount = 0,
                    MembershipEndDate = null,
                    IsPaid = ispaid,
                });


                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 2,
                    RequestNameArabic = "رسوم تجديد اشتراك لمدة (" + office.RenewYears + ") سنوات",
                    RequestNameEnglish = "Renew Registration Fees For (" + office.RenewYears + ") Years",
                    PaymentDate = DateTime.UtcNow,
                    Amount = office.Entity.YearlyFees * office.RenewYears,
                    YearsCount = office.RenewYears,
                    MembershipEndDate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (currentdate.Year - membershipenddateutc.Year)),
                    IsPaid = ispaid,
                });
                newmembershipenddate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (currentdate.Year - membershipenddateutc.Year));
            }
            if (ispaid == true)
            {
                office.MembershipEndDate = newmembershipenddate;
                office.IsActive = true;
                office.IsVerified = true;
                _db.OfficePayments.AddRange(fees);
                _db.Entry(office).State = EntityState.Modified;
                _db.SaveChanges();
            }

            return new ResultWithMessage
            {
                Success = true,
                Result = new OfficePaymentViewModel
                {
                    StatusNameEnglish = office.MembershipEndDate < DateTime.UtcNow ? "In Active" : "Active",
                    StatusNameArabic = office.MembershipEndDate < DateTime.UtcNow ? "غير فعال" : "فعال",
                    CurrentMembershipEndDate = office.MembershipEndDate.Value,
                    NextMembershipEndDate = fees.FirstOrDefault(x => x.RequestNameEnglish.StartsWith("Renew Registration Fees For")).MembershipEndDate,
                    TotalAmount = fees.Sum(x => x.Amount),
                    Payments = fees
                }
            };
        }

        public ResultWithMessage GetAllOfficePayments(int officeid)
        {
            var officepayments = _db.OfficePayments?
                                 .Include(x => x.Type)
                                 .Where(x => x.OfficeId == officeid)
                                 .OrderByDescending(x => x.PaymentDate).Select(x => new OfficePaymentWithTypeViewModel(x)).ToList();
            return new ResultWithMessage { Success = true, Result = officepayments };
        }
    }
}
