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
        public DictionaryService(ApplicationDbContext db)
        {
            _db = db;
        }
        //Area
        public List<AreaViewModel> GetAllAreas()
        {
            var list = new List<AreaViewModel>();
            list = _db.Areas?.Include(x => x.Parent).Where(x => x.IsDeleted == false)
                .Select(x => new AreaViewModel(x)).ToList();
            return list ?? new List<AreaViewModel>();
        }
        public async Task<ResultWithMessage> PostAreaAsync(Area model)
        {
            var a = _db.Areas.FirstOrDefault(x => (x.NameArabic == model.NameArabic && x.ParentId == model.ParentId)
                                    || (x.NameEnglish == model.NameEnglish && x.ParentId == model.ParentId));
            if (a != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Area {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Area {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"المنطقة {model.NameArabic} موجودة مسبقاً !!!"
                };
            }
            await _db.Areas.AddAsync(model);
            _db.SaveChanges();
            var result = new AreaViewModel(model);
            return new ResultWithMessage { Success = true, Result = result };

        }
        //public async Task<ResultWithMessage> DeleteAreaAsync(int id)
        //{
        //    var area = _db.Areas.Find(id);
        //    if (area == null)
        //    {
        //        return new ResultWithMessage
        //        {
        //            Success = false,
        //            Message = $@"Area Not Found !!!",
        //            MessageEnglish = $@"Area Not Found !!!",
        //            MessageArabic = "المنطقة غير موجودة !!!"
        //        };
        //    }
        //    _db.Areas.Remove(area);
        //    _db.SaveChanges();
        //    return new ResultWithMessage
        //    {
        //        Success = true,
        //        Message = $@"Area {area.NameEnglish} Deleted !!!",
        //        MessageEnglish = $@"Area {area.NameEnglish} Deleted !!!",
        //        MessageArabic = $@"تم حذف المنطقة {area.NameArabic} !!!"
        //    };
        //}
        public ResultWithMessage PutArea(int id,Area model)
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
            var area = _db.Areas.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(area).State = EntityState.Detached;
            if (area == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Area Not Found !!!",
                    MessageEnglish = $@"Area Not Found !!!",
                    MessageArabic = "المنطقة غير موجودة !!!"
                };
            }
            area = model;
            _db.Entry(area).State = EntityState.Modified;
            _db.SaveChanges();
            var result = new AreaViewModel(model);
            return new ResultWithMessage { Success = true, Result = result };

        }
        public List<Area> GetAreasByGovernorateId(int id)
        {
            var list = new List<Area>();
            list = _db.Areas?.Where(a => a.ParentId == id).ToList();
            return list ?? new List<Area>();
        }


        //Governorate
        public List<GovernorateViewModel> GetAllGovernorates()
        {
            var list = new List<GovernorateViewModel>();
            list = _db.Governorates?.Include(x => x.Parent).Where(x => x.IsDeleted == false)
                .Select(x => new GovernorateViewModel(x)).ToList();
            return list ?? new List<GovernorateViewModel>();
        }
        public async Task<ResultWithMessage> PostGovernorateAsync(Governorate model)
        {
            var a = _db.Governorates.Where(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish)
                                  ).FirstOrDefault();
            if (a != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Governorate {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Governorate {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"المحافظة {model.NameArabic} موجودة بالفعل !!!"
                };
        }
            await _db.Governorates.AddAsync(model);
            _db.SaveChanges();
            var result = new GovernorateViewModel(model);
            return new ResultWithMessage { Success = true, Result = result };
        }
        //public async Task<ResultWithMessage> DeleteGovernorateAsync(int id)
        //{
        //    var governorate = _db.Governorates.Find(id);
        //    if (governorate == null)
        //    {
        //        return new ResultWithMessage
        //        {
        //            Success = false,
        //            Message = $@"Governorate Not Found !!!",
        //            MessageEnglish = $@"Governorate Not Found !!!",
        //        MessageArabic = $@"المحافظة غير موجودة !!!"
        //        };
        //}
        //    _db.Governorates.Remove(governorate);
        //    _db.SaveChanges();

        //     return new ResultWithMessage
        //        {
        //            Success = true,
        //            Message = $@"Governorate {governorate.NameEnglish} Deleted !!!",
        //            MessageEnglish = $@"Governorate {governorate.NameEnglish} Deleted !!!",
        //            MessageArabic = $@"تم حذف المحافظة {governorate.NameArabic} !!!"
        //     };
        //}
        public List<Governorate> GetGovernoratesByCountryId(int id)
        {
            var list = new List<Governorate>();
            list = _db.Governorates?.Where(a => a.ParentId == id).ToList();
            return list ?? new List<Governorate>();
        }
        public ResultWithMessage PutGovernorate(int id, Governorate model)
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
            var governorate = _db.Governorates.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(governorate).State = EntityState.Detached;
            if (governorate == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Governorate Not Found !!!",
                    MessageEnglish = $@"Governorate Not Found !!!",
                    MessageArabic = "المحافظة غير موجودة !!!"
                };
            }
            governorate = model;
            _db.Entry(governorate).State = EntityState.Modified;
            _db.SaveChanges();
            var result = new GovernorateViewModel(governorate);
            return new ResultWithMessage { Success = true, Result = result };

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
            var a = _db.Countries.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish));
            if (a != null)
            {
               
                 return new ResultWithMessage
                 {
                     Success = false,
                     Message = $@"Country {model.NameEnglish} Already Exist !!!",
                     MessageEnglish = $@"Country {model.NameEnglish} Already Exist !!!",
                     MessageArabic = $@"البلد {model.NameArabic} موجود مسبقاً !!!"
                 };
            }
            await _db.Countries.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };

        }
        //public async Task<ResultWithMessage> DeleteCountryAsync(int id)
        //{
        //    var country = _db.Countries.Find(id);
        //    if (country == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Country Not Found !!!" };
        //    }
        //    _db.Countries.Remove(country);
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Country {country.NameArabic} Deleted !!!" };
        //}
        public List<Country> GetLocationsByCountryId(int id)
        {
            var list = new List<Country>();
            list = _db.Countries?.Include(g => g.Governorates).ThenInclude(a => a.Areas)
                        .Where(a => a.Id == id && a.IsDeleted == false)
                        .ToList();
            return list ?? new List<Country>();
        }
        public ResultWithMessage PutCountry(int id, Country model)
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
            var country = _db.Countries.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(country).State = EntityState.Detached;
            if (country == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Country Not Found !!!",
                    MessageEnglish = $@"Country Not Found !!!",
                    MessageArabic = "البلد غير موجودة !!!"
                };
            }
            country = model;
            _db.Entry(country).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = country };

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
            var cEntity = _db.CertificateEntities.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish));
            if (cEntity != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Certificate Entity {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Certificate Entity {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@" الكيان {model.NameEnglish} موجود سبقاً !!!"
                };
            }
            await _db.CertificateEntities.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        //public async Task<ResultWithMessage> DeleteCertificateEntityAsync(int id)
        //{
        //    var cEntity = _db.CertificateEntities.Find(id);
        //    if (cEntity == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Certificate Entity  Not Found !!!" };
        //    }
        //    cEntity.IsDeleted = true;
        //    _db.Entry(cEntity).State = EntityState.Modified;
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Certificate Entity {cEntity.NameArabic} Deleted !!!" };
        //}

        //CourseCategory
        public ResultWithMessage PutCertificateEntity(int id, CertificateEntity model)
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
            var cEntity = _db.CertificateEntities.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(cEntity).State = EntityState.Detached;
            if (cEntity == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Certificate Entity  Not Found !!!",
                    MessageEnglish = $@"Certificate Entity Not Found !!!",
                    MessageArabic = "الكيان غير موجود !!!"
                };
            }
            cEntity = model;
            _db.Entry(cEntity).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = cEntity };

        }

        public List<CourseCategory> GetAllCourseCategories()
        {
            var list = new List<CourseCategory>();
            list = _db.CourseCategories?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<CourseCategory>();
        }
        public async Task<ResultWithMessage> PostCourseCategoryAsync(CourseCategory model)
        {
            var coursecategory = _db.CourseCategories.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish));
            if (coursecategory != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Course Category {model.NameArabic} Already Exist !!!" };

                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Course Category {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Course Category {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"التصنيف {model.NameEnglish} موجود مسبقاً !!!"
                };


            }
            await _db.CourseCategories.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public ResultWithMessage PutCourseCategory(int id, CourseCategory model)
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
            var coursecategory = _db.CourseCategories.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(coursecategory).State = EntityState.Detached;
            if (coursecategory == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Course Category Not Found !!!",
                    MessageEnglish = $@"Course Category Not Found !!!",
                    MessageArabic = "التصنيف غير موجود !!!"
                };
            }
            coursecategory = model;
            _db.Entry(coursecategory).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = coursecategory };

        }
        //public async Task<ResultWithMessage> DeleteCourseCategoryAsync(int id)
        //{
        //    var coursecategory = _db.CourseCategories.Find(id);
        //    if (coursecategory == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Course Category  Not Found !!!" };
        //    }
        //    coursecategory.IsDeleted = true;
        //    _db.Entry(coursecategory).State = EntityState.Modified;
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Course Category {coursecategory.NameArabic} Deleted !!!" };
        //}

        
        
        
        //Genders
        public List<Gender> GetAllGenders()
        {
            var list = new List<Gender>();
            list = _db.Genders?.Where(x => x.IsDeleted == false).ToList();

            return list ?? new List<Gender>();
        }

        //OfficeActivity
        public List<ActivityViewModel> GetAllOfficeActivities()
        {
            var list = new List<ActivityViewModel>();
            list = _db.Activities?.Include(x => x.Parent).Where(x => x.IsDeleted == false).Select(x => new ActivityViewModel(x)).ToList();
            return list ?? new List<ActivityViewModel>();

        }
        public async Task<ResultWithMessage> PostOfficeActivityAsync(Activity model)
        {
            var officeactivity = _db.Activities?.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish)
                                   );
            if (officeactivity != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Activity {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Office Activity {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"النشاط {model.NameArabic} موجود مسبقاً !!!"
                };
            }
            await _db.Activities.AddAsync(model);
            _db.SaveChanges();
            var result = new ActivityViewModel(model);
            return new ResultWithMessage { Success = true, Result = result };
        }
        public ResultWithMessage PutOfficeActivity(int id, Activity model)
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
            var officeactivity = _db.Activities?.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(officeactivity).State = EntityState.Detached;
            if (officeactivity == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Activity Not Found !!!",
                    MessageEnglish = $@"Office Activity Not Found !!!",
                    MessageArabic = "النشاط غير موجود !!!"
                };
            }
            officeactivity = model;
            _db.Entry(officeactivity).State = EntityState.Modified;
            _db.SaveChanges();
            var result = new ActivityViewModel(model);
            return new ResultWithMessage { Success = true, Result = result };

        }
        //public async Task<ResultWithMessage> DeleteOfficeActivityAsync(int id)
        //{
        //    var officeactivity = _db.Activities?.Find(id);
        //    if (officeactivity == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Office Activity Not Found !!!" };
        //    }
        //    officeactivity.IsDeleted = true;
        //    _db.Entry(officeactivity).State = EntityState.Modified;
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Office Activity {officeactivity.NameArabic} Deleted !!!" };
        //}
        public List<ActivityViewModel> GetAllOfficeActivitiesByOfficeTypeId(int id)
        {
            var office = _db.Offices?.Find(id);
            if (office == null)
            {
                return new List<ActivityViewModel>();
            }
            var list = new List<ActivityViewModel>();
            list = _db.Activities?.Include(x => x.Parent)
                                  .Where(x => x.IsDeleted == false && (x.ParentId == office.TypeId))
                                  .Select(x => new ActivityViewModel(x)).ToList();
            return list ?? new List<ActivityViewModel>();
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
            var officeentity = _db.OfficeEntities.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish));
            if (officeentity != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Entity {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Office Entity {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"الكيان {model.NameArabic} موجود مسبقاً !!!"
                };
            }
            await _db.OfficeEntities.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public ResultWithMessage PutOfficeEntity(int id, OfficeEntity model)
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
            var officeentity = _db.OfficeEntities?.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(officeentity).State = EntityState.Detached;
            if (officeentity == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Entity Not Found !!!",
                    MessageEnglish = $@"Office Entity Not Found !!!",
                    MessageArabic = "الكيان غير موجود !!!"
                };
            }
            officeentity = model;
            _db.Entry(officeentity).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = officeentity };

        }
        //public async Task<ResultWithMessage> DeleteOfficeEntityAsync(int id)
        //{
        //    var officeentity = _db.OfficeEntities.Find(id);
        //    if (officeentity == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Office Entity Not Found !!!" };
        //    }
        //    officeentity.IsDeleted = true;
        //    _db.Entry(officeentity).State = EntityState.Modified;
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Office Entity {officeentity.NameArabic} Deleted !!!" };
        //}




        //OfficeLegalEntity
        public List<OfficeLegalEntity> GetAllOfficeLegalEntities()
        {
            var list = new List<OfficeLegalEntity>();
            list = _db.OfficeLegalEntities?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<OfficeLegalEntity>();
        }
        public async Task<ResultWithMessage> PostOfficeLegalEntityAsync(OfficeLegalEntity model)
        {
            var officelegalentity = _db.OfficeLegalEntities.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish) );
            if (officelegalentity != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Legal Entity {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Office Legal Entity {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"الكيان القانوني {model.NameArabic} موجود مسبقاً !!!",
                };
            }
            await _db.OfficeLegalEntities.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }

        public ResultWithMessage PutOfficeLegalEntity(int id, OfficeLegalEntity model)
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
            var officelegalentity = _db.OfficeLegalEntities?.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(officelegalentity).State = EntityState.Detached;
            if (officelegalentity == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Legal Entity Not Found !!!",
                    MessageEnglish = $@"Office Legal Entity Not Found !!!",
                    MessageArabic = "الكيان القانوني غير موجود !!!"
                };
            }
            officelegalentity = model;
            _db.Entry(officelegalentity).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = officelegalentity };

        }
        //public async Task<ResultWithMessage> DeleteOfficeLegalEntityAsync(int id)
        //{
        //    var officelegalentity = _db.OfficeLegalEntities.Find(id);
        //    if (officelegalentity == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Office Legal Entity Not Found !!!" };
        //    }
        //    officelegalentity.IsDeleted = true;
        //    _db.Entry(officelegalentity).State = EntityState.Modified;
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Office Legal Entity {officelegalentity.NameArabic} Deleted !!!" };
        //}

        //OfficeSpeciality
        public List<SpecialityViewModel> GetAllOfficeSpecialities()
        {
            var list = new List<SpecialityViewModel>();
            list = _db.Specialities?.Include(x => x.Parent).Where(x => x.IsDeleted == false).Select(x => new SpecialityViewModel(x)).ToList();
            return list ?? new List<SpecialityViewModel>();
        }
        public async Task<ResultWithMessage> PostOfficeSpecialityAsync(Speciality model)
        {
            var OfficeSpeciality = _db.Specialities?.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish));
            if (OfficeSpeciality != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Speciality {model.NameArabic} Already Exist !!!",
                    MessageEnglish = $@"Office Speciality {model.NameArabic} Already Exist !!!",
                    MessageArabic = $@"التخصص{model.NameArabic} موجود مسقباً !!!"
                };
            }
            await _db.Specialities.AddAsync(model);
            _db.SaveChanges();
            var result = new SpecialityViewModel(model);
            return new ResultWithMessage { Success = true, Result = result };
        }
        public ResultWithMessage PutOfficeSpeciality(int id, Speciality model)
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
            var OfficeSpeciality = _db.Specialities?.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(OfficeSpeciality).State = EntityState.Detached;
            if (OfficeSpeciality == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Speciality Not Found !!!",
                    MessageEnglish = $@"Office Speciality Not Found !!!",
                    MessageArabic = "التخصص غير موجود !!!"
                };
            }
            OfficeSpeciality = model;
            _db.Entry(OfficeSpeciality).State = EntityState.Modified;
            _db.SaveChanges();
            var result = new SpecialityViewModel(model);
            return new ResultWithMessage { Success = true, Result = result };

        }

        //public async Task<ResultWithMessage> DeleteOfficeSpecialityAsync(int id)
        //{
        //    var OfficeSpeciality = _db.Specialities?.Find(id);
        //    if (OfficeSpeciality == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Office Speciality Not Found !!!" };
        //    }
        //    OfficeSpeciality.IsDeleted = true;
        //    _db.Entry(OfficeSpeciality).State = EntityState.Modified;
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Office Speciality {OfficeSpeciality.NameArabic} Deleted !!!" };
        //}
        public List<SpecialityViewModel> GetAllOfficeSpecialitiesByOfficeTypeId(int id)
        {
            var office = _db.Offices?.Find(id);
            if (office == null)
            {
                return new List<SpecialityViewModel>();
            }
            var list = new List<SpecialityViewModel>();
            list = _db.Specialities?.Include(x => x.Parent)
                                    .Where(x => x.IsDeleted == false && (x.ParentId == office.TypeId))
                                    .Select(x => new SpecialityViewModel(x)).ToList();
            return list ?? new List<SpecialityViewModel>();
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
            var ownerSpeciality = _db.OfficeOwnerSpecialities.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish));
            if (ownerSpeciality != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Owner Speciality {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Office Owner Speciality {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"التخصص {model.NameArabic} موجود بالفعل !!!",
                };
            }
            await _db.OfficeOwnerSpecialities.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }

        public ResultWithMessage PutOfficeOwnerSpeciality(int id, OfficeOwnerSpeciality model)
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
            var ownerSpeciality = _db.OfficeOwnerSpecialities?.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(ownerSpeciality).State = EntityState.Detached;
            if (ownerSpeciality == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Owner Speciality Not Found !!!",
                    MessageEnglish = $@"Office Owner Speciality Not Found !!!",
                    MessageArabic = "التخصص غير موجود !!!"
                };
            }
            ownerSpeciality = model;
            _db.Entry(ownerSpeciality).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = ownerSpeciality };

        }
        //public async Task<ResultWithMessage> DeleteOfficeOwnerSpecialityAsync(int id)
        //{
        //    var OfficeSpeciality = _db.OfficeOwnerSpecialities.Find(id);
        //    if (OfficeSpeciality == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Office Speciality Not Found !!!" };
        //    }
        //    OfficeSpeciality.IsDeleted = true;
        //    _db.Entry(OfficeSpeciality).State = EntityState.Modified;
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Office Owner Speciality {OfficeSpeciality.NameArabic} Deleted !!!" };
        //}

        //OfficeStatus
        public List<OfficeStatus> GetAllOfficeStatuses()
        {
            var list = new List<OfficeStatus>();
            list = _db.Statuses?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<OfficeStatus>();
        }
        public async Task<ResultWithMessage> PostOfficeStatusAsync(OfficeStatus model)
        {
            var officeStatus = _db.Statuses?.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                             || (x.NameEnglish == model.NameEnglish));
            if (officeStatus != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Status {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Office Status {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"حالة المكتب {model.NameEnglish} موجودة مسبقاً !!!"
                };

            }
            await _db.Statuses.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }

        public ResultWithMessage PutOfficeStatus(int id, OfficeStatus model)
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
            var officeStatus = _db.Statuses?.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(officeStatus).State = EntityState.Detached;
            if (officeStatus == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Status Not Found !!!",
                    MessageEnglish = $@"Office Status Not Found !!!",
                    MessageArabic = "حالة المكتب غير موجودة !!!"
                };
            }
            officeStatus = model;
            _db.Entry(officeStatus).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = officeStatus };

        }
        //public async Task<ResultWithMessage> DeleteOfficeStatusAsync(int id)
        //{
        //    var officeStatus = _db.Statuses?.Find(id);
        //    if (officeStatus == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Office Status Not Found !!!" };
        //    }
        //    officeStatus.IsDeleted = true;
        //    _db.Entry(officeStatus).State = EntityState.Modified;
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Office Status {officeStatus.NameArabic} Deleted !!!" };
        //}

        //OfficeType
        public List<OfficeType> GetAllOfficeTypes()
        {
            var list = new List<OfficeType>();
            list = _db.OfficeTypes?.Where(x => (x.IsDeleted == false) && (x.IsAdmin == false)).ToList();
            return list ?? new List<OfficeType>();
        }
        public async Task<ResultWithMessage> PostOfficeTypeAsync(OfficeType model)
        {
            var officeType = _db.OfficeTypes.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                            || (x.NameEnglish == model.NameEnglish));
            if (officeType != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Type {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Office Type {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"نوع المكتب {model.NameArabic} موجود بالفعل !!!",
                };
            }
            await _db.OfficeTypes.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public ResultWithMessage PutOfficeType(int id, OfficeType model)
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
            var officeType = _db.OfficeTypes?.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(officeType).State = EntityState.Detached;
            if (officeType == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Type Not Found !!!",
                    MessageEnglish = $@"Office Type Not Found !!!",
                    MessageArabic = "نوع المكتب غير موجود !!!"
                };
            }
            officeType.NameArabic = model.NameArabic;
            officeType.NameEnglish = model.NameEnglish;
            officeType.DescriptionArabic = model.DescriptionArabic;
            officeType.DescriptionEnglish = model.DescriptionEnglish;
            _db.Entry(officeType).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = officeType };

        }
        //public async Task<ResultWithMessage> DeleteOfficeTypeAsync(int id)
        //{
        //    var officeType = _db.OfficeTypes.Find(id);
        //    if (officeType == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Office Type Not Found !!!" };
        //    }
        //    officeType.IsDeleted = true;
        //    _db.Entry(officeType).State = EntityState.Modified;
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Office Type {officeType.NameArabic} Deleted !!!" };
        //}
        public List<OfficeType> GetAllOfficeTypesWithDetials(int id)
        {
            var list = new List<OfficeType>();
            list = _db.OfficeTypes.Where(x => (x.IsDeleted == false) && (x.Id == id) && (x.IsAdmin == false))
                              .Include(s => s.OfficeSpecialities)
                              .Include(a => a.OfficeActivities).ToList();
            return list ?? new List<OfficeType>();
        }


        //OfficeDocumentType
        public List<OfficeDocumentType> GetAllOfficeDocumentTypes()
        {
            var list = new List<OfficeDocumentType>();
            var officeDocumentTypes = _db.OfficeDocumentTypes?.Where(x => x.IsDeleted == false).ToList();
            if (officeDocumentTypes == null)
            {
                return list;
            }
            return officeDocumentTypes;
        }
        public async Task<ResultWithMessage> PostOfficeDocumentTypeAsync(OfficeDocumentType model)
        {
            var officeDocumentType = _db.OfficeDocumentTypes.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish));
            if (officeDocumentType != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Document Type {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Office Document Type {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"نوع المستند {model.NameArabic} موجود مسبقاً !!!"
                };

            }
            await _db.OfficeDocumentTypes.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public ResultWithMessage PutOfficeDocumentType(int id, OfficeDocumentType model)
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
            var officeDocumentType = _db.OfficeDocumentTypes?.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(officeDocumentType).State = EntityState.Detached;
            if (officeDocumentType == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Document Type Not Found !!!",
                    MessageEnglish = $@"Office Document Type Not Found !!!",
                    MessageArabic = "نوع المستند غير موجود !!!"
                };
            }
            officeDocumentType = model;
            _db.Entry(officeDocumentType).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = officeDocumentType };

        }
        //public async Task<ResultWithMessage> DeleteOfficeDocumentTypeAsync(int id)
        //{
        //    var officeentity = _db.OfficeDocumentTypes?.Find(id);
        //    if (officeentity == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Office Document Type Not Found !!!" };
        //    }
        //    _db.OfficeDocumentTypes.Remove(officeentity);
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Office Document Type {officeentity.NameArabic} Deleted !!!" };
        //}

        //OwnerDocumentType


        public List<OwnerDocumentType> GetAllOwnerDocumentTypes()
        {
            var list = new List<OwnerDocumentType>();
            var ownerDocumentTypes = _db.OwnerDocumentTypes?.Where(x => x.IsDeleted == false).ToList();
            if (ownerDocumentTypes == null)
            {
                return list;
            }
            return ownerDocumentTypes;
        }
        public async Task<ResultWithMessage> PostOwnerDocumentTypeAsync(OwnerDocumentType model)
        {
            var ownerDocumentType = _db.OwnerDocumentTypes?.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish));
            if (ownerDocumentType != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Owner Document Type {model.NameEnglish} Already Exist !!!" };

                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Owner Document Type {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Owner Document Type {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"نوع المستند {model.NameArabic} موجود مسبقاً !!!"
                };
            }
            await _db.OwnerDocumentTypes.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public ResultWithMessage PutOwnerDocumentType(int id, OwnerDocumentType model)
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
            var ownerDocumentType = _db.OwnerDocumentTypes?.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(ownerDocumentType).State = EntityState.Detached;
            if (ownerDocumentType == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Owner Document Type Not Found !!!",
                    MessageEnglish = $@"Owner Document Type Not Found !!!",
                    MessageArabic = "نوع المستند غير موجود !!!"
                };
            }
            ownerDocumentType = model;
            _db.Entry(ownerDocumentType).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = ownerDocumentType };

        }
        //public async Task<ResultWithMessage> DeleteOwnerDocumentTypeAsync(int id)
        //{
        //    var ownerDocumentType = _db.OwnerDocumentTypes?.Find(id);
        //    if (ownerDocumentType == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Office Document Type Not Found !!!" };
        //    }
        //    _db.OwnerDocumentTypes.Remove(ownerDocumentType);
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Owner Document Type {ownerDocumentType.NameEnglish} | {ownerDocumentType.NameArabic} Deleted !!!" };
        //}

        //Contact Type



        public List<ContactType> GetAllContactTypes()
        {
            var list = new List<ContactType>();
            var contacttypes = _db.ContactTypes?.Where(x => x.IsDeleted == false).ToList();
            if (contacttypes == null)
            {
                return list;
            }
            return contacttypes;
        }
        public async Task<ResultWithMessage> PostContactTypeAsync(ContactType model)
        {
            var contacttype = _db.ContactTypes?.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish));
            if (contacttype != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Contact Type {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Contact Type {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"نوع جهة الاتصال {model.NameArabic} موجود بالفعل !!!"
                };
            }
            await _db.ContactTypes.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public ResultWithMessage PutContactType(int id, ContactType model)
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
            var contacttype = _db.ContactTypes?.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(contacttype).State = EntityState.Detached;
            if (contacttype == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Contact Type Not Found !!!",
                    MessageEnglish = $@"Contact Type Not Found !!!",
                    MessageArabic = "نوع جهة الاتصال غير موجودة !!!"
                };
            }
            contacttype = model;
            _db.Entry(contacttype).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = contacttype };

        }

        //public async Task<ResultWithMessage> DeleteContactTypeAsync(int id)
        //{
        //    var contacttype = _db.ContactTypes.Find(id);
        //    if (contacttype == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Contact Type Not Found !!!" };
        //    }
        //    _db.ContactTypes.Remove(contacttype);
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Contact Type {contacttype.NameArabic} Deleted !!!" };
        //}




        //PaymentType



        public List<PaymentType> GetAllPaymentTypes()
        {
            var list = new List<PaymentType>();
            list = _db.PaymentTypes?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<PaymentType>();
        }
        public async Task<ResultWithMessage> PostPaymentTypeAsync(PaymentType model)
        {
            var paymenttype = _db.PaymentTypes?.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish));
            if (paymenttype != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Payment Type {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Payment Type {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"نوع الدفع {model.NameArabic} موجود مسبقاً !!!"
                };
            }
            await _db.PaymentTypes.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public ResultWithMessage PutPaymentType(int id, PaymentType model)
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
            var paymenttype = _db.PaymentTypes?.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(paymenttype).State = EntityState.Detached;
            if (paymenttype == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Payment Type  Not Found !!!",
                    MessageEnglish = $@"Payment Type  Not Found !!!",
                    MessageArabic = "نوع الدفع غير موجود !!!"
                };
            }
            paymenttype = model;
            _db.Entry(paymenttype).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = paymenttype };

        }

        //public async Task<ResultWithMessage> DeletePaymentTypeAsync(int id)
        //{
        //    var paymenttype = _db.PaymentTypes.Find(id);
        //    if (paymenttype == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Payment Type Not Found !!!" };
        //    }
        //    paymenttype.IsDeleted = true;
        //    _db.Entry(paymenttype).State = EntityState.Modified;
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Payment Type {paymenttype.NameArabic} Deleted !!!" };
        //}


        //Request Type



        public List<RequestType> GetAllRequestTypes()
        {
            var list = _db.RequestTypes?.Where(x => x.IsDeleted == false)
                                    .ToList();
            return list ?? new List<RequestType>();
        }
        public async Task<ResultWithMessage> PostRequestTypeAsync(RequestType model)
        {
            var requesttype = _db.RequestTypes?.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish));
            if (requesttype != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Request Type {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Request Type {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"نوع الطلب {model.NameArabic} موجود بالفعل !!!"
                };
            }
            await _db.RequestTypes.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }

        public ResultWithMessage PutRequestType(int id, RequestType model)
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
            var requesttype = _db.RequestTypes?.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(requesttype).State = EntityState.Detached;
            if (requesttype == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Request Type Not Found !!!",
                    MessageEnglish = $@"Request Type Not Found !!!",
                    MessageArabic = "نوع الطلب غير موجود !!!"
                };
            }
            requesttype = model;
            _db.Entry(requesttype).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = requesttype };

        }

        //public async Task<ResultWithMessage> DeleteRequestTypeAsync(int id)
        //{
        //    var requesttype = _db.RequestTypes?.Find(id);
        //    if (requesttype == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Request Type Not Found !!!" };
        //    }
        //    requesttype.IsDeleted = true;
        //    _db.Entry(requesttype).State = EntityState.Modified;
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Request Type {requesttype.NameArabic} Deleted !!!" };
        //}
        //public List<RequestType> GetAllRequestTypesByOfficeTypeId(int id)
        //{
        //    var list = new List<RequestTypeViewModel>();
        //    list = _db.RequestTypes?.Include(x => x.Parent).Where(x => x.IsDeleted == false && x.ParentId == id)
        //                            .Select(x => new RequestTypeViewModel(x))
        //                            .ToList();
        //    return list ?? new List<RequestTypeViewModel>();
        //}


        //OwnerPositionType
        public List<OwnerPositionType> GetAllOwnerPositionTypes()
        {
            var list = new List<OwnerPositionType>();
            var ownerPositionType = _db.OwnerPositionTypes?.Where(x => x.IsDeleted == false).ToList();
            if (ownerPositionType == null)
            {
                return list;
            }
            return ownerPositionType;
        }
        public async Task<ResultWithMessage> PostOwnerPositionTypeAsync(OwnerPositionType model)
        {
            var ownerPositionType = _db.OwnerPositionTypes?.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish));
            if (ownerPositionType != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Owner Position Type {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Owner Position Type {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"نوع المنصب {model.NameArabic} موجود مسبقاً !!!"
                };
            }
            await _db.OwnerPositionTypes.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }

        public ResultWithMessage PutOwnerPositionType(int id, OwnerPositionType model)
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
            var ownerPositionType = _db.OwnerPositionTypes?.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(ownerPositionType).State = EntityState.Detached;
            if (ownerPositionType == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Owner Position Type Not Found !!!",
                    MessageEnglish = $@"Owner Position Type Not Found !!!",
                    MessageArabic = "نوع المنصب غير موجود !!!"
                };
            }
            ownerPositionType = model;
            _db.Entry(ownerPositionType).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = ownerPositionType };

        }
        //public async Task<ResultWithMessage> DeleteOwnerPositionTypeAsync(int id)
        //{
        //    var ownerPositionType = _db.OwnerPositionTypes?.Find(id);
        //    if (ownerPositionType == null)
        //    {
        //        return new ResultWithMessage { Success = false, Message = $@"Owner Position Type Not Found !!!" };
        //    }
        //    _db.OwnerPositionTypes.Remove(ownerPositionType);
        //    _db.SaveChanges();
        //    return new ResultWithMessage { Success = true, Message = $@"Office Document Type {ownerPositionType.NameArabic} Deleted !!!" };
        //}


        //Nationality


        public List<Nationality> GetAllNationalities()
        {
            var list = new List<Nationality>();
            list = _db.Nationalities?.Where(x => x.IsDeleted == false).ToList();

            return list ?? new List<Nationality>();
        }
    }
}
