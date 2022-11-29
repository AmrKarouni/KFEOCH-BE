﻿using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Binding;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KFEOCH.Services
{
    public class OwnerDocumentService : IOwnerDocumentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;

        public OwnerDocumentService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, IFileService fileService)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
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
            var type = _db.OwnerDocumentTypes?.Find(model.TypeId);
            if (type == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Document Type Not Found !!!" };
            }
            var olddocument = _db.OwnerDocuments.FirstOrDefault(x => x.OwnerId == model.OwnerId && x.TypeId == model.TypeId);
            if (olddocument != null)
            {
                olddocument.IsActive = false;
                _db.Entry(olddocument).State = EntityState.Modified;
            }
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var uploadResult = await _fileService.UploadFile(model, "owners");
            if (!uploadResult.Success)
            {
                return new ResultWithMessage { Success = false, Message = $@"Upload Document Failed !!!" };
            }
            var documentUrl = hostpath + uploadResult.Message ;
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
            return new ResultWithMessage { Success = true, Result = document };
        }

        public FilePathModel GetDocumentUrl(int documentid)
        {
            var result= new FilePathModel();
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

    }
}
