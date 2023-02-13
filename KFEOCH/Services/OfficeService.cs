using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace KFEOCH.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly TimeZoneInfo timezone;
        public OfficeService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, IFileService fileService, IUserService userService)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
            timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            _userService = userService;
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

        public async Task<ResultWithMessage> PutOfficeInfo(int id, OfficePutBindingModel model)
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
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, model.Id);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
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
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, office.Id);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
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
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal,id);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
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

        public ResultWithMessage GetOfficesForAdmin(FilterModel model)
        {
            var list = new List<OfficeAdminViewModel>();
            var offices = _db.Offices?.Include(x => x.Type)
                                        .Include(x => x.Area)
                                        .Include(x => x.Governorate)
                                        .Include(x => x.Country)
                                        .Include(x => x.Entity)
                                        .Include(x => x.LegalEntity)
                                        .Include(x => x.OfficeSpecialities)
                                        .Include(x => x.OfficeActivities).ToList();
            if (model.Types?.Count() > 0)
            {
                offices = offices?.Where(x => model.Types.Contains(x.TypeId)).ToList();
            }

            if (model.Entities?.Count() > 0)
            {
                offices = offices?.Where(x => model.Entities.Contains(x.EntityId ?? 0)).ToList();
            }

            if (model.LegalEntities?.Count() > 0)
            {
                offices = offices?.Where(x => model.LegalEntities.Contains(x.LegalEntityId ?? 0)).ToList();
            }

            if (model.Specialities?.Count() > 0)
            {
                offices = offices?.Where(x => x.OfficeSpecialities.Select(x => x.SpecialityId.Value).Intersect(model.Specialities).Any()).ToList();
            }

            if (model.Activities?.Count() > 0)
            {
                offices = offices?.Where(x => x.OfficeActivities.Select(x => x.ActivityId.Value).Intersect(model.Activities).Any()).ToList();
            }
            if (model.IsActive != null)
            {
                offices = offices?.Where(x => x.IsActive == model.IsActive).ToList();
            }

            if (model.IsVerified != null)
            {
                offices = offices?.Where(x => x.IsVerified == model.IsVerified).ToList();
            } 
            if (!string.IsNullOrEmpty(model.SearchQuery))
            {
                if (long.TryParse(model.SearchQuery, out long license))
                {
                    offices = offices?.Where(x => x.LicenseId == license).ToList();
                }
                else
                {
                    offices = offices?.Where(x => x.NameArabic.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                 x.NameEnglish.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                 (x.Area != null &&
                                                                    (x.Area.NameArabic.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                                     x.Area.NameEnglish.ToLower().Contains(model.SearchQuery.ToLower()))) ||
                                                 (x.Governorate != null &&
                                                                    (x.Governorate.NameArabic.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                                     x.Governorate.NameEnglish.ToLower().Contains(model.SearchQuery.ToLower()))) ||
                                                 (x.Country != null &&
                                                                    (x.Country.NameArabic.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                                     x.Country.NameEnglish.ToLower().Contains(model.SearchQuery.ToLower()))) ||
                                                 x.Email.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                 x.PhoneNumber.ToLower().Contains(model.SearchQuery.ToLower())).ToList();
                }
            }

            var dataSize = offices.Count();
            var sortProperty = typeof(OfficeAdminViewModel).GetProperty(model?.Sort ?? "Id");
            if (model?.Order == "desc")
            {
                list = offices?.Select(o => new OfficeAdminViewModel(o)).OrderByDescending(x => sortProperty.GetValue(x)).ToList();
            }
            else
            {
                list = offices?.Select(o => new OfficeAdminViewModel(o)).OrderBy(x => sortProperty.GetValue(x)).ToList();
            }

            var result = list.Skip(model.PageSize * model.PageIndex).Take(model.PageSize).ToList();
            return new ResultWithMessage
            {
                Success = true,
                Message = "",
                Result = new ObservableData(result, dataSize)
            };
        }

        public ResultWithMessage GetOfficesPaymentsReportForAdmin(FilterModel model)
        {
            var list = new List<OfficesPaymentsReportViewModel>();
            var payments = _db.OfficePayments.Include(x => x.Office).ToList();
            
            if (model.OfficeTypes?.Count() > 0)
            {
                payments = payments?.Where(x => model.OfficeTypes.Contains(x.Office.TypeId)).ToList();
            }
            if (model.Offices?.Count() > 0)
            {
                payments = payments?.Where(x => model.Offices.Contains(x.OfficeId)).ToList();
            }
            if (!string.IsNullOrEmpty(model.PaymentCategoryEnglish))
            {
                payments = payments?.Where(x => x.PaymentCategoryEnglish == model.PaymentCategoryEnglish).ToList();
            }
            if (model.DateFrom.HasValue)
            {
                payments = payments?.Where(x => x.PaymentDate >= model.DateFrom).ToList();
            }
            if (model.DateTo.HasValue)
            {
                payments = payments?.Where(x => x.PaymentDate.Date < model.DateTo.Value.AddDays(1)).ToList();
            }
            var dataSize = payments.Count();
            var sortProperty = typeof(OfficesPaymentsReportViewModel).GetProperty(model?.Sort ?? "Id");
            if (model?.Order == "desc")
            {
                list = payments?.Select(o => new OfficesPaymentsReportViewModel(o)).OrderByDescending(x => sortProperty.GetValue(x)).ToList();
            }
            else
            {
                list = payments?.Select(o => new OfficesPaymentsReportViewModel(o)).OrderBy(x => sortProperty.GetValue(x)).ToList();
            }
            foreach (var l in list)
            {
                l.PaymentDate = TimeZoneInfo.ConvertTimeFromUtc(l.PaymentDate, timezone);
            }
            var result = list.Skip(model.PageSize * model.PageIndex).Take(model.PageSize).ToList();
            return new ResultWithMessage
            {
                Success = true,
                Message = "",
                Result = new ObservableData(result, dataSize)
            };
        }

        public List<OfficesPaymentsReportViewModel> ExportOfficesPaymentsReportForAdmin(FilterModel model)
        {
            model.PageSize = 999999;
            model.PageIndex = 0;
            var list = new List<OfficesPaymentsReportViewModel>();
            var payments = _db.OfficePayments.Include(x => x.Office).ToList();
            foreach (var pay in payments)
            {
                pay.PaymentDate = TimeZoneInfo.ConvertTimeFromUtc(pay.PaymentDate, timezone);
            }
            if (model.OfficeTypes?.Count() > 0)
            {
                payments = payments?.Where(x => model.OfficeTypes.Contains(x.Office.TypeId)).ToList();
            }
            if (model.Offices?.Count() > 0)
            {
                payments = payments?.Where(x => model.Offices.Contains(x.OfficeId)).ToList();
            }
            if (!string.IsNullOrEmpty(model.PaymentCategoryEnglish))
            {
                payments = payments?.Where(x => x.PaymentCategoryEnglish == model.PaymentCategoryEnglish).ToList();
            }
            if (model.DateFrom.HasValue)
            {
                payments = payments?.Where(x => x.PaymentDate >= model.DateFrom).ToList();
            }
            if (model.DateTo.HasValue)
            {
                payments = payments?.Where(x => x.PaymentDate <= model.DateTo).ToList();
            }
            var sortProperty = typeof(OfficesPaymentsReportViewModel).GetProperty(model?.Sort ?? "Id");
            if (model?.Order == "desc")
            {
                list = payments?.Select(o => new OfficesPaymentsReportViewModel(o)).OrderByDescending(x => sortProperty.GetValue(x)).ToList();
            }
            else
            {
                list = payments?.Select(o => new OfficesPaymentsReportViewModel(o)).OrderBy(x => sortProperty.GetValue(x)).ToList();
            }

            var result = list.Skip(model.PageSize * model.PageIndex).Take(model.PageSize).ToList();
            return result;
        }

        public List<OfficeAdminExportViewModel> ExportOfficesForAdmin(FilterModel model)
        {
            model.PageSize = 999999;
            model.PageIndex = 0;
            var list = new List<OfficeAdminExportViewModel>();
            var offices = _db.Offices?.Include(x => x.Type)
                                        .Include(x => x.Area)
                                        .Include(x => x.Governorate)
                                        .Include(x => x.Country)
                                        .Include(x => x.Entity)
                                        .Include(x => x.LegalEntity)
                                        .Include(x => x.OfficeSpecialities)
                                        .Include(x => x.OfficeActivities).ToList();
            if (model.Types?.Count() > 0)
            {
                offices = offices?.Where(x => model.Types.Contains(x.TypeId)).ToList();
            }

            if (model.Entities?.Count() > 0)
            {
                offices = offices?.Where(x => model.Entities.Contains(x.EntityId ?? 0)).ToList();
            }

            if (model.LegalEntities?.Count() > 0)
            {
                offices = offices?.Where(x => model.LegalEntities.Contains(x.LegalEntityId ?? 0)).ToList();
            }

            if (model.Specialities?.Count() > 0)
            {
                offices = offices?.Where(x => x.OfficeSpecialities.Select(x => x.SpecialityId.Value).Intersect(model.Specialities).Any()).ToList();
            }

            if (model.Activities?.Count() > 0)
            {
                offices = offices?.Where(x => x.OfficeActivities.Select(x => x.ActivityId.Value).Intersect(model.Activities).Any()).ToList();
            }
            if (model.IsActive != null)
            {
                offices = offices?.Where(x => x.IsActive == model.IsActive).ToList();
            }

            if (model.IsVerified != null)
            {
                offices = offices?.Where(x => x.IsVerified == model.IsVerified).ToList();
            }
            if (!string.IsNullOrEmpty(model.SearchQuery))
            {
                if (long.TryParse(model.SearchQuery, out long license))
                {
                    offices = offices?.Where(x => x.LicenseId == license).ToList();
                }
                else
                {
                    offices = offices?.Where(x => x.NameArabic.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                 x.NameEnglish.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                 (x.Area != null &&
                                                                    (x.Area.NameArabic.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                                     x.Area.NameEnglish.ToLower().Contains(model.SearchQuery.ToLower()))) ||
                                                 (x.Governorate != null &&
                                                                    (x.Governorate.NameArabic.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                                     x.Governorate.NameEnglish.ToLower().Contains(model.SearchQuery.ToLower()))) ||
                                                 (x.Country != null &&
                                                                    (x.Country.NameArabic.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                                     x.Country.NameEnglish.ToLower().Contains(model.SearchQuery.ToLower()))) ||
                                                 x.Email.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                 x.PhoneNumber.ToLower().Contains(model.SearchQuery.ToLower())).ToList();
                }
            }

            var sortProperty = typeof(OfficeAdminExportViewModel).GetProperty(model?.Sort ?? "Id");
            if (model?.Order == "desc")
            {
                list = offices?.Select(o => new OfficeAdminExportViewModel(o)).OrderByDescending(x => sortProperty.GetValue(x)).ToList();
            }
            else
            {
                list = offices?.Select(o => new OfficeAdminExportViewModel(o)).OrderBy(x => sortProperty.GetValue(x)).ToList();
            }
            foreach (var o in list)
            {
                if (o.RegistrationDate.HasValue)
                {
                    o.RegistrationDate = TimeZoneInfo.ConvertTimeFromUtc(o.RegistrationDate.Value, timezone);
                }

                if (o.EstablishmentDate.HasValue)
                {
                    o.EstablishmentDate = TimeZoneInfo.ConvertTimeFromUtc(o.EstablishmentDate.Value, timezone);
                }

                if (o.LicenseEndDate.HasValue)
                {
                    o.LicenseEndDate = TimeZoneInfo.ConvertTimeFromUtc(o.LicenseEndDate.Value, timezone);
                }

                if (o.MembershipEndDate.HasValue)
                {
                    o.MembershipEndDate = TimeZoneInfo.ConvertTimeFromUtc(o.MembershipEndDate.Value, timezone);
                }

            }
            return list;
        }
    }
}
