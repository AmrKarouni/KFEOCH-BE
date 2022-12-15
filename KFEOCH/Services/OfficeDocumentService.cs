using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KFEOCH.Services
{
    public class OfficeDocumentService : IOfficeDocumentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;

        public OfficeDocumentService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
                                    IFileService fileService, IConfiguration configuration)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
            _configuration = configuration;
        }

        public OfficeWithDocuments GetAllDocumentsByOfficeId(int officeid)
        {
            var office = _db.Offices?.Include(x => x.Documents).FirstOrDefault(x => x.Id == officeid);
            if (office == null)
            {
                return new OfficeWithDocuments();
            }
            var types = _db.OfficeDocumentTypes?.ToList().Select(x => new OfficeDocumentTypeView
            {
                Id = x.Id,
                NameArabic = x.NameArabic,
                NameEnglish = x.NameEnglish,
                Files = office.Documents.Where(d => (d.TypeId ?? 0) == x.Id && d.IsActive == true).Select(d => new OfficeDocumentView
                {
                    Id = d.Id,
                    DocumentUrl = d.DocumentUrl,
                    AddedDate = d.AddedDate
                }).ToList()
            });
            var result = new OfficeWithDocuments()
            {
                Id = office.Id,
                NameArabic = office.NameArabic,
                NameEnglish = office.NameEnglish,
                Documents = types
            };
            return result ?? new OfficeWithDocuments();
        }
        public async Task<ResultWithMessage> PostOfficeDocumentAsync(OfficeFileModel model)
        {

            var office = _db.Offices?.Find(model.OfficeId);
            if (office == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Not Found !!!" };
            }
            var type = _db.OfficeDocumentTypes?.Find(model.TypeId);
            if (type == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Document Type Not Found !!!" };
            }
            var olddocument = _db.OfficeDocuments.Where(x => x.OfficeId == model.OfficeId && x.TypeId == model.TypeId && x.IsActive == true).ToList();
            foreach (var doc in olddocument)
            {
                doc.IsActive = false;
                _db.Entry(doc).State = EntityState.Modified;
            }
            var uploadResult = await _fileService.UploadFile(model, "offices");
            if (!uploadResult.Success)
            {
                return new ResultWithMessage { Success = false, Message = $@"Upload Document Failed !!!" };
            }
            var documentUrl = uploadResult.Message;
            var document = new OfficeDocument()
            {
                OfficeId = model.OfficeId,
                TypeId = model.TypeId,
                AddedDate = DateTime.UtcNow,
                IsActive = true,
                IsApproved = true,
                IsDeleted = false,
                DocumentUrl = documentUrl,
            };
            await _db.OfficeDocuments.AddAsync(document);
            _db.SaveChanges();
            var result = new OfficeDocumentView
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
            var doc = _db.OfficeDocuments?.Find(documentid);
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

        public async Task<ResultWithMessage> DeleteDocumentAsync(int documentid)
        {
            var doc = _db.OfficeDocuments?.Find(documentid);
            if (doc == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Office Document Not Found !!!" };
            }
            var deletedfile = await _fileService.DeleteFile(doc.DocumentUrl);
            _db.OfficeDocuments.Remove(doc);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = "Office Document Deleted !!!" };
        }
    }
}
