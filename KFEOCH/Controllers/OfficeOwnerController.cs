using KFEOCH.Models;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace KFEOCH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeOwnerController : ControllerBase
    {
        private readonly IOfficeOwnerService _officeOwnerService;
        private readonly IOwnerDocumentService _ownerDocumentService;

        public OfficeOwnerController(IOfficeOwnerService officeOwnerService, IOwnerDocumentService ownerDocumentService)
        {
            _officeOwnerService = officeOwnerService;
            _ownerDocumentService = ownerDocumentService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _officeOwnerService.GetById(id);
            if (result.Success == false)
            {
                return BadRequest(new { message = "No Owner Found!!!" });
            }
            return Ok(result.Result);
        }

        [HttpPost]
        public async Task<ActionResult> PostOfficeOwnerAsync(OfficeOwner model)
        {
            var result = await _officeOwnerService.PostOfficeOwnerAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }
        [HttpPut("{id}")]
        public IActionResult PutOfficeOwnerAsync(int id, OfficeOwner model)
        {
            var result = _officeOwnerService.PutOfficeOwnerAsync(id, model);
            if (!result.Result.Success)
            {
                return BadRequest(new
                {
                    message = result.Result.Message
                });
            }
            return Ok(result.Result.Result);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteOfficeOwnerAsync(int id)
        {
            var result = _officeOwnerService.DeleteOfficeOwnerAsync(id);
            if (!result.Result.Success)
            {
                return BadRequest(new
                {
                    message = result.Result.Message
                });
            }
            return Ok(result.Result.Result);
        }

        [HttpGet("Document/{ownerid}")]
        public IActionResult GetAllDocumentsByOwnerId(int ownerid)
        {
            var result = _ownerDocumentService.GetAllDocumentsByOwnerId(ownerid);
            if (result == null)
            {
                return BadRequest(new { message = "No Owner Found!!!" });
            }
            return Ok(result);
        }

        [HttpGet("Document/View/{documentid}")]
        public IActionResult ViewDocumentByUrl(int documentid)
        {
            var result = _ownerDocumentService.GetDocument(documentid);
            if (result.Bytes == null)
            {
                return BadRequest(new { message = "Bad Request" });
            }
            return File(result.Bytes, result.ContentType,result.FileName);

        }
        [HttpPost("Document")]
        public async Task<ActionResult> PostOwnerDocumentAsync([FromForm] OwnerFileModel model)
        {
            var result = await _ownerDocumentService.PostOwnerDocumentAsync(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Result);
        }

        [HttpDelete("Document/{documentid}")]
        public async Task<ActionResult> DeleteDocumentAsync(int documentid)
        {
            var result = await _ownerDocumentService.DeleteDocumentAsync(documentid);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(result.Success);
        }
    }
}
