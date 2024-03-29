﻿using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace KFEOCH.Services
{
    public class OwnerDocumentService : IOwnerDocumentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public OwnerDocumentService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
                                    IFileService fileService, IConfiguration configuration,IUserService userService)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
            _configuration = configuration;
            _userService = userService; 
        }

        public OfficeOwnerWithDocuments GetAllDocumentsByOwnerId(int ownerid)
        {

            var officeOwner = _db.OfficeOwners?.Include(x => x.Documents).FirstOrDefault(x => x.Id == ownerid);
            if (officeOwner == null)
            {
                return new OfficeOwnerWithDocuments();
            }
            var types = _db.OwnerDocumentTypes.ToList().Select(x => new OfficeOwnerDocumentTypeView
            {
                Id = x.Id,
                NameArabic = x.NameArabic,
                NameEnglish = x.NameEnglish,
                HasForm = x.HasForm,
                FormUrl = x.FormUrl,
                Files = officeOwner.Documents.Where(d => (d.TypeId ?? 0) == x.Id && d.IsActive == true).Select(d => new OfficeOwnerDocumentView
                {
                    Id = d.Id,
                    DocumentUrl = d.DocumentUrl,
                    AddedDate = d.AddedDate
                }).ToList()
            });
            var result = new OfficeOwnerWithDocuments()
            {
                Id = officeOwner.Id,
                NameArabic = officeOwner.NameArabic,
                NameEnglish = officeOwner.NameEnglish,
                Documents = types
            };
            return result ?? new OfficeOwnerWithDocuments();


        }
        public async Task<ResultWithMessage> PostOwnerDocumentAsync(OwnerFileModel model)
        {
            
            var owner = _db.OfficeOwners?.Find(model.OwnerId);
            if (owner == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Owner Not Found !!!" };
            }

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, owner.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }

            var type = _db.OwnerDocumentTypes?.Find(model.TypeId);
            if (type == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Document Type Not Found !!!" };
            }
            var olddocument = _db.OwnerDocuments.Where(x => x.OwnerId == model.OwnerId && x.TypeId == model.TypeId && x.IsActive == true).ToList();

            foreach (var doc in olddocument)
            {
                doc.IsActive = false;
                _db.Entry(doc).State = EntityState.Modified;
            }

            var uploadResult = await _fileService.UploadFile(model, "owners");
            if (!uploadResult.Success)
            {
                return new ResultWithMessage { Success = false, Message = $@"Upload Document Failed !!!" };
            }
            var documentUrl = uploadResult.Message;
            var document = new OwnerDocument()
            {
                OwnerId = model.OwnerId,
                TypeId = model.TypeId,
                AddedDate = DateTime.UtcNow,
                IsActive = true,
                IsApproved = true,
                IsDeleted = false,
                DocumentUrl = documentUrl,
            };
            await _db.OwnerDocuments.AddAsync(document);
            _db.SaveChanges();
            var result = new OfficeOwnerDocumentView
            {
                Id = document.Id,
                Name = document.DocumentUrl.Substring(document.DocumentUrl.LastIndexOf('/') + 1).ToLower(),
                DocumentUrl = document.DocumentUrl,
                AddedDate = document.AddedDate
            };
            return new ResultWithMessage { Success = true, Result = result };
        }

        public FilePathModel GetDocumentUrl(int documentid)
        {
            var result = new FilePathModel();
            string docurl;
            var doc = _db.OwnerDocuments?.Find(documentid);
            if (doc == null)
            {
                return result;
            }
            
            docurl = doc.DocumentUrl;
            if (docurl != null)
            {
                result = _fileService.GetFilePath(docurl);
            }
            return result;
        }

        public FileBytesModel GetDocument(int documentid)
        {
            FileBytesModel result = new FileBytesModel();
            try
            {
                var url = GetDocumentUrl(documentid);
                var host = _configuration.GetValue<string>("FileHostServer");

                if (url == null || url.Path == null)
                {
                    return result;
                }
                result.Bytes = File.ReadAllBytes(host + url.Path);
                result.FileName = url.Path.Substring(url.Path.LastIndexOf('/') + 1).ToLower();
                result.ContentType = url.ContentType;
                return result;
            }
            catch (Exception e)
            {
                return result = new FileBytesModel();
                throw;
            }
        }

        public FileBytesModel GetForm(int typeid)
        {
            FileBytesModel result = new FileBytesModel();
            try
            {
                var doctype = _db.OwnerDocumentTypes.FirstOrDefault(x => x.Id == typeid);

                if (doctype == null || doctype.HasForm == false)
                {
                    return result;
                }
                var url = _fileService.GetFilePath(doctype.FormUrl);
                if (url == null || url.Path == null)
                {
                    return result;
                }
                var host = _configuration.GetValue<string>("FileHostServer");
                result.Bytes = File.ReadAllBytes(host + url.Path);
                result.FileName = url.Path.Substring(url.Path.LastIndexOf('/') + 1).ToLower();
                result.ContentType = url.ContentType;
                return result;
            }
            catch (Exception e)
            {
                return result = new FileBytesModel();
                throw;
            }
        }

        public async Task<ResultWithMessage> DeleteDocumentAsync(int documentid)
        {
            var doc = _db.OwnerDocuments?.Find(documentid);
            if (doc == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Owner Document Not Found !!!" };
            }
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, doc.Owner.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
            var deletedfile = await _fileService.DeleteFile(doc.DocumentUrl);
            _db.OwnerDocuments.Remove(doc);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = "Owner Document Deleted !!!" };
        }
    }
}
