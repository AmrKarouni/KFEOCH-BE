using KFEOCH.Models.Dictionaries;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KFEOCH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryService _dictionaryService;
        public DictionaryController(IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }
        [HttpPost("countries")]
        public async Task<ActionResult> PostCountryAsync(Country model)
        {
            var result = await _dictionaryService.PostCountryAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
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
        public IActionResult PutCountry(int id, Country model)
        {
            var result = _dictionaryService.PutCountry(id, model);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
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
        public async Task<ActionResult> PostGovernorateAsync(Governorate model)
        {
            var result = await _dictionaryService.PostGovernorateAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
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
        public IActionResult PutGovernorate(int id, Governorate model)
        {
            var result = _dictionaryService.PutGovernorate(id, model);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

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
        public async Task<ActionResult> PostAreaAsync(Area model)
        {
            var result = await _dictionaryService.PostAreaAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
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
        public IActionResult PutArea(int id,Area model)
        {
            var result = _dictionaryService.PutArea(id,model);
            if (result.Success == false)
            {
                return BadRequest(new { message =result.Message });
            }
            return Ok(result.Result);
        }

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
        public async Task<ActionResult> PostCertificateEntityAsync(CertificateEntity model)
        {
            var result = await _dictionaryService.PostCertificateEntityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("certificate-entities/{id}")]
        public ActionResult PutCertificateEntity(int id,CertificateEntity model)
        {
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
        public async Task<ActionResult> PostCourseCategoryAsync(CourseCategory model)
        {
            var result = await _dictionaryService.PostCourseCategoryAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        [HttpPut("course-categories/{id}")]
        public IActionResult PutCourseCategory(int id, CourseCategory model)
        {
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
        public async Task<ActionResult> PostOfficeActivityAsync(Activity model)
        {
            var result = await _dictionaryService.PostOfficeActivityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("office-activities/{id}")]
        public IActionResult PutOfficeActivity(int id, Activity model)
        {
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
        public async Task<ActionResult> PostOfficeEntityAsync(OfficeEntity model)
        {
            var result = await _dictionaryService.PostOfficeEntityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("office-entities/{id}")]
        public ActionResult PutOfficeEntity(int id, OfficeEntity model)
        {
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
        public async Task<ActionResult> PostOfficeLegalEntityAsync(OfficeLegalEntity model)
        {
            var result = await _dictionaryService.PostOfficeLegalEntityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        [HttpPut("office-legal-entities/{id}")]
        public IActionResult PutOfficeLegalEntity(int id, OfficeLegalEntity model)
        {
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
        public async Task<ActionResult> PostOfficeOwnerSpecialityAsync(OfficeOwnerSpeciality model)
        {
            var result = await _dictionaryService.PostOfficeOwnerSpecialityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("office-owner-specialities/{id}")]
        public ActionResult PutOfficeOwnerSpeciality(int id, OfficeOwnerSpeciality model)
        {
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
        public async Task<ActionResult> PostOfficeSpecialityAsync(Speciality model)
        {
            var result = await _dictionaryService.PostOfficeSpecialityAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        [HttpPut("office-specialities/{id}")]
        public ActionResult PutOfficeSpeciality(int id, Speciality model)
        {
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
        public async Task<ActionResult> PostOfficeStatusAsync(OfficeStatus model)
        {
            var result = await _dictionaryService.PostOfficeStatusAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("office-statuses/{id}")]
        public ActionResult PutOfficeStatus(int id, OfficeStatus model)
        {
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
        public async Task<ActionResult> PostOfficeTypeAsync(OfficeType model)
        {
            var result = await _dictionaryService.PostOfficeTypeAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("office-types/{id}")]
        public ActionResult PutOfficeType(int id, OfficeType model)
        {
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
        public async Task<ActionResult> PostOfficeDocumentTypeAsync(OfficeDocumentType model)
        {
            var result = await _dictionaryService.PostOfficeDocumentTypeAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("office-document-types/{id}")]
        public ActionResult PutOfficeDocumentType(int id, OfficeDocumentType model)
        {
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
        public async Task<ActionResult> PostOwnerDocumentTypeAsync(OwnerDocumentType model)
        {
            var result = await _dictionaryService.PostOwnerDocumentTypeAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("owner-document-types/{id}")]
        public ActionResult PutOwnerDocumentType(int id, OwnerDocumentType model)
        {
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
        public async Task<ActionResult> PostContactTypeAsync(ContactType model)
        {
            var result = await _dictionaryService.PostContactTypeAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("contact-types/{id}")]
        public ActionResult PutContactType(int id, ContactType model)
        {
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
        public async Task<ActionResult> PostPaymentTypeAsync(PaymentType model)
        {
            var result = await _dictionaryService.PostPaymentTypeAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("payment-types/{id}")]
        public ActionResult PutPaymentType(int id, PaymentType model)
        {
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

        [HttpPost("request-types")]
        public async Task<ActionResult> PostRequestTypeAsync(RequestType model)
        {
            var result = await _dictionaryService.PostRequestTypeAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("request-types/{id}")]
        public ActionResult PutRequestType(int id, RequestType model)
        {
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

        [HttpGet("request-types/{id}")]
        public IActionResult GetAllRequestTypesByOfficeTypeId(int id)
        {
            var result = _dictionaryService.GetAllRequestTypesByOfficeTypeId(id);
            if (result == null)
            {
                return BadRequest(new { message = "No Request Type Found!!!" });
            }
            return Ok(result);
        }



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
        public async Task<ActionResult> PostOwnerPositionTypeAsync(OwnerPositionType model)
        {
            var result = await _dictionaryService.PostOwnerPositionTypeAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpPut("office-owner-positions/{id}")]
        public ActionResult PutOwnerPositionType(int id, OwnerPositionType model)
        {
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
    }
}
