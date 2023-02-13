using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Globalization;
using System.Security.Claims;

namespace KFEOCH.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private readonly IOfficeService _officeService;
        private readonly IOfficeOwnerService _officeOwnerService;
        private readonly IOfficeDocumentService _officeDocumentService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string[] roles;
        private readonly string[] adminroles;

        public OfficeController(IOfficeService officeService,
                                IOfficeOwnerService officeOwnerService,
                                IOfficeDocumentService officeDocumentService,
                                IUserService userService,
                                IHttpContextAccessor httpContextAccessor
                                )
        {
            _officeService = officeService;
            _officeOwnerService = officeOwnerService;
            _officeDocumentService = officeDocumentService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            roles = new string[] { "SuperUser", "Administrator", "Office", "OfficeManager" };
            adminroles = new string[] { "SuperUser", "Administrator", "OfficeManager" };
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetOfficeById(int id)
        {
            var result = _officeService.GetById(id);
            if (result == null)
            {
                return BadRequest(new { message = "No Office Found!!!" });
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOffice(int id, Office model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, adminroles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _officeService.PutOfficeAsync(id, model);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message
                });
            }
            return Ok(result.Result);
        }

        [HttpPut("update-info/{id}")]
        public async Task<IActionResult> PutOfficeInfo(int id, OfficePutBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });

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


            var result = await _officeService.PutOfficeInfo(id, model);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message
                });
            }
            return Ok(result.Result);
        }

        [HttpPost("upload-logo")]
        public async Task<IActionResult> UploadLogo([FromForm] FileModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });

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
            var result = await _officeService.UploadLogo(model);
            if (!result.Success)
            {
                return BadRequest(new {message = result.Message});
            }
            return Ok(result.Result);
        }

        [HttpDelete("delete-logo/{id}")]
        public async Task<IActionResult> DeleteLogoAsync(int id)
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
            var result = await _officeService.DeleteLogoAsync(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Success);
        }

        [AllowAnonymous]
        [HttpGet("Owners/{officeId}")]
        public IActionResult GetAllOfficeOwnersByOfficeId(int officeId)
        {
            
            var result = _officeOwnerService.GetAllOfficeOwnersByOfficeId(officeId);
            if (result == null)
            {
                return BadRequest(new { message = "No Owner Found!!!" });
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("Document/{officeid}")]
        public IActionResult GetAllDocumentsByOfficeId(int officeid)
        {
            var result = _officeDocumentService.GetAllDocumentsByOfficeId(officeid);
            if (result == null)
            {
                return BadRequest(new { message = "No Office Found!!!" });
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("Document/View/{documentid}")]
        public IActionResult ViewDocumentByUrl(int documentid)
        {
           
            var result = _officeDocumentService.GetDocument(documentid);
            if (result.Bytes == null)
            {
                return BadRequest(new { message = "Bad Request" });
            }
            return File(result.Bytes, result.ContentType, result.FileName);

        }

        [AllowAnonymous]
        [HttpGet("Form/View/{typeid}")]
        public IActionResult ViewFormByUrl(int typeid)
        {
            
            var result = _officeDocumentService.GetForm(typeid);
            if (result.Bytes == null)
            {
                return BadRequest(new { message = "Bad Request" });
            }
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpPost("Document")]
        public async Task<IActionResult> PostOfficeDocumentAsync([FromForm] OfficeFileModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });
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
            var result = await _officeDocumentService.PostOfficeDocumentAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpDelete("Document/{documentid}")]
        public async Task<IActionResult> DeleteDocumentAsync(int documentid)
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
            var result = await _officeDocumentService.DeleteDocumentAsync(documentid);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Success);
        }

        //admin
        [HttpPost("filter")]
        public async Task<IActionResult> GetOfficesForAdmin(FilterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, adminroles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }

            var result = _officeService.GetOfficesForAdmin(model);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message});
            }
            return Ok(result.Result);
        }

        //admin
        [HttpPost("filter-export")]
        public async Task<IActionResult> ExportOfficesForAdmin(FilterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, adminroles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _officeService.ExportOfficesForAdmin(model);
            FileBytesModel excelfile = new FileBytesModel();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var stream = new MemoryStream();
            var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells.LoadFromCollection(result, true);
            workSheet.Column(7).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss AM/PM";
            workSheet.Column(8).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss AM/PM";
            workSheet.Column(9).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss AM/PM";
            workSheet.Column(10).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss AM/PM";
            package.Save();
            excelfile.Bytes = stream.ToArray();
            stream.Position = 0;
            stream.Close();
            string excelName = $"Data-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            excelfile.FileName = excelName;
            excelfile.ContentType = contentType;
            return File(excelfile.Bytes, excelfile.ContentType, excelfile.FileName);
        }

        //admin
        [HttpPost("payments/filter")]
        public async Task<IActionResult> GetOfficesPaymentsReportForAdmin(FilterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });
            adminroles.Append("BillingManager");

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, adminroles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var result = _officeService.GetOfficesPaymentsReportForAdmin(model);
            if (result.Success == false)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        //admin
        [HttpPost("payments/filter-export")]
        public async Task<IActionResult> ExportOfficesPaymentsReportForAdmin(FilterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Model" });
            adminroles.Append("BillingManager");

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var isAuthorized = await _userService.IsAuthorized(principal, adminroles);
            if (isAuthorized == false)
            {
                return Unauthorized(new
                {
                    Message = "Unauthorized",
                    MessageEnglish = "Unauthorized",
                    MessageArabic = "غير مصرح",
                });
            }
            var data = _officeService.ExportOfficesPaymentsReportForAdmin(model);
            FileBytesModel excelfile = new FileBytesModel();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var stream = new MemoryStream();
            var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells.LoadFromCollection(data, true);
            workSheet.Column(7).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss AM/PM";
            package.Save();
            excelfile.Bytes = stream.ToArray();
            stream.Position = 0;
            stream.Close();
            string excelName = $"Data-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            excelfile.FileName = excelName;
            excelfile.ContentType = contentType;
            return File(excelfile.Bytes, excelfile.ContentType, excelfile.FileName);
        }
    }
}
