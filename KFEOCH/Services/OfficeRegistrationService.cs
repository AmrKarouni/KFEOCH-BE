using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Dictionaries;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KFEOCH.Services
{
    public class OfficeRegistrationService : IOfficeRegistrationService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly TimeZoneInfo timezone;
        private readonly IKnetPaymentService _knetPaymentService;
        private readonly IUserService _userService;
        public OfficeRegistrationService(ApplicationDbContext db,
                                         IHttpContextAccessor httpContextAccessor,
                                         IFileService fileService,
                                         IConfiguration configuration,
                                         IKnetPaymentService knetPaymentService,
                                         IUserService userService)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
            _configuration = configuration;
            timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            _knetPaymentService = knetPaymentService;
            _userService = userService;
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

        public async Task<ResultWithMessage> PostLicense(License model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, model.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
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
                office.MembershipEndDate = new DateTime(TimeZoneInfo.ConvertTimeFromUtc(license.StartDate, timezone).Year, 12, 31).AddYears(2);

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

        public async Task<ResultWithMessage> PutLicense(int id, License model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, model.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
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
                    MembershipEndDate = new DateTime(TimeZoneInfo.ConvertTimeFromUtc(firstlicense.StartDate, timezone).Year, 12, 31).AddYears(DefaultFirstRegistrationYears - 1),
                });
                return new ResultWithMessage
                {
                    Success = true,
                    Result = new OfficePaymentViewModel
                    {
                        StatusNameEnglish = "New",
                        StatusNameArabic = "مكتب جديد",
                        CurrentMembershipEndDate = office.MembershipEndDate,
                        NextMembershipEndDate = new DateTime(TimeZoneInfo.ConvertTimeFromUtc(firstlicense.StartDate, timezone).Year, 12, 31).AddYears(DefaultFirstRegistrationYears - 1),
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

        public async Task<ResultWithMessage> CalculationFeesForNewOfficeByLisense(License model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, model.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
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
                    MembershipEndDate = new DateTime(TimeZoneInfo.ConvertTimeFromUtc(model.StartDate, timezone).Year, 12, 31).AddYears(DefaultFirstRegistrationYears - 1),
                });
                return new ResultWithMessage
                {
                    Success = true,
                    Result = new OfficePaymentViewModel
                    {
                        StatusNameEnglish = "New",
                        StatusNameArabic = "مكتب جديد",
                        CurrentMembershipEndDate = office.MembershipEndDate,
                        NextMembershipEndDate = new DateTime(TimeZoneInfo.ConvertTimeFromUtc(model.StartDate, timezone).Year, 12, 31).AddYears(DefaultFirstRegistrationYears - 1),
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
                var paymentdate = DateTime.UtcNow;
                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 1,
                    RequestNameArabic = "رسم انتساب ",
                    RequestNameEnglish = "Registration Fees",
                    PaymentDate = paymentdate,
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
                    PaymentDate = paymentdate,
                    Amount = fee,
                    YearsCount = period,
                    MembershipEndDate = new DateTime(TimeZoneInfo.ConvertTimeFromUtc(license.StartDate, timezone).Year, 12, 31).AddYears(DefaultFirstRegistrationYears - 1),
                    IsPaid = true,
                    PaymentCategoryArabic = "تجديد اشتراك",
                    PaymentCategoryEnglish = "Renew Registration",

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

        //public ResultWithMessage CalculationFeesForRenew(int officeid, bool ispaid)
        //{
        //    var office = _db.Offices?.Include(x => x.Entity).FirstOrDefault(x => x.Id == officeid);
        //    if (office == null)
        //    {
        //        return new ResultWithMessage
        //        {
        //            Success = false,
        //            Message = "Office Not Found !!!",
        //            MessageEnglish = "Office Not Found !!!",
        //            MessageArabic = "المكتب غير موجود "

        //        };
        //    }
        //    if (office.IsActive == false ||
        //        office.IsVerified == false)
        //    {
        //        return new ResultWithMessage
        //        {
        //            Success = false,
        //            Message = "Office Not Active Or Not IsVerified !!!",
        //            MessageEnglish = "Office Not Active Or Not IsVerified !!!",
        //            MessageArabic = "المكتب غير فعال أو غير موثق  "

        //        };
        //    }
        //    if (office.LicenseEndDate == null ||
        //        office.MembershipEndDate == null)
        //    {
        //        return new ResultWithMessage
        //        {
        //            Success = false,
        //            Message = "Office License Date Or Membership Date Not Found !!!",
        //            MessageEnglish = "Office License Date Or Membership Date Not Found !!!",
        //            MessageArabic = "خطأ في تاريخ نهاية الرخصة أو تاريخ نهاية الاشتراك"

        //        };
        //    }

        //    var lastLicense = _db.Licenses?.FirstOrDefault(x => x.OfficeId == officeid && x.IsLast == true && x.IsApproved == true);
        //    if (lastLicense == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = "Office Last Approved License Not Found !!!" };
        //        return new ResultWithMessage
        //        {
        //            Success = false,
        //            Message = "Office Last Approved License Not Found !!!",
        //            MessageEnglish = "Office Last Approved License Not Found !!!",
        //            MessageArabic = "أخر رخصة موافق عليها غير موجودة"

        //        };
        //    }
        //    var fees = new List<OfficePayment>();


        //    var membershipenddate = office.MembershipEndDate.Value;
        //    var membershipenddateutc = TimeZoneInfo.ConvertTimeFromUtc(office.MembershipEndDate.Value, timezone);
        //    var lastLicenseenddate = lastLicense.EndDate;
        //    var currentdate = DateTime.UtcNow;
        //    var paymentdate = DateTime.UtcNow;
        //    var newmembershipenddate = new DateTime();

        //    if (membershipenddate.Date < lastLicenseenddate.Date
        //        && membershipenddateutc.Year >= currentdate.Year)
        //    {

        //        fees.Add(new OfficePayment
        //        {
        //            OfficeId = office.Id,
        //            TypeId = 2,
        //            RequestNameArabic = "رسوم تجديد اشتراك لفترة الانقطاع لمدة (" + (lastLicenseenddate.Year - membershipenddate.Year) + ") سنوات",
        //            RequestNameEnglish = "Missing Period Registration Fees For (" + (lastLicenseenddate.Year - membershipenddate.Year) + ") Years",
        //            PaymentDate = paymentdate,
        //            Amount = office.Entity.YearlyFees * (lastLicenseenddate.Year - membershipenddate.Year),
        //            YearsCount = 0,
        //            MembershipEndDate = null,
        //            IsPaid = ispaid,
        //        });


        //        fees.Add(new OfficePayment
        //        {
        //            OfficeId = office.Id,
        //            TypeId = 2,
        //            RequestNameArabic = "رسوم تجديد اشتراك لمدة (" + office.RenewYears + ") سنوات",
        //            RequestNameEnglish = "Renew Registration Fees For (" + office.RenewYears + ") Years",
        //            PaymentDate = paymentdate,
        //            Amount = office.Entity.YearlyFees * office.RenewYears,
        //            YearsCount = office.RenewYears,
        //            MembershipEndDate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (lastLicenseenddate.Year - membershipenddate.Year)),
        //            IsPaid = ispaid,
        //        });
        //        newmembershipenddate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (lastLicenseenddate.Year - membershipenddate.Year));
        //    }
        //    else if (membershipenddate.Date < lastLicenseenddate.Date
        //       && membershipenddateutc.Year < currentdate.Year)
        //    {
        //        fees.Add(new OfficePayment
        //        {
        //            OfficeId = office.Id,
        //            TypeId = 2,
        //            RequestNameArabic = "رسوم تجديد اشتراك لفترة الانقطاع لمدة (" + (currentdate.Year - membershipenddateutc.Year) + ") سنوات",
        //            RequestNameEnglish = "Missing Period Registration Fees For (" + (currentdate.Year - membershipenddateutc.Year) + ") Years",
        //            PaymentDate = paymentdate,
        //            Amount = office.Entity.YearlyFees * (currentdate.Year - membershipenddateutc.Year),
        //            YearsCount = 0,
        //            MembershipEndDate = null,
        //            IsPaid = ispaid,
        //        });


        //        fees.Add(new OfficePayment
        //        {
        //            OfficeId = office.Id,
        //            TypeId = 2,
        //            RequestNameArabic = "رسوم تجديد اشتراك لمدة (" + office.RenewYears + ") سنوات",
        //            RequestNameEnglish = "Renew Registration Fees For (" + office.RenewYears + ") Years",
        //            PaymentDate = paymentdate,
        //            Amount = office.Entity.YearlyFees * office.RenewYears,
        //            YearsCount = office.RenewYears,
        //            MembershipEndDate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (currentdate.Year - membershipenddateutc.Year)),
        //            IsPaid = ispaid,
        //        });
        //        newmembershipenddate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (currentdate.Year - membershipenddateutc.Year));
        //    }
        //    if(membershipenddate.Date >= lastLicenseenddate.Date
        //        && membershipenddateutc.Year >= currentdate.Year)
        //    {
        //        fees.Add(new OfficePayment
        //        {
        //            OfficeId = office.Id,
        //            TypeId = 2,
        //            RequestNameArabic = "رسوم تجديد اشتراك لمدة (" + office.RenewYears + ") سنوات",
        //            RequestNameEnglish = "Renew Registration Fees For (" + office.RenewYears + ") Years",
        //            PaymentDate = paymentdate,
        //            Amount = office.Entity.YearlyFees * office.RenewYears,
        //            YearsCount = office.RenewYears,
        //            MembershipEndDate = office.MembershipEndDate.Value.AddYears(office.RenewYears),
        //            IsPaid = ispaid,
        //        });
        //        newmembershipenddate = office.MembershipEndDate.Value.AddYears(office.RenewYears);
        //    }
        //    else if(membershipenddate.Date >= lastLicenseenddate.Date
        //        && membershipenddateutc.Year < currentdate.Year)
        //    {
        //        fees.Add(new OfficePayment
        //        {
        //            OfficeId = office.Id,
        //            TypeId = 2,
        //            RequestNameArabic = "رسوم تجديد اشتراك لفترة الانقطاع لمدة (" + (currentdate.Year - membershipenddateutc.Year) + ") سنوات",
        //            RequestNameEnglish = "Missing Period Registration Fees For (" + (currentdate.Year - membershipenddateutc.Year) + ") Years",
        //            PaymentDate = paymentdate,
        //            Amount = office.Entity.YearlyFees * (currentdate.Year - membershipenddateutc.Year),
        //            YearsCount = 0,
        //            MembershipEndDate = null,
        //            IsPaid = ispaid,
        //        });


        //        fees.Add(new OfficePayment
        //        {
        //            OfficeId = office.Id,
        //            TypeId = 2,
        //            RequestNameArabic = "رسوم تجديد اشتراك لمدة (" + office.RenewYears + ") سنوات",
        //            RequestNameEnglish = "Renew Registration Fees For (" + office.RenewYears + ") Years",
        //            PaymentDate = paymentdate,
        //            Amount = office.Entity.YearlyFees * office.RenewYears,
        //            YearsCount = office.RenewYears,
        //            MembershipEndDate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (currentdate.Year - membershipenddateutc.Year)),
        //            IsPaid = ispaid,
        //        });
        //        newmembershipenddate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (currentdate.Year - membershipenddateutc.Year));
        //    }
        //    if (ispaid == true)
        //    {
        //        office.MembershipEndDate = newmembershipenddate;
        //        office.IsActive = true;
        //        office.IsVerified = true;
        //        _db.OfficePayments.AddRange(fees);
        //        _db.Entry(office).State = EntityState.Modified;
        //        _db.SaveChanges();
        //    }

        //    return new ResultWithMessage
        //    {
        //        Success = true,
        //        Result = new OfficePaymentViewModel
        //        {
        //            StatusNameEnglish = office.MembershipEndDate < DateTime.UtcNow ? "In Active" : "Active",
        //            StatusNameArabic = office.MembershipEndDate < DateTime.UtcNow ? "غير فعال" : "فعال",
        //            CurrentMembershipEndDate = office.MembershipEndDate.Value,
        //            NextMembershipEndDate = fees.FirstOrDefault(x => x.RequestNameEnglish.StartsWith("Renew Registration Fees For")).MembershipEndDate,
        //            TotalAmount = fees.Sum(x => x.Amount),
        //            Payments = fees
        //        }
        //    };
        //}


        public async Task<ResultWithMessage> CalculationFeesForRenew(int officeid, bool ispaid, string lang, string returnUrl)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, officeid);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
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
            var paymentdate = DateTime.UtcNow;
            var newmembershipenddate = new DateTime();
            var renewYears = 0;
            var missedYears = 0;
            if (membershipenddate.Date < lastLicenseenddate.Date
                && membershipenddateutc.Year >= currentdate.Year)
            {
                renewYears = office.RenewYears;
                missedYears = (lastLicenseenddate.Year - membershipenddate.Year);
                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 2,
                    RequestNameArabic = "رسوم تجديد الاشتراك لفترة الانقطاع لمدة (" + (lastLicenseenddate.Year - membershipenddate.Year) + ") سنوات",
                    RequestNameEnglish = "Missing Period Registration Fees For (" + (lastLicenseenddate.Year - membershipenddate.Year) + ") Years",
                    //PaymentDate = paymentdate,
                    Amount = office.Entity.YearlyFees * (lastLicenseenddate.Year - membershipenddate.Year),
                    YearsCount = 0,
                    MembershipEndDate = null,
                    IsPaid = false
                });


                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 2,
                    RequestNameArabic = "رسوم تجديد الاشتراك لمدة (" + office.RenewYears + ") سنوات",
                    RequestNameEnglish = "Renew Registration Fees For (" + office.RenewYears + ") Years",
                    //PaymentDate = paymentdate,
                    Amount = office.Entity.YearlyFees * office.RenewYears,
                    YearsCount = office.RenewYears,
                    MembershipEndDate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (lastLicenseenddate.Year - membershipenddate.Year)),
                    IsPaid = false
                });
                newmembershipenddate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (lastLicenseenddate.Year - membershipenddate.Year));
            }
            else if (membershipenddate.Date < lastLicenseenddate.Date
               && membershipenddateutc.Year < currentdate.Year)
            {
                renewYears = office.RenewYears;
                missedYears = (currentdate.Year - membershipenddateutc.Year);
                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 2,
                    RequestNameArabic = "رسوم تجديد الاشتراك لفترة الانقطاع لمدة (" + (currentdate.Year - membershipenddateutc.Year) + ") سنوات",
                    RequestNameEnglish = "Missing Period Registration Fees For (" + (currentdate.Year - membershipenddateutc.Year) + ") Years",
                    //PaymentDate = paymentdate,
                    Amount = office.Entity.YearlyFees * (currentdate.Year - membershipenddateutc.Year),
                    YearsCount = 0,
                    MembershipEndDate = null,
                    IsPaid = false
                });


                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 2,
                    RequestNameArabic = "رسوم تجديد الاشتراك لمدة (" + office.RenewYears + ") سنوات",
                    RequestNameEnglish = "Renew Registration Fees For (" + office.RenewYears + ") Years",
                    //PaymentDate = paymentdate,
                    Amount = office.Entity.YearlyFees * office.RenewYears,
                    YearsCount = office.RenewYears,
                    MembershipEndDate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (currentdate.Year - membershipenddateutc.Year)),
                    IsPaid = false
                });
                newmembershipenddate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (currentdate.Year - membershipenddateutc.Year));
            }
            if (membershipenddate.Date >= lastLicenseenddate.Date
                && membershipenddateutc.Year >= currentdate.Year)
            {
                renewYears = office.RenewYears;
                missedYears = 0;
                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 2,
                    RequestNameArabic = "رسوم تجديد الاشتراك لمدة (" + office.RenewYears + ") سنوات",
                    RequestNameEnglish = "Renew Registration Fees For (" + office.RenewYears + ") Years",
                    //PaymentDate = paymentdate,
                    Amount = office.Entity.YearlyFees * office.RenewYears,
                    YearsCount = office.RenewYears,
                    MembershipEndDate = office.MembershipEndDate.Value.AddYears(office.RenewYears),
                    IsPaid = false
                });
                newmembershipenddate = office.MembershipEndDate.Value.AddYears(office.RenewYears);
            }
            else if (membershipenddate.Date >= lastLicenseenddate.Date
                && membershipenddateutc.Year < currentdate.Year)
            {
                renewYears = office.RenewYears;
                missedYears = (currentdate.Year - membershipenddateutc.Year);
                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 2,
                    RequestNameArabic = "رسوم تجديد الاشتراك لفترة الانقطاع لمدة (" + (currentdate.Year - membershipenddateutc.Year) + ") سنوات",
                    RequestNameEnglish = "Missing Period Registration Fees For (" + (currentdate.Year - membershipenddateutc.Year) + ") Years",
                    //PaymentDate = paymentdate,
                    Amount = office.Entity.YearlyFees * (currentdate.Year - membershipenddateutc.Year),
                    YearsCount = 0,
                    MembershipEndDate = null,
                    IsPaid = false
                });


                fees.Add(new OfficePayment
                {
                    OfficeId = office.Id,
                    TypeId = 2,
                    RequestNameArabic = "رسوم تجديد الاشتراك لمدة (" + office.RenewYears + ") سنوات",
                    RequestNameEnglish = "Renew Registration Fees For (" + office.RenewYears + ") Years",
                    //PaymentDate = paymentdate,
                    Amount = office.Entity.YearlyFees * office.RenewYears,
                    YearsCount = office.RenewYears,
                    MembershipEndDate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (currentdate.Year - membershipenddateutc.Year)),
                    IsPaid = false
                });
                newmembershipenddate = office.MembershipEndDate.Value.AddYears(office.RenewYears + (currentdate.Year - membershipenddateutc.Year));
            }
            if (ispaid == true)
            {
                //office.MembershipEndDate = newmembershipenddate;
                //office.IsActive = true;
                //office.IsVerified = true;
                _db.OfficePayments.AddRange(fees);
                //_db.Entry(office).State = EntityState.Modified;
                //_db.SaveChanges();

                var paymentResult = _knetPaymentService.GenerateRenewPayment(office.Id,
                                                       fees.Sum(x => x.Amount),
                                                       renewYears,
                                                       missedYears,
                                                       lang,
                                                       returnUrl);
                if (paymentResult == "Failed")
                {
                    return new ResultWithMessage
                    {
                        Success = false,
                        Message = "Error Connecting to Payment Gateway ",
                        MessageEnglish = "Error Connecting to Payment Gateway ",
                        MessageArabic = "حدث خطأ في الاتصال مع مخدم الدفع"
                    };
                }
                return new ResultWithMessage { Success = true, Result = new { PaymentUrl = paymentResult } };
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

        public ResultWithMessage GetAllOfficeRenewPayments(int officeid)
        {
            var officepayments = _db.OfficePayments?
                                 .Include(x => x.Type)
                                 .Where(x => x.OfficeId == officeid && x.PaymentCategoryEnglish.StartsWith("Renew Registration"))
                                 .OrderByDescending(x => x.PaymentDate).Select(x => new OfficePaymentWithTypeViewModel(x)).ToList();
            return new ResultWithMessage { Success = true, Result = officepayments };
        }

        public IEnumerable<DateTime> EachYear(DateTime from, DateTime thru)
        {
            for (var day = new DateTime(TimeZoneInfo.ConvertTimeFromUtc(from, timezone).Year, 12, 31); day.Date <= thru.Date; day = day.AddYears(1))
                yield return day;
        }
        public async Task<ResultWithMessage> GenerateRenewReceipt(int id)
        {
            
            var pay = _db.OfficePayments.Include(x => x.Office).FirstOrDefault(x => x.Id == id && x.PaymentCategoryEnglish == "Renew Registration");
            if (pay == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Payment Not Found",
                    MessageEnglish = "Payment Not Found",
                    MessageArabic = "الدفعة غير موجود"
                };
            }
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, pay.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
            List<string> years = new List<string>();
            foreach (DateTime day in EachYear(pay.Office.EstablishmentDate.Value, pay.Office.MembershipEndDate.Value))
            {
                years.Add(day.Year.ToString());
            }
            var cnt = years.Count;
            if (cnt > 22)
            {
                years = years.Skip(cnt - 22).Take(22).ToList();
            }
            else if (cnt < 22)
            {
                for (int i = cnt - 1; i < 22; i++)
                {
                    years.Add("");
                }
            }
            
            var html = $@"<html>

                        <head>
                            <meta charset='UTF-8'>
                        </head>
                        <table width=100%>
                            <tr>
                                <td colspan='4' align='center' style='font-weight: 600;'>

                                    <table style='width : 50mm; '>
                                        <tr>
                                            <td colspan='2' align='center' style='font-weight: 600;'>
                                                <div style='width: 50mm '>
                                                    <p style='margin-bottom: 2px;'>سند قبض</p>
                                                    <p style='margin-top: 2px; margin-bottom: 2px;'>Receipt Voucher </p>
                                                    <hr>
                                                </div>
                                        <tr>
                                        <tr>
                                            <td align='left' style='font-weight: 600;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>Knet</p>
                                                </div>
                                            </td>
                                            <td align='right' style='font-weight: 600;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>كي نت</p>
                                                </div>
                                            </td>
                                        <tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td colspan='4' align='right'>
                                    <table>
                                        <tr>
                                            <td align='left' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>No.:</p>
                                                </div>
                                            </td>
                                            <td align='center' style='padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px; width: 4cm;'>{pay.Id}</p>
                                                </div>
                                            </td>
                                            <td align='left' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>:رقم السند</p>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td colspan='3' align='right'>
                                    <table>
                                        <tr>
                                            <td align='left' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>Date and Time</p>
                                                </div>
                                            </td>
                                            <td align='center' style='padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px; width: 4cm;'>
                                                       {TimeZoneInfo.ConvertTimeFromUtc(pay.PaymentDate, timezone).ToShortTimeString()}
                                                        <span>
                                                            &nbsp;
                                                        </span>
                                                        {TimeZoneInfo.ConvertTimeFromUtc(pay.PaymentDate, timezone).ToShortDateString()}
                                                    </p>
                                                </div>
                                            </td>
                                            <td align='right' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>التاريخ و الوقت</p>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align='left' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>Membership No. </p>
                                                </div>
                                            </td>
                                            <td align='center' style='padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px; width: 4cm;'>{pay.Office.LicenseId}</p>
                                                </div>
                                            </td>
                                            <td align='right' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>رقم العضــوية</p>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align='right'>
                                    <table>
                                        <tr>
                                            <td align='left' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>Received From Mr.\ Messrs:</p>
                                                </div>
                                            </td>
                                            <td colspan='2' align='center' style='padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px; width: 8cm;'>{pay.Office.NameArabic}</p>
                                                </div>
                                            </td>
                                            <td align='right' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>:وصلنا من السيد / السادة</p>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td align='center' style=' padding-top: 1cm;'>

                                    <table
                                        style='border: 1px solid black;border-collapse: collapse; width: 95%; border-bottom: none; border-left: none; border-right: none;'>
                                        <tr style='border-bottom: 1px solid black;'>
                                            <th align='center'
                                                style='font-weight: 600; background-color:#ccc9c9; border-right:  1px solid black; border-left: 1px solid black;'>

                                                <p>م</p>
                                                <p>No.</p>

                                            </th>
                                            <th colspan='2' align='center'
                                                style='font-weight: 600; background-color:#ccc9c9; border-right:  1px solid black;'>

                                                <p style='width: 8cm;'>البيــــــان</p>
                                                <p style='width: 8cm;'>Description</p>

                                            </th>
                                            <th align='center'
                                                style='font-weight: 600; background-color:#ccc9c9; border-right:  1px solid black;'>

                                                <p>المبلغ</p>
                                                <p>Amount</p>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td align='center' style='border-right:  1px solid black;  border-left: 1px solid black;'>
                                                <div>
                                                    <p>1</p>
                                                </div>
                                            </td>
                                            <td colspan='2' align='right' style=' border-right: 1px solid black; padding-right: 10px;'>
                                                <div>
                                                    <p style='width: 8cm;'>{pay.RequestNameArabic}</p>
                                                </div>
                                            </td>
                                            <td align='center'
                                                style='border-left:  1px solid black;  border-right:  1px solid black;  padding-right: 10px;'>
                                                <div>
                                                    <p>{string.Format("{0:F2}", pay.Amount)}</p>
                                                </div>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan='3' align='right'
                                                style='font-weight: 600; border-top:  1px solid black; padding-right: 10px;'>
                                                <div>
                                                    <p style='width: 8cm;'> دينار كويتي فقط لاغير </p>
                                                </div>
                                            </td>
                                            <td align='center' style='font-weight: 700; border-top:  1px solid black;'>
                                                <div>
                                                    <p>{string.Format("{0:F2}", pay.Amount)}</p>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td align='center' style=' padding-top: 1cm;'>

                                    <table
                                        style='border: 1px solid black;border-collapse: collapse; width: 95%; border-bottom: none; border-left: none; border-right: none;'>
                                        <tr style='border-bottom: 1px solid black;'>
                                            <th align='left' colspan='6'
                                                style='font-weight: 600; background-color:#ccc9c9; border-right:  1px solid black; border-left: 1px solid black;'>

                                                <p>The annual subscription was paid for year :</p>
                                            </th>
                                            <th align='right' colspan='5'
                                                style='font-weight: 600; background-color:#ccc9c9; border-right:  1px solid black; border-left: 1px solid black;'>

                                                <p> : تم تسديد الاشتراك السنوي عن عام</p>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td align='center'
                                                style='border-right:  1px solid gray;  border-left: 1px solid black;border-bottom: 1px solid gray;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[10].ToString()) ? "" : years[10].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid gray;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[9].ToString()) ? "" : years[9].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid gray;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[8].ToString()) ? "" : years[8].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid gray;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[7].ToString()) ? "" : years[7].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid gray;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[6].ToString()) ? "" : years[6].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid gray;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[5].ToString()) ? "" : years[5].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid gray;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[4].ToString()) ? "" : years[4].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid gray;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[3].ToString()) ? "" : years[3].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid gray;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[2].ToString()) ? "" : years[2].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid gray;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[1].ToString()) ? "" : years[1].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid black;border-bottom: 1px solid gray;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[0].ToString()) ? "" : years[0].ToString())}</p>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align='center'
                                                style='border-right:  1px solid gray;  border-left: 1px solid black;border-bottom: 1px solid black;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[21].ToString()) ? "" : years[21].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid black;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[20].ToString()) ? "" : years[20].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid black;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[19].ToString()) ? "" : years[19].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid black;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[18].ToString()) ? "" : years[18].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid black;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[17].ToString()) ? "" : years[17].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid black;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[16].ToString()) ? "" : years[16].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid black;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[15].ToString()) ? "" : years[15].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid black;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[14].ToString()) ? "" : years[14].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid black;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[13].ToString()) ? "" : years[13].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid gray;border-bottom: 1px solid black;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[12].ToString()) ? "" : years[12].ToString())}</p>
                                                </div>
                                            </td>
                                            <td align='center' style='border-right:  1px solid black;border-bottom: 1px solid black;width: 9%; height: 25px;'>
                                                <div>
                                                    <p>{(string.IsNullOrEmpty(years[11].ToString()) ? "" : years[11].ToString())}</p>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>



                            <tr>
                                <td style='font-weight: 600; padding-top: 2cm;  padding-left: 1cm; '>
                                    <p>
                                        <span>
                                            Recept
                                        </span>
                                        <span>
                                            &nbsp;
                                        </span>
                                        <span>
                                            &nbsp;
                                        </span>
                                        <span>
                                            المستلم
                                        </span>
                                    </p>
                                </td>
                            </tr>

                            <tr>
                                <td align='center' style=' padding-top: 1cm;'>

                                    <table style='width: 95%;'>
                                        <tr>
                                            <td align='left' style='font-weight: 600;'>
                                                <div>
                                                    <p style='margin-bottom: 2px; width: 3cm;'>
                                                        {TimeZoneInfo.ConvertTimeFromUtc(pay.Office.MembershipEndDate.Value.AddDays(1), timezone).ToShortDateString()}</p>
                                                </div>
                                            </td>
                                            <td align='left' style='font-weight: 600;'> : مطلوب تجديد اشتراك العضوية اعتبارا من
                                            </td>
                                            <td align='right' style='font-weight: 600;'>
                                                <div>
                                                    <p style='margin-bottom: 2px; width: 3cm;'>
                                                        {TimeZoneInfo.ConvertTimeFromUtc(pay.Office.MembershipEndDate.Value,timezone).ToShortDateString()}</p>
                                                </div>
                                            </td>
                                            <td align='right' style='font-weight: 600;'> : تاريخ انتهاء اشتراك العضوية في
                                            </td>
                                        </tr>

                                    </table>

                                </td>
                            </tr>
                        </table>

                        </html>";
            pay.HtmlBody = html;
            _db.Entry(pay).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = new { Html = html } }; 

        }
    }
}
