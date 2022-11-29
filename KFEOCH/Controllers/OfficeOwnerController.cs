﻿using KFEOCH.Models;
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
            if (result == null)
            {
                return BadRequest(new { message = "No Owner Found!!!" });
            }
            return Ok(result);
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
            var url = _ownerDocumentService.GetDocumentUrl(documentid);
            if (url == null || url.Path == null)
            {
                return BadRequest(new { message = "No File Found!!!" });
            }
            //var fs = new FileStream(url.Path, FileMode.Open);
            //var file = File(fs, url.ContentType);
            var file = PhysicalFile(url.Path, url.ContentType);
            return Ok(file);
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
    }
}
