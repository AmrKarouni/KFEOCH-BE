using KFEOCH.Models;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KFEOCH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private readonly IOfficeService _officeService;
        private readonly IOfficeOwnerService _officeOwnerService;
        private readonly IOfficeDocumentService _officeDocumentService;

        public OfficeController(IOfficeService officeService, IOfficeOwnerService officeOwnerService, IOfficeDocumentService officeDocumentService)
        {
            _officeService = officeService;
            _officeOwnerService = officeOwnerService;
            _officeDocumentService = officeDocumentService;
        }

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
        public IActionResult PutOffice(int id, Office model)
        {
            var result = _officeService.PutOfficeAsync(id, model);
            if (!result.Result.Success)
            {
                return BadRequest(new
                {
                    message = result.Result.Message
                });
            }
            return Ok(result.Result.Result);
        }

        [HttpPost("upload-logo")]
        public async Task<IActionResult> UploadLogo([FromForm] FileModel model)
        {
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
            var result = await _officeService.DeleteLogoAsync(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Success);
        }

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
        [HttpPost("Document")]
        public async Task<ActionResult> PostOfficeDocumentAsync([FromForm] OfficeFileModel model)
        {
            var result = await _officeDocumentService.PostOfficeDocumentAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpDelete("Document/{documentid}")]
        public async Task<ActionResult> DeleteDocumentAsync(int documentid)
        {
            var result = await _officeDocumentService.DeleteDocumentAsync(documentid);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Success);
        }
    }
}
