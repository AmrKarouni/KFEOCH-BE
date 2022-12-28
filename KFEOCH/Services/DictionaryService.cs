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
            var a = _db.Areas.Where(x => (x.NameArabic == model.NameArabic && x.ParentId == model.ParentId)
                                    || (x.NameEnglish == model.NameEnglish && x.ParentId == model.ParentId)
                                    ).FirstOrDefault();
            if (a != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Area {model.NameArabic} Already Exist !!!" };
            }
            await _db.Areas.AddAsync(model);
            _db.SaveChanges();
            var result = new AreaViewModel(model);
            return new ResultWithMessage { Success = true, Result = result };

        }
        public async Task<ResultWithMessage> DeleteAreaAsync(int id)
        {
            var area = _db.Areas.Find(id);
            if (area == null)
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
            list = _db.Areas?.Where(a => a.ParentId == id).ToList();
            return list ?? new List<Area>();
        }

        //Governorate
        public List<GovernorateViewModel> GetAllGovernorates()
        {
            var list = new List<GovernorateViewModel>();
            list = _db.Governorates?.Include(x=> x.Parent).Where(x => x.IsDeleted == false)
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
                return new ResultWithMessage { Success = false, Message = $@"Governorate {model.NameArabic} Already Exist !!!" };
            }
            await _db.Governorates.AddAsync(model);
            _db.SaveChanges();
            var result = new GovernorateViewModel(model);
            return new ResultWithMessage { Success = true, Result = result };
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
            list = _db.Governorates?.Where(a => a.ParentId == id).ToList();
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
                return new ResultWithMessage { Success = false, Message = $@"Office Activity {model.NameArabic} Already Exist !!!" };
            }
            await _db.Activities.AddAsync(model);
            _db.SaveChanges();
            var result = new ActivityViewModel(model);
            return new ResultWithMessage { Success = true, Result = result };
        }
        public async Task<ResultWithMessage> DeleteOfficeActivityAsync(int id)
        {
            var officeactivity = _db.Activities?.Find(id);
            if (officeactivity == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Activity Not Found !!!" };
            }
            officeactivity.IsDeleted = true;
            _db.Entry(officeactivity).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Office Activity {officeactivity.NameArabic} Deleted !!!" };
        }
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
                return new ResultWithMessage { Success = false, Message = $@"Office Speciality {model.NameArabic} Already Exist !!!" };
            }
            await _db.Specialities.AddAsync(model);
            _db.SaveChanges();
            var result = new SpecialityViewModel(model);
            return new ResultWithMessage { Success = true, Result = result };
        }
        public async Task<ResultWithMessage> DeleteOfficeSpecialityAsync(int id)
        {
            var OfficeSpeciality = _db.Specialities?.Find(id);
            if (OfficeSpeciality == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Speciality Not Found !!!" };
            }
            OfficeSpeciality.IsDeleted = true;
            _db.Entry(OfficeSpeciality).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Office Speciality {OfficeSpeciality.NameArabic} Deleted !!!" };
        }
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
            list = _db.Statuses?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<OfficeStatus>();
        }
        public async Task<ResultWithMessage> PostOfficeStatusAsync(OfficeStatus model)
        {
            var officeStatus = _db.Statuses?.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                             || (x.NameEnglish == model.NameEnglish));
            if (officeStatus != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Status {model.NameArabic} Already Exist !!!" };
            }
            await _db.Statuses.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeleteOfficeStatusAsync(int id)
        {
            var officeStatus = _db.Statuses?.Find(id);
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
            list = _db.OfficeTypes?.Where(x => (x.IsDeleted == false) && (x.IsAdmin == false)).ToList();
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
            var officeDocumentType = _db.OfficeDocumentTypes.Where(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish)
                                  ).FirstOrDefault();
            if (officeDocumentType != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Document Type {model.NameEnglish} | {model.NameArabic} Already Exist !!!" };
            }
            await _db.OfficeDocumentTypes.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeleteOfficeDocumentTypeAsync(int id)
        {
            var officeentity = _db.OfficeDocumentTypes?.Find(id);
            if (officeentity == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Document Type Not Found !!!" };
            }
            _db.OfficeDocumentTypes.Remove(officeentity);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Office Document Type {officeentity.NameArabic} Deleted !!!" };
        }

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
            var ownerDocumentType = _db.OwnerDocumentTypes?.Where(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish)
                                  ).FirstOrDefault();
            if (ownerDocumentType != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Owner Document Type {model.NameEnglish} | {model.NameArabic} Already Exist !!!" };
            }
            await _db.OwnerDocumentTypes.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeleteOwnerDocumentTypeAsync(int id)
        {
            var ownerDocumentType = _db.OwnerDocumentTypes?.Find(id);
            if (ownerDocumentType == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Document Type Not Found !!!" };
            }
            _db.OwnerDocumentTypes.Remove(ownerDocumentType);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Owner Document Type {ownerDocumentType.NameEnglish} | {ownerDocumentType.NameArabic} Deleted !!!" };
        }

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
            var contacttype = _db.ContactTypes?.Where(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish)
                                  ).FirstOrDefault();
            if (contacttype != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Contact Type {model.NameArabic} Already Exist !!!" };
            }
            await _db.ContactTypes.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeleteContactTypeAsync(int id)
        {
            var contacttype = _db.ContactTypes.Find(id);
            if (contacttype == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Contact Type Not Found !!!" };
            }
            _db.ContactTypes.Remove(contacttype);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Contact Type {contacttype.NameArabic} Deleted !!!" };
        }

        //PaymentType
        public List<PaymentType> GetAllPaymentTypes()
        {
            var list = new List<PaymentType>();
            list = _db.PaymentTypes?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<PaymentType>();
        }
        public async Task<ResultWithMessage> PostPaymentTypeAsync(PaymentType model)
        {
            var paymenttype = _db.PaymentTypes?.Where(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish)
                                  ).FirstOrDefault();
            if (paymenttype != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Payment Type {model.NameArabic} Already Exist !!!" };
            }
            await _db.PaymentTypes.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeletePaymentTypeAsync(int id)
        {
            var paymenttype = _db.PaymentTypes.Find(id);
            if (paymenttype == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Payment Type Not Found !!!" };
            }
            paymenttype.IsDeleted = true;
            _db.Entry(paymenttype).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Payment Type {paymenttype.NameArabic} Deleted !!!" };
        }


        //Request Type
        public List<RequestTypeViewModel> GetAllRequestTypes()
        {
            var list = new List<RequestTypeViewModel>();
            list = _db.RequestTypes?.Include(x => x.Parent).Where(x => x.IsDeleted == false)
                                    .Select(x => new RequestTypeViewModel(x))
                                    .ToList();
            return list ?? new List<RequestTypeViewModel>();
        }
        public async Task<ResultWithMessage> PostRequestTypeAsync(RequestType model)
        {
            var requesttype = _db.RequestTypes?.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish));
            if (requesttype != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Request Type {model.NameArabic} Already Exist !!!" };
            }
            await _db.RequestTypes.AddAsync(model);
            _db.SaveChanges();
            var result = new RequestTypeViewModel(model);
            return new ResultWithMessage { Success = true, Result = result };
        }
        public async Task<ResultWithMessage> DeleteRequestTypeAsync(int id)
        {
            var requesttype = _db.RequestTypes?.Find(id);
            if (requesttype == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Request Type Not Found !!!" };
            }
            requesttype.IsDeleted = true;
            _db.Entry(requesttype).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Request Type {requesttype.NameArabic} Deleted !!!" };
        }
        public List<RequestTypeViewModel> GetAllRequestTypesByOfficeTypeId(int id)
        {
            var list = new List<RequestTypeViewModel>();
            list = _db.RequestTypes?.Include(x => x.Parent).Where(x => x.IsDeleted == false && x.ParentId == id)
                                    .Select(x => new RequestTypeViewModel(x))
                                    .ToList();
            return list ?? new List<RequestTypeViewModel>();
        }


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
            var ownerPositionType = _db.OwnerPositionTypes?.Where(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish)
                                  ).FirstOrDefault();
            if (ownerPositionType != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Owner Position Type {model.NameEnglish} | {model.NameArabic} Already Exist !!!" };
            }
            await _db.OwnerPositionTypes.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        public async Task<ResultWithMessage> DeleteOwnerPositionTypeAsync(int id)
        {
            var ownerPositionType = _db.OwnerPositionTypes?.Find(id);
            if (ownerPositionType == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Owner Position Type Not Found !!!" };
            }
            _db.OwnerPositionTypes.Remove(ownerPositionType);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Office Document Type {ownerPositionType.NameArabic} Deleted !!!" };
        }


        //Nationality
        public List<Nationality> GetAllNationalities()
        {
            var list = new List<Nationality>();
            list = _db.Nationalities?.Where(x => x.IsDeleted == false).ToList();

            return list ?? new List<Nationality>();
        }
    }
}
