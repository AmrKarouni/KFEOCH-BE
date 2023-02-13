using KFEOCH.Models.Dictionaries;
using KFEOCH.Models.Site;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KFEOCH.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryService _dictionaryService;
        private readonly ISiteService _siteService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string[] roles;
        public DictionaryController(IDictionaryService dictionaryService,
                                    ISiteService siteService,
                                    IUserService userService,
                                    IHttpContextAccessor httpContextAccessor)
        {
            _dictionaryService = dictionaryService;
            _siteService = siteService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            roles = new string[] { "SuperUser", "Administrator", "DictionaryManager" };
            
        }

        [HttpPost("countries")]
        public async Task<IActionResult> PostCountryAsync(Country model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostCountryAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [AllowAnonymous]
        [HttpGet("countries")]
        public IActionResult GetAllCountries()
        {
            var result = _dictionaryService.GetAllCountries();
            if (result == null)
            {
                return BadRequest(new { message = "No Country Found!!!" });
            }
            return Ok(result);
        }

        [HttpPut("countries/{id}")]
        public async Task<IActionResult> PutCountry(int id, Country model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutCountry(id, model);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [AllowAnonymous]
        [HttpGet("country-locations")]
        public IActionResult GetLocationsByCountryId(int id)
        {
            var result = _dictionaryService.GetLocationsByCountryId(id);
            if (result == null)
            {
                return BadRequest(new { message = "Country Not Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("governorates")]
        public async Task<IActionResult> PostGovernorateAsync(Governorate model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostGovernorateAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        
        
        [AllowAnonymous]
        [HttpGet("governorates")]
        public IActionResult GetAllGovernorates()
        {
            var result = _dictionaryService.GetAllGovernorates();
            if (result == null)
            {
                return BadRequest(new { message = "No Governorate Found!!!" });
            }
            return Ok(result);
        }


        //[HttpDelete("governorates/{id}")]
        //public async Task<IActionResult> DeleteGovernorateAsync(int id)
        //{
        //    var result = await _dictionaryService.DeleteGovernorateAsync(id);
        //    if (result.Success == false)
        //    {
        //        return BadRequest(new { message = result.Message });
        //    }
        //    return Ok(result.Message);
        //}

        [HttpPut("governorates/{id}")]
        public async Task<IActionResult> PutGovernorate(int id, Governorate model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutGovernorate(id, model);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [AllowAnonymous]
        [HttpGet("governorate-locations/{id}")]
        public IActionResult GetGovernoratesByCountryId(int id)
        {
            var result = _dictionaryService.GetGovernoratesByCountryId(id);
            if (result == null)
            {
                return BadRequest(new { message = "Country Not Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("areas")]
        public async Task<IActionResult> PostAreaAsync(Area model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostAreaAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }


        [AllowAnonymous]
        [HttpGet("areas")]
        public IActionResult GetAllAreas()
        {
            var result = _dictionaryService.GetAllAreas();
            if (result == null)
            {
                return BadRequest(new { message = "No Area Found!!!" });
            }
            return Ok(result);
        }

        [HttpPut("areas/{id}")]
        public async Task<IActionResult> PutArea(int id,Area model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutArea(id,model);
            if (result.Success == false)
            {
                return BadRequest(new { message =result.Message });
            }
            return Ok(result.Result);
        }

        [AllowAnonymous]
        [HttpGet("area-locations/{id}")]
        public IActionResult GetAreasByGovernorateId(int id)
        {
            var result = _dictionaryService.GetAreasByGovernorateId(id);
            if (result == null)
            {
                return BadRequest(new { message = "Areas Not Found!!!" });
            }
            return Ok(result);
        }


        [AllowAnonymous]
        [HttpGet("certificate-entities")]
        public IActionResult GetAllCertificateEntity()
        {
            var result = _dictionaryService.GetAllCertificateEntity();
            if (result == null)
            {
                return BadRequest(new { message = "No Certificate Entity Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("certificate-entities")]
        public async Task<IActionResult> PostCertificateEntityAsync(CertificateEntity model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostCertificateEntityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("certificate-entities/{id}")]
        public async Task<IActionResult> PutCertificateEntity(int id,CertificateEntity model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result =  _dictionaryService.PutCertificateEntity(id,model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }


        //[HttpDelete("certificate-entities/{id}")]
        //public IActionResult DeleteCertificateEntityAsync(int id)
        //{
        //    var result = _dictionaryService.DeleteCertificateEntityAsync(id);
        //    if (result == null)
        //    {
        //        return BadRequest(new { message = "No Certificate Entity Found!!!" });
        //    }
        //    return Ok(result);
        //}
        [AllowAnonymous]
        [HttpGet("course-categories")]
        public IActionResult GetAllCourseCategories()
        {
            var result = _dictionaryService.GetAllCourseCategories();
            if (result == null)
            {
                return BadRequest(new { message = "No Course Category Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("course-categories")]
        public async Task<IActionResult> PostCourseCategoryAsync(CourseCategory model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostCourseCategoryAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        
        [HttpPut("course-categories/{id}")]
        public async Task<IActionResult> PutCourseCategory(int id, CourseCategory model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result =  _dictionaryService.PutCourseCategory(id, model);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        //[HttpDelete("course-categories/{id}")]
        //public IActionResult DeleteCourseCategoryAsync(int id)
        //{
        //    var result = _dictionaryService.DeleteCourseCategoryAsync(id);
        //    if (result == null)
        //    {
        //        return BadRequest(new { message = "No Course Category Found!!!" });
        //    }
        //    return Ok(result);
        //}
        
        [AllowAnonymous]
        [HttpGet("genders")]
        public IActionResult GetAllGenders()
        {
            var result = _dictionaryService.GetAllGenders();
            if (result == null)
            {
                return BadRequest(new { message = "No Gender Found!!!" });
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("office-activities")]
        public IActionResult GetAllOfficeActivities()
        {
            var result = _dictionaryService.GetAllOfficeActivities();
            if (result == null)
            {
                return BadRequest(new { message = "No Office Activity Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("office-activities")]
        public async Task<IActionResult> PostOfficeActivityAsync(Activity model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostOfficeActivityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("office-activities/{id}")]
        public async Task<IActionResult> PutOfficeActivity(int id, Activity model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutOfficeActivity(id, model);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }


        //[HttpDelete("office-activities/{id}")]
        //public IActionResult DeleteOfficeActivityAsync(int id)
        //{
        //    var result = _dictionaryService.DeleteOfficeActivityAsync(id);
        //    if (result == null)
        //    {
        //        return BadRequest(new { message = "No Office Activity Found!!!" });
        //    }
        //    return Ok(result);
        //}
        [AllowAnonymous]
        [HttpGet("office-type-activities/{id}")]
        public IActionResult GetAllOfficeActivitiesByOfficeTypeId(int id)
        {
            var result = _dictionaryService.GetAllOfficeActivitiesByOfficeTypeId(id);
            if (result == null)
            {
                return BadRequest(new { message = "No Office Activity Found!!!" });
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("office-entities")]
        public IActionResult GetAllOfficeEntities()
        {
            var result = _dictionaryService.GetAllOfficeEntities();
            if (result == null)
            {
                return BadRequest(new { message = "No Office Entity Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("office-entities")]
        public async Task<IActionResult> PostOfficeEntityAsync(OfficeEntity model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostOfficeEntityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("office-entities/{id}")]
        public async Task<IActionResult> PutOfficeEntity(int id, OfficeEntity model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutOfficeEntity(id, model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        //[HttpDelete("office-entities/{id}")]
        //public IActionResult DeleteOfficeEntityAsync(int id)
        //{
        //    var result = _dictionaryService.DeleteOfficeEntityAsync(id);
        //    if (result == null)
        //    {
        //        return BadRequest(new { message = "No Office Entity Found!!!" });
        //    }
        //    return Ok(result);
        //}

        [AllowAnonymous]
        [HttpGet("office-legal-entities")]
        public IActionResult GetAllOfficeLegalEntities()
        {
            var result = _dictionaryService.GetAllOfficeLegalEntities();
            if (result == null)
            {
                return BadRequest(new { message = "No Office Legal Entity Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("office-legal-entities")]
        public async Task<IActionResult> PostOfficeLegalEntityAsync(OfficeLegalEntity model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostOfficeLegalEntityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        [HttpPut("office-legal-entities/{id}")]
        public async Task<IActionResult> PutOfficeLegalEntity(int id, OfficeLegalEntity model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutOfficeLegalEntity(id,model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        //[HttpDelete("office-legal-entities/{id}")]
        //public IActionResult DeleteOfficeLegalEntityAsync(int id)
        //{
        //    var result = _dictionaryService.DeleteOfficeLegalEntityAsync(id);
        //    if (result == null)
        //    {
        //        return BadRequest(new { message = "No Office Legal Entity Found!!!" });
        //    }
        //    return Ok(result);
        //}

        [AllowAnonymous]
        [HttpGet("office-owner-specialities")]
        public IActionResult GetAllOfficeOwnerSpecialities()
        {
            var result = _dictionaryService.GetAllOfficeOwnerSpecialities();
            if (result == null)
            {
                return BadRequest(new { message = "No Owner Speciality Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("office-owner-specialities")]
        public async Task<IActionResult> PostOfficeOwnerSpecialityAsync(OfficeOwnerSpeciality model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostOfficeOwnerSpecialityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("office-owner-specialities/{id}")]
        public async Task<IActionResult> PutOfficeOwnerSpeciality(int id, OfficeOwnerSpeciality model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutOfficeOwnerSpeciality(id, model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }


        //[HttpDelete("office-owner-specialities/{id}")]
        //public IActionResult DeleteOfficeOwnerSpecialityAsync(int id)
        //{
        //    var result = _dictionaryService.DeleteOfficeOwnerSpecialityAsync(id);
        //    if (result == null)
        //    {
        //        return BadRequest(new { message = "No Office Legal Entity Found!!!" });
        //    }
        //    return Ok(result);
        //}
        [AllowAnonymous]
        [HttpGet("office-specialities")]
        public IActionResult GetAllOfficeSpecialities()
        {
            var result = _dictionaryService.GetAllOfficeSpecialities();
            if (result == null)
            {
                return BadRequest(new { message = "No Office Speciality Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("office-specialities")]
        public async Task<IActionResult> PostOfficeSpecialityAsync(Speciality model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostOfficeSpecialityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        [HttpPut("office-specialities/{id}")]
        public async Task<IActionResult> PutOfficeSpeciality(int id, Speciality model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutOfficeSpeciality(id, model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        //[HttpDelete("office-specialities/{id}")]
        //public IActionResult DeleteOfficeSpecialityAsync(int id)
        //{
        //    var result = _dictionaryService.DeleteOfficeSpecialityAsync(id);
        //    if (result == null)
        //    {
        //        return BadRequest(new { message = "No Office Speciality Found!!!" });
        //    }
        //    return Ok(result);
        //}

        [AllowAnonymous]
        [HttpGet("office-type-specialities/{id}")]
        public IActionResult GetAllOfficeSpecialitiesByOfficeTypeId(int id)
        {
            var result = _dictionaryService.GetAllOfficeSpecialitiesByOfficeTypeId(id);
            if (result == null)
            {
                return BadRequest(new { message = "No Office Speciality Found!!!" });
            }
            return Ok(result);
        }
        
        [AllowAnonymous]
        [HttpGet("office-statuses")]
        public IActionResult GetAllOfficeStatuses()
        {
            var result = _dictionaryService.GetAllOfficeStatuses();
            if (result == null)
            {
                return BadRequest(new { message = "No Office Status Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("office-statuses")]
        public async Task<IActionResult> PostOfficeStatusAsync(OfficeStatus model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostOfficeStatusAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("office-statuses/{id}")]
        public async Task<IActionResult> PutOfficeStatus(int id, OfficeStatus model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutOfficeStatus(id, model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        //[HttpDelete("office-statuses/{id}")]
        //public IActionResult DeleteOfficeStatusAsync(int id)
        //{
        //    var result = _dictionaryService.DeleteOfficeStatusAsync(id);
        //    if (result == null)
        //    {
        //        return BadRequest(new { message = "No Office Status Found!!!" });
        //    }
        //    return Ok(result);
        //}

        [AllowAnonymous]
        [HttpGet("office-types")]
        public IActionResult GetAllOfficeTypes()
        {
            var result = _dictionaryService.GetAllOfficeTypes();
            if (result == null)
            {
                return BadRequest(new { message = "No Office Status Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("office-types")]
        public async Task<IActionResult> PostOfficeTypeAsync(OfficeType model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostOfficeTypeAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("office-types/{id}")]
        public async Task<IActionResult> PutOfficeType(int id, OfficeType model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutOfficeType(id, model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        //[HttpDelete("office-types/{id}")]
        //public IActionResult DeleteOfficeTypeAsync(int id)
        //{
        //    var result = _dictionaryService.DeleteOfficeTypeAsync(id);
        //    if (result == null)
        //    {
        //        return BadRequest(new { message = "No Office Type Found!!!" });
        //    }
        //    return Ok(result);
        //}

        [AllowAnonymous]
        [HttpGet("office-type-details/{id}")]
        public IActionResult GetAllOfficeTypesWithDetials(int id)
        {
            var result = _dictionaryService.GetAllOfficeTypesWithDetials(id);
            if (result == null)
            {
                return BadRequest(new { message = "No Office Type Found!!!" });
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("office-document-types")]
        public IActionResult GetAllOfficeDocumentTypes()
        {
            var result = _dictionaryService.GetAllOfficeDocumentTypes();
            if (result == null)
            {
                return BadRequest(new { message = "No Office Document Type Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("office-document-types")]
        public async Task<IActionResult> PostOfficeDocumentTypeAsync(OfficeDocumentType model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostOfficeDocumentTypeAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("office-document-types/{id}")]
        public async Task<IActionResult> PutOfficeDocumentType(int id, OfficeDocumentType model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutOfficeDocumentType(id, model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        //[HttpDelete("office-document-types/{id}")]
        //public IActionResult DeleteOfficeDocumentTypeAsync(int id)
        //{
        //    var result = _dictionaryService.DeleteOfficeDocumentTypeAsync(id);
        //    if (result == null)
        //    {
        //        return BadRequest(new { message = "No Office Document Type Found!!!" });
        //    }
        //    return Ok(result);
        //}

        [AllowAnonymous]
        [HttpGet("owner-document-types")]
        public IActionResult GetAllOwnerDocumentTypes()
        {
            var result = _dictionaryService.GetAllOwnerDocumentTypes();
            if (result == null)
            {
                return BadRequest(new { message = "No Owner Document Type Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("owner-document-types")]
        public async Task<IActionResult> PostOwnerDocumentTypeAsync(OwnerDocumentType model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostOwnerDocumentTypeAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("owner-document-types/{id}")]
        public async Task<IActionResult> PutOwnerDocumentType(int id, OwnerDocumentType model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutOwnerDocumentType(id, model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        //[HttpDelete("owner-document-types/{id}")]
        //public IActionResult DeleteOwnerDocumentTypeAsync(int id)
        //{
        //    var result = _dictionaryService.DeleteOwnerDocumentTypeAsync(id);
        //    if (result == null)
        //    {
        //        return BadRequest(new { message = "No Owner Document Type Found!!!" });
        //    }
        //    return Ok(result);
        //}

        [AllowAnonymous]
        [HttpGet("contact-types")]
        public IActionResult GetAllContactTypes()
        {
            var result = _dictionaryService.GetAllContactTypes();
            if (result == null)
            {
                return BadRequest(new { message = "No Contact Type Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("contact-types")]
        public async Task<IActionResult> PostContactTypeAsync(ContactType model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostContactTypeAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("contact-types/{id}")]
        public async Task<IActionResult> PutContactType(int id, ContactType model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutContactType(id, model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        //[HttpDelete("contact-types/{id}")]
        //public async Task<IActionResult> DeleteContactTypeAsync(int id)
        //{
        //    var result = await _dictionaryService.DeleteContactTypeAsync(id);
        //    if (result == null)
        //    {
        //        return BadRequest(new { message = "No Contact Type Found!!!" });
        //    }
        //    return Ok(result);
        //}

        [AllowAnonymous]
        [HttpGet("payment-types")]
        public IActionResult GetAllPaymentTypes()
        {
            var result = _dictionaryService.GetAllPaymentTypes();
            if (result == null)
            {
                return BadRequest(new { message = "No Payment Type Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("payment-types")]
        public async Task<IActionResult> PostPaymentTypeAsync(PaymentType model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostPaymentTypeAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("payment-types/{id}")]
        public async Task<IActionResult> PutPaymentType(int id, PaymentType model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutPaymentType(id, model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        //[HttpDelete("payment-types/{id}")]
        //public IActionResult DeletePaymentTypeAsync(int id)
        //{
        //    var result = _dictionaryService.DeletePaymentTypeAsync(id);
        //    if (result == null)
        //    {
        //        return BadRequest(new { message = "No Payment Type Found!!!" });
        //    }
        //    return Ok(result);
        //}

        [AllowAnonymous]
        [HttpGet("request-types")]
        public IActionResult GetAllRequestTypes()
        {
            var result = _dictionaryService.GetAllRequestTypes();
            if (result == null)
            {
                return BadRequest(new { message = "No Request Type Found!!!" });
            }
            return Ok(result);
        }

        //[HttpPost("request-types")]
        //public async Task<IActionResult> PostRequestTypeAsync(RequestType model)
        //{
        //    var result = await _dictionaryService.PostRequestTypeAsync(model);
        //    if (!result.Success)
        //    {
        //        return BadRequest(new { message = result.Message });
        //    }
        //    return Ok(result.Result);
        //}

        [HttpPut("request-types/{id}")]
        public async Task<IActionResult> PutRequestType(int id, RequestType model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutRequestType(id, model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        //[HttpDelete("request-types/{id}")]
        //public IActionResult DeleteRequestTypeAsync(int id)
        //{
        //    var result = _dictionaryService.DeleteRequestTypeAsync(id);
        //    if (result == null)
        //    {
        //        return BadRequest(new { message = "No Request Type Found!!!" });
        //    }
        //    return Ok(result);
        //}



        [AllowAnonymous]
        [HttpGet("office-owner-positions")]
        public IActionResult GetAllOwnerPositionTypes()
        {
            var result = _dictionaryService.GetAllOwnerPositionTypes ();
            if (result == null)
            {
                return BadRequest(new { message = "No Owner Speciality Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("office-owner-positions")]
        public async Task<IActionResult> PostOwnerPositionTypeAsync(OwnerPositionType model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = await _dictionaryService.PostOwnerPositionTypeAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("office-owner-positions/{id}")]
        public async Task<IActionResult> PutOwnerPositionType(int id, OwnerPositionType model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _dictionaryService.PutOwnerPositionType(id, model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        //[HttpDelete("office-owner-positions/{id}")]
        //public IActionResult DeleteOwnerPositionTypeAsync(int id)
        //{
        //    var result = _dictionaryService.DeleteOwnerPositionTypeAsync(id);
        //    if (result == null)
        //    {
        //        return BadRequest(new { message = "No Owner Position Type Found!!!" });
        //    }
        //    return Ok(result);
        //}


        [AllowAnonymous]
        [HttpGet("nationalities")]
        public IActionResult GetAllNationalities()
        {
            var result = _dictionaryService.GetAllNationalities();
            if (result == null)
            {
                return BadRequest(new { message = "No Nationality Found!!!" });
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("post-categories")]
        public IActionResult GetAllPostCategories()
        {
            var result = _siteService.GetAllPostCategories();
            if (result == null)
            {
                return BadRequest(new { message = "No Post Category Found!!!" });
            }
            return Ok(result);
        }

        [HttpPost("post-categories")]
        public async Task<IActionResult> AddPostCategoryAsync(PostCategory model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorize = await _userService.IsAuthorized(principal, roles);
            if (isAuthorize == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }

            //if (roles.FirstOrDefault(x => User.IsInRole(x)) == null)
            //{
            //    return Unauthorized();
            //}
            var result = await _siteService.AddPostCategoryAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("post-categories/{id}")]
        public async Task<IActionResult> PutOfficeEntity(int id, PostCategory model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, roles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _siteService.PutPostCategory(id, model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
    }
}
