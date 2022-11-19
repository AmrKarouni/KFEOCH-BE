using KFEOCH.Models.Dictionaries;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IDictionaryService
    {
        //Area
        List<Area> GetAllAreas();
        Task<ResultWithMessage> PostAreaAsync(Area model);
        Task<ResultWithMessage> DeleteAreaAsync(int id);
        List<Area> GetAreasByGovernorateId(int id);

        //Governorate
        List<Governorate> GetAllGovernorates();
        Task<ResultWithMessage> PostGovernorateAsync(Governorate model);
        Task<ResultWithMessage> DeleteGovernorateAsync(int id);
        List<Governorate> GetGovernoratesByCountryId(int id);

        //Country
        List<Country> GetAllCountries();
        Task<ResultWithMessage> PostCountryAsync(Country model);
        Task<ResultWithMessage> DeleteCountryAsync(int id);
        List<Country> GetLocationsByCountryId(int id);

        //CertificateEntity
        List<CertificateEntity> GetAllCertificateEntity();
        Task<ResultWithMessage> PostCertificateEntityAsync(CertificateEntity model);
        Task<ResultWithMessage> DeleteCertificateEntityAsync(int id);

        //CourseCategory
        List<CourseCategory> GetAllCourseCategories();
        Task<ResultWithMessage> PostCourseCategoryAsync(CourseCategory model);
        Task<ResultWithMessage> DeleteCourseCategoryAsync(int id);

        //Gender
        List<Gender> GetAllGenders();

        //OfficeActivity
        List<OfficeActivity> GetAllOfficeActivities();
        Task<ResultWithMessage> PostOfficeActivityAsync(OfficeActivity model);
        Task<ResultWithMessage> DeleteOfficeActivityAsync(int id);
        List<OfficeActivity> GetAllOfficeActivitiesByOfficeTypeId(int id);

        //OfficeEntity
        List<OfficeEntity> GetAllOfficeEntities();
        Task<ResultWithMessage> PostOfficeEntityAsync(OfficeEntity model);
        Task<ResultWithMessage> DeleteOfficeEntityAsync(int id);

        //OfficeLegalEntity
        List<OfficeLegalEntity> GetAllOfficeLegalEntities();
        Task<ResultWithMessage> PostOfficeLegalEntityAsync(OfficeLegalEntity model);
        Task<ResultWithMessage> DeleteOfficeLegalEntityAsync(int id);

        //OfficeSpeciality
        List<OfficeSpeciality> GetAllOfficeSpecialities();
        Task<ResultWithMessage> PostOfficeSpecialityAsync(OfficeSpeciality model);
        Task<ResultWithMessage> DeleteOfficeSpecialityAsync(int id);
        List<OfficeSpeciality> GetAllOfficeSpecialitiesByOfficeTypeId(int id);

        //OfficeOwnerSpeciality
        List<OfficeOwnerSpeciality> GetAllOfficeOwnerSpecialities();
        Task<ResultWithMessage> PostOfficeOwnerSpecialityAsync(OfficeOwnerSpeciality model);
        Task<ResultWithMessage> DeleteOfficeOwnerSpecialityAsync(int id);


        //OfficeStatus
        List<OfficeStatus> GetAllOfficeStatuses();
        Task<ResultWithMessage> PostOfficeStatusAsync(OfficeStatus model);
        Task<ResultWithMessage> DeleteOfficeStatusAsync(int id);

        //OfficeType
        List<OfficeType> GetAllOfficeTypes();
        Task<ResultWithMessage> PostOfficeTypeAsync(OfficeType model);
        Task<ResultWithMessage> DeleteOfficeTypeAsync(int id);
        List<OfficeType> GetAllOfficeTypesWithDetials(int id);
    }
}
