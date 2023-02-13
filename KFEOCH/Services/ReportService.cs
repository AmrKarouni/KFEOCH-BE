using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KFEOCH.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _db;
        private readonly TimeZoneInfo timezone;
        public ReportService(ApplicationDbContext db)
        {
            _db = db;
            timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
        }
        public List<OfficeAdminExportViewModel> ExportOfficesForAdmin()
        {
            var list = new List<OfficeAdminExportViewModel>();
            var offices = _db.Offices?.Include(x => x.Type)
                                        .Include(x => x.Area)
                                        .Include(x => x.Governorate)
                                        .Include(x => x.Country)
                                        .Include(x => x.Entity)
                                        .Include(x => x.LegalEntity)
                                        .Include(x => x.OfficeSpecialities)
                                        .Include(x => x.OfficeActivities).ToList();


            list = offices?.Select(o => new OfficeAdminExportViewModel(o)).OrderBy(x => x.Id).ToList();
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

        public List<OfficeOwnerReportViewModel> ExportOfficeOwnersForAdmin()
        {
            var list = new List<OfficeOwnerReportViewModel>();
            var owners = _db.OfficeOwners.Include(x => x.Office)
                                          .Include(x => x.Gender)
                                          .Include(x => x.Speciality)
                                          .Include(x => x.Nationality).ToList();
            list = owners.Select(v => new OfficeOwnerReportViewModel(v)).OrderBy(x => x.OfficeId).ToList();
            return list;
        }

        public List<OfficeSpecialityReportViewModel> ExportOfficeSpecialitiesForAdmin()
        {
            var list = new List<OfficeSpecialityReportViewModel>();
            var specialities = _db.OfficeSpecialities.Include(x => x.Office)
                                          .Include(x => x.Speciality).ToList();
            foreach (var p in specialities)
            {
                p.AddedDate = TimeZoneInfo.ConvertTimeFromUtc(p.AddedDate.Value, timezone);
            }
            list = specialities.Select(v => new OfficeSpecialityReportViewModel(v)).OrderBy(x => x.OfficeId).ToList();
            return list;
        }

        public List<OfficeContactReportViewModel> ExportOfficeContactsForAdmin()
        {
            var list = new List<OfficeContactReportViewModel>();
            var contacts = _db.OfficeContacts.Include(x => x.Office)
                                          .Include(x => x.Contact).ToList();
            foreach (var c in contacts)
            {
                c.AddedDate = TimeZoneInfo.ConvertTimeFromUtc(c.AddedDate.Value, timezone);
            }
            list = contacts.Select(v => new OfficeContactReportViewModel(v)).OrderBy(x => x.OfficeId).ToList();
            return list;
        }

        public List<OfficeLicenseReportViewModel> ExportOfficeLicensesForAdmin()
        {
            var list = new List<OfficeLicenseReportViewModel>();
            var licenses = _db.Licenses.Include(x => x.Office)
                                        .Include(x => x.OfficeEntity).ToList();
            foreach(var l in licenses)
            {
                l.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(l.CreatedDate, timezone);
                l.StartDate = TimeZoneInfo.ConvertTimeFromUtc(l.StartDate, timezone);
                l.EndDate = TimeZoneInfo.ConvertTimeFromUtc(l.EndDate, timezone);
            }
            list = licenses.Select(v => new OfficeLicenseReportViewModel(v))
                           .OrderBy(x => x.OfficeId)
                           .ThenBy(x => x.CreatedDate)
                           .ThenBy(x => x.StartDate).ToList();
            return list;
        }
    }
}
