using KFEOCH.Models.Dictionaries;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IDictionaryService
    {
        //Area
        List<AreaViewModel> GetAllAreas();
        Task<ResultWithMessage> PostAreaAsync(Area model);
        //Task<ResultWithMessage> DeleteAreaAsync(int id);
        List<Area> GetAreasByGovernorateId(int id);
        ResultWithMessage PutArea(int id, Area model);
        

        //Governorate
        List<GovernorateViewModel> GetAllGovernorates();
        Task<ResultWithMessage> PostGovernorateAsync(Governorate model);
        //Task<ResultWithMessage> DeleteGovernorateAsync(int id);
        ResultWithMessage PutGovernorate(int id, Governorate model);
        List<Governorate> GetGovernoratesByCountryId(int id);

        //Country
        List<Country> GetAllCountries();
        Task<ResultWithMessage> PostCountryAsync(Country model);
        //Task<ResultWithMessage> DeleteCountryAsync(int id);
        List<Country> GetLocationsByCountryId(int id);
        ResultWithMessage PutCountry(int id, Country model);

        //CertificateEntity
        List<CertificateEntity> GetAllCertificateEntity();
        Task<ResultWithMessage> PostCertificateEntityAsync(CertificateEntity model);
        ResultWithMessage PutCertificateEntity(int id, CertificateEntity model);
        //Task<ResultWithMessage> DeleteCertificateEntityAsync(int id);

        //CourseCategory
        List<CourseCategory> GetAllCourseCategories();
        Task<ResultWithMessage> PostCourseCategoryAsync(CourseCategory model);
        ResultWithMessage PutCourseCategory(int id, CourseCategory model);
        //Task<ResultWithMessage> DeleteCourseCategoryAsync(int id);

        //Gender
        List<Gender> GetAllGenders();

        //OfficeActivity
        List<ActivityViewModel> GetAllOfficeActivities();
        Task<ResultWithMessage> PostOfficeActivityAsync(Activity model);
        ResultWithMessage PutOfficeActivity(int id, Activity model);
        //Task<ResultWithMessage> DeleteOfficeActivityAsync(int id);
        List<ActivityViewModel> GetAllOfficeActivitiesByOfficeTypeId(int id);

        //OfficeEntity
        List<OfficeEntity> GetAllOfficeEntities();
        Task<ResultWithMessage> PostOfficeEntityAsync(OfficeEntity model);
        ResultWithMessage PutOfficeEntity(int id, OfficeEntity model);
        //Task<ResultWithMessage> DeleteOfficeEntityAsync(int id);

        //OfficeLegalEntity
        List<OfficeLegalEntity> GetAllOfficeLegalEntities();
        Task<ResultWithMessage> PostOfficeLegalEntityAsync(OfficeLegalEntity model);
        ResultWithMessage PutOfficeLegalEntity(int id, OfficeLegalEntity model);
        //Task<ResultWithMessage> DeleteOfficeLegalEntityAsync(int id);

        //OfficeSpeciality
        List<SpecialityViewModel> GetAllOfficeSpecialities();
        Task<ResultWithMessage> PostOfficeSpecialityAsync(Speciality model);
        //Task<ResultWithMessage> DeleteOfficeSpecialityAsync(int id);
        ResultWithMessage PutOfficeSpeciality(int id, Speciality model);
        List<SpecialityViewModel> GetAllOfficeSpecialitiesByOfficeTypeId(int id);

        //OfficeOwnerSpeciality
        List<OfficeOwnerSpeciality> GetAllOfficeOwnerSpecialities();
        Task<ResultWithMessage> PostOfficeOwnerSpecialityAsync(OfficeOwnerSpeciality model);
        ResultWithMessage PutOfficeOwnerSpeciality(int id, OfficeOwnerSpeciality model);
        //Task<ResultWithMessage> DeleteOfficeOwnerSpecialityAsync(int id);



        //OfficeStatus
        List<OfficeStatus> GetAllOfficeStatuses();
        Task<ResultWithMessage> PostOfficeStatusAsync(OfficeStatus model);
        ResultWithMessage PutOfficeStatus(int id, OfficeStatus model);
        //Task<ResultWithMessage> DeleteOfficeStatusAsync(int id);

        //OfficeType
        List<OfficeType> GetAllOfficeTypes();
        Task<ResultWithMessage> PostOfficeTypeAsync(OfficeType model);
        ResultWithMessage PutOfficeType(int id, OfficeType model);
        //Task<ResultWithMessage> DeleteOfficeTypeAsync(int id);
        List<OfficeType> GetAllOfficeTypesWithDetials(int id);

        //OfficeDocumentType
        List<OfficeDocumentType> GetAllOfficeDocumentTypes();
        Task<ResultWithMessage> PostOfficeDocumentTypeAsync(OfficeDocumentType model);
        ResultWithMessage PutOfficeDocumentType(int id, OfficeDocumentType model);
        //Task<ResultWithMessage> DeleteOfficeDocumentTypeAsync(int id);

        //OwnerDocumentType
        List<OwnerDocumentType> GetAllOwnerDocumentTypes();
        Task<ResultWithMessage> PostOwnerDocumentTypeAsync(OwnerDocumentType model);
        ResultWithMessage PutOwnerDocumentType(int id, OwnerDocumentType model);
        //Task<ResultWithMessage> DeleteOwnerDocumentTypeAsync(int id);

        //Contact Type
        List<ContactType> GetAllContactTypes();
        Task<ResultWithMessage> PostContactTypeAsync(ContactType model);
        ResultWithMessage PutContactType(int id, ContactType model);
        //Task<ResultWithMessage> DeleteContactTypeAsync(int id);

        //PaymentType
        List<PaymentType> GetAllPaymentTypes();
        Task<ResultWithMessage> PostPaymentTypeAsync(PaymentType model);
        ResultWithMessage PutPaymentType(int id, PaymentType model);
        //Task<ResultWithMessage> DeletePaymentTypeAsync(int id);

        //Request Type
        List<RequestType> GetAllRequestTypes();
        Task<ResultWithMessage> PostRequestTypeAsync(RequestType model);
        ResultWithMessage PutRequestType(int id, RequestType model);
        //Task<ResultWithMessage> DeleteRequestTypeAsync(int id);
        //List<RequestTypeViewModel> GetAllRequestTypesByOfficeTypeId(int id);

        //OwnerPositionType
        List<OwnerPositionType> GetAllOwnerPositionTypes();
        Task<ResultWithMessage> PostOwnerPositionTypeAsync(OwnerPositionType model);
        ResultWithMessage PutOwnerPositionType(int id, OwnerPositionType model);
        //Task<ResultWithMessage> DeleteOwnerPositionTypeAsync(int id);

        //Nationality
        List<Nationality> GetAllNationalities();
    }
}
