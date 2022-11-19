using KFEOCH.Contexts;
using KFEOCH.Models.Dictionaries;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KFEOCH.Services
{
    public class DictionaryService : IDictionaryService
    {
        private readonly ApplicationDbContext _db;
        public DictionaryService( ApplicationDbContext db)
        {
            _db = db;
        }
        //Area
        public List<Area> GetAllAreas()
        {
            var list = new List<Area>();
            list = _db.Areas?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<Area>();
        }
        public async Task<ResultWithMessage> PostAreaAsync(Area model)
        {
            var a = _db.Areas.Where(x => (x.NameArabic == model.NameArabic && x.GovernorateId == model.GovernorateId)
                                    || (x.NameEnglish == model.NameEnglish && x.GovernorateId == model.GovernorateId)
                                    ).FirstOrDefault();
            if (a != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Area {model.NameArabic} Already Exist !!!" };
            }
            await _db.Areas.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };

        }
        public async Task<ResultWithMessage> DeleteAreaAsync(int id)
        {
            var area = _db.Areas.Find(id);
            if(area == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Area Not Found !!!" };
            }
            area.IsDeleted = true;
            _db.Entry(area).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Area {area.NameArabic} Deleted !!!" };
        }
        public List<Area> GetAreasByGovernorateId(int id)
        {
            var list = new List<Area>();
            list = _db.Areas?.Where(a => a.GovernorateId == id).ToList();
            return list ?? new List<Area>();
        }

        //Governorate
        public List<Governorate> GetAllGovernorates()
        {
            var list = new List<Governorate>();
            list = _db.Governorates?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<Governorate>();
        }
        public async Task<ResultWithMessage> PostGovernorateAsync(Governorate model)
        {
            var a = _db.Governorates.Where(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish)
                                  ).FirstOrDefault();
            if (a != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Governorate {model.NameArabic} Already Exist !!!" };
            }
            await _db.Governorates.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeleteGovernorateAsync(int id)
        {
            var governorate = _db.Governorates.Find(id);
            if (governorate == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Governorate Not Found !!!" };
            }
            governorate.IsDeleted = true;
            _db.Entry(governorate).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Governorate {governorate.NameArabic} Deleted !!!" };
        }
        public List<Governorate> GetGovernoratesByCountryId(int id)
        {
            var list = new List<Governorate>(); 
            list = _db.Governorates?.Include(a => a.Areas)
                        .Where(a => a.CountryId == id && a.IsDeleted == false)
                        .ToList();
            return list ?? new List<Governorate>();
        }

        //Country
        public List<Country> GetAllCountries()
        {
            var list = new List<Country>();
            list = _db.Countries?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<Country>();
        }
        public async Task<ResultWithMessage> PostCountryAsync(Country model)
        {
            var a = _db.Countries.Where(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish)
                                   ).FirstOrDefault();
            if (a != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Country {model.NameArabic} Already Exist !!!" };
            }
            await _db.Countries.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };

        }
        public async Task<ResultWithMessage> DeleteCountryAsync(int id)
        {
            var country = _db.Countries.Find(id);
            if (country == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Country Not Found !!!" };
            }
            country.IsDeleted = true;
            _db.Entry(country).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Country {country.NameArabic} Deleted !!!" };
        }
        public List<Country> GetLocationsByCountryId(int id)
        {
            var list = new List<Country>();
            list = _db.Countries?.Include(g => g.Governorates).ThenInclude(a => a.Areas)
                        .Where(a => a.Id == id && a.IsDeleted == false)
                        .ToList();
            return list ?? new List<Country>();
        }

        //CertificateEntity
        public List<CertificateEntity> GetAllCertificateEntity()
        {
            var list = new List<CertificateEntity>();
            list = _db.CertificateEntities?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<CertificateEntity>();
        }
        public async Task<ResultWithMessage> PostCertificateEntityAsync(CertificateEntity model)
        {
            var cEntity = _db.CertificateEntities.Where(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish)
                                   ).FirstOrDefault();
            if (cEntity != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Certificate Entity {model.NameArabic} Already Exist !!!" };
            }
            await _db.CertificateEntities.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeleteCertificateEntityAsync(int id)
        {
            var cEntity = _db.CertificateEntities.Find(id);
            if (cEntity == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Certificate Entity  Not Found !!!" };
            }
            cEntity.IsDeleted = true;
            _db.Entry(cEntity).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Certificate Entity {cEntity.NameArabic} Deleted !!!" };
        }
       
        //CourseCategory
        public List<CourseCategory> GetAllCourseCategories()
        {
            var list = new List<CourseCategory>();
            list = _db.CourseCategories?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<CourseCategory>();
        }
        public async Task<ResultWithMessage> PostCourseCategoryAsync(CourseCategory model)
        {
            var coursecategory = _db.CourseCategories.Where(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish)
                                  ).FirstOrDefault();
            if (coursecategory != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Course Category {model.NameArabic} Already Exist !!!" };
            }
            await _db.CourseCategories.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeleteCourseCategoryAsync(int id)
        {
            var coursecategory = _db.CourseCategories.Find(id);
            if (coursecategory == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Course Category  Not Found !!!" };
            }
            coursecategory.IsDeleted = true;
            _db.Entry(coursecategory).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Course Category {coursecategory.NameArabic} Deleted !!!" };
        }

        //Genders
        public List<Gender> GetAllGenders()
        {
            var list = new List<Gender>();
            list = _db.Genders?.Where(x => x.IsDeleted == false).ToList();
            
            return list ?? new List<Gender>();
        }

        //OfficeActivity
        public List<OfficeActivity> GetAllOfficeActivities()
        {
            var list = new List<OfficeActivity>();
            list  = _db.OfficeActivities?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<OfficeActivity>();
        }
        public async Task<ResultWithMessage> PostOfficeActivityAsync(OfficeActivity model)
        {
            var officeactivity = _db.OfficeActivities.Where(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish)
                                   ).FirstOrDefault();
            if (officeactivity != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Activity {model.NameArabic} Already Exist !!!" };
            }
            await _db.OfficeActivities.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeleteOfficeActivityAsync(int id)
        {
            var officeactivity = _db.OfficeActivities.Find(id);
            if (officeactivity == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Activity Not Found !!!" };
            }
            officeactivity.IsDeleted = true;
            _db.Entry(officeactivity).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Office Activity {officeactivity.NameArabic} Deleted !!!" };
        }
        public List<OfficeActivity> GetAllOfficeActivitiesByOfficeTypeId(int id)
        {
            var list = new List<OfficeActivity>();
            list = _db.OfficeActivities?.Where(x => (x.IsDeleted == false)&& (x.OfficeTypeId == id)).ToList();
            return list ?? new List<OfficeActivity>();
        }

        //OfficeEntity
        public List<OfficeEntity> GetAllOfficeEntities()
        {
            var list = new List<OfficeEntity>();
            var officeentities = _db.OfficeEntities?.Where(x => x.IsDeleted == false).ToList();
            if (officeentities == null)
            {
                return null;
            }
            return officeentities;
        }
        public async Task<ResultWithMessage> PostOfficeEntityAsync(OfficeEntity model)
        {
            var officeentity = _db.OfficeEntities.Where(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish)
                                  ).FirstOrDefault();
            if (officeentity != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Entity {model.NameArabic} Already Exist !!!" };
            }
            await _db.OfficeEntities.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeleteOfficeEntityAsync(int id)
        {
            var officeentity = _db.OfficeEntities.Find(id);
            if (officeentity == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Entity Not Found !!!" };
            }
            officeentity.IsDeleted = true;
            _db.Entry(officeentity).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Office Entity {officeentity.NameArabic} Deleted !!!" };
        }

        //OfficeLegalEntity
        public List<OfficeLegalEntity> GetAllOfficeLegalEntities()
        {
            var list = new List<OfficeLegalEntity>();
            list = _db.OfficeLegalEntities?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<OfficeLegalEntity>();
        }
        public async Task<ResultWithMessage> PostOfficeLegalEntityAsync(OfficeLegalEntity model)
        {
            var officelegalentity = _db.OfficeLegalEntities.Where(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish)
                                  ).FirstOrDefault();
            if (officelegalentity != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Legal Entity {model.NameArabic} Already Exist !!!" };
            }
            await _db.OfficeLegalEntities.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeleteOfficeLegalEntityAsync(int id)
        {
            var officelegalentity = _db.OfficeLegalEntities.Find(id);
            if (officelegalentity == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Legal Entity Not Found !!!" };
            }
            officelegalentity.IsDeleted = true;
            _db.Entry(officelegalentity).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Office Legal Entity {officelegalentity.NameArabic} Deleted !!!" };
        }

        //OfficeSpeciality
        public List<OfficeSpeciality> GetAllOfficeSpecialities()
        {
            var list = new List<OfficeSpeciality>();
            list = _db.OfficeSpecialities?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<OfficeSpeciality>();
        }
        public async Task<ResultWithMessage> PostOfficeSpecialityAsync(OfficeSpeciality model)
        {
            var OfficeSpeciality = _db.OfficeSpecialities.Where(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish)
                                   ).FirstOrDefault();
            if (OfficeSpeciality != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Speciality {model.NameArabic} Already Exist !!!" };
            }
            await _db.OfficeSpecialities.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeleteOfficeSpecialityAsync(int id)
        {
            var OfficeSpeciality = _db.OfficeSpecialities.Find(id);
            if (OfficeSpeciality == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Speciality Not Found !!!" };
            }
            OfficeSpeciality.IsDeleted = true;
            _db.Entry(OfficeSpeciality).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Office Speciality {OfficeSpeciality.NameArabic} Deleted !!!" };
        }
        public List<OfficeSpeciality> GetAllOfficeSpecialitiesByOfficeTypeId(int id)
        {
            var list = new List<OfficeSpeciality>();
            list = _db.OfficeSpecialities?.Where(x => (x.IsDeleted == false) && (x.OfficeTypeId == id)).ToList();
            return list ?? new List<OfficeSpeciality>();
        }

        //OfficeOwnerSpeciality
        public List<OfficeOwnerSpeciality> GetAllOfficeOwnerSpecialities()
        {
            var list = new List<OfficeOwnerSpeciality>();
            list = _db.OfficeOwnerSpecialities?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<OfficeOwnerSpeciality>();
        }
        public async Task<ResultWithMessage> PostOfficeOwnerSpecialityAsync(OfficeOwnerSpeciality model)
        {
            var OfficeSpeciality = _db.OfficeOwnerSpecialities.Where(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish)
                                   ).FirstOrDefault();
            if (OfficeSpeciality != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Owner Speciality {model.NameArabic} Already Exist !!!" };
            }
            await _db.OfficeOwnerSpecialities.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeleteOfficeOwnerSpecialityAsync(int id)
        {
            var OfficeSpeciality = _db.OfficeOwnerSpecialities.Find(id);
            if (OfficeSpeciality == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Speciality Not Found !!!" };
            }
            OfficeSpeciality.IsDeleted = true;
            _db.Entry(OfficeSpeciality).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Office Owner Speciality {OfficeSpeciality.NameArabic} Deleted !!!" };
        }

        //OfficeStatus
        public List<OfficeStatus> GetAllOfficeStatuses()
        {
            var list = new List<OfficeStatus>();
            list = _db.officeStatuses?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<OfficeStatus>();
        }
        public async Task<ResultWithMessage> PostOfficeStatusAsync(OfficeStatus model)
        {
            var officeStatus = _db.officeStatuses.Where(x => (x.NameArabic == model.NameArabic)
                             || (x.NameEnglish == model.NameEnglish)
                             ).FirstOrDefault();
            if (officeStatus != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Status {model.NameArabic} Already Exist !!!" };
            }
            await _db.officeStatuses.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeleteOfficeStatusAsync(int id)
        {
            var officeStatus = _db.officeStatuses.Find(id);
            if (officeStatus == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Status Not Found !!!" };
            }
            officeStatus.IsDeleted = true;
            _db.Entry(officeStatus).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Office Status {officeStatus.NameArabic} Deleted !!!" };
        }

        //OfficeType
        public List<OfficeType> GetAllOfficeTypes()
        {
            var list = new List<OfficeType>();
            list = _db.OfficeTypes?.Where(x => (x.IsDeleted == false) && (x.IsAdmin == false) ).ToList();
            return list ?? new List<OfficeType>();
        }
        public async Task<ResultWithMessage> PostOfficeTypeAsync(OfficeType model)
        {
            var officeType = _db.OfficeTypes.Where(x => (x.NameArabic == model.NameArabic)
                            || (x.NameEnglish == model.NameEnglish)
                            ).FirstOrDefault();
            if (officeType != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Type {model.NameArabic} Already Exist !!!" };
            }
            await _db.OfficeTypes.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeleteOfficeTypeAsync(int id)
        {
            var officeType = _db.OfficeTypes.Find(id);
            if (officeType == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Type Not Found !!!" };
            }
            officeType.IsDeleted = true;
            _db.Entry(officeType).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Office Type {officeType.NameArabic} Deleted !!!" };
        }
        public List<OfficeType> GetAllOfficeTypesWithDetials(int id)
        {
            var list = new List<OfficeType>();
            list = _db.OfficeTypes.Where(x => (x.IsDeleted == false) && (x.Id == id) && (x.IsAdmin == false))
                              .Include(s => s.OfficeSpecialities)
                              .Include(a => a.OfficeActivities).ToList();
            return list ?? new List<OfficeType>();
        }
    }
}
