using KFEOCH.Models;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Security.Claims;

namespace KFEOCH.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string[] roles;
        public ReportController(IReportService reportService,
                                IUserService userService,
                                IHttpContextAccessor httpContextAccessor)
        {
            _reportService = reportService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            roles = new string[] { "SuperUser", "Administrator", "ReportManager" };
        }

        [HttpGet("offices")]
        public async Task<IActionResult> ExportOfficesForAdmin()
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
            var result = _reportService.ExportOfficesForAdmin();
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

        [HttpGet("owners")]
        public async Task<IActionResult> ExportOfficeOwnersForAdmin()
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
            var result = _reportService.ExportOfficeOwnersForAdmin();
            FileBytesModel excelfile = new FileBytesModel();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var stream = new MemoryStream();
            var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells.LoadFromCollection(result, true);
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

        [HttpGet("specialities")]
        public async Task<IActionResult> ExportOfficeSpecialitiesForAdmin()
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
            var result = _reportService.ExportOfficeSpecialitiesForAdmin();
            FileBytesModel excelfile = new FileBytesModel();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var stream = new MemoryStream();
            var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells.LoadFromCollection(result, true);
            workSheet.Column(8).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss AM/PM";
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

        [HttpGet("contacts")]
        public async Task<IActionResult> ExportOfficeContactsForAdmin()
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
            var result = _reportService.ExportOfficeContactsForAdmin();
            FileBytesModel excelfile = new FileBytesModel();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var stream = new MemoryStream();
            var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells.LoadFromCollection(result, true);
            workSheet.Column(9).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss AM/PM";
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

        [HttpGet("licenses")]
        public async Task<IActionResult> ExportOfficeLicensesForAdmin()
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
            var result = _reportService.ExportOfficeLicensesForAdmin();
            FileBytesModel excelfile = new FileBytesModel();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var stream = new MemoryStream();
            var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells.LoadFromCollection(result, true);
            workSheet.Column(9).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss AM/PM";
            workSheet.Column(10).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss AM/PM";
            workSheet.Column(11).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss AM/PM";
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
