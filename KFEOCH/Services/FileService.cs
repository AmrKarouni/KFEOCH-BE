using KFEOCH.Models;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace KFEOCH.Services
{
    public class FileService : IFileService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public FileService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public async Task<ResultWithMessage> UploadFile(FileModel model, string path)
        {
            int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB
            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".pdf" };
            var fileHostServer = _configuration.GetValue<string>("FileHostServer");
            if (model.File == null)
            {
                return new ResultWithMessage { Success = false, Message = "No File Found !!" };
            }
            var extension = model.File.FileName.Substring(model.File.FileName.LastIndexOf('.')).ToLower();
            if (!AllowedFileExtensions.Contains(extension))
            {
                return new ResultWithMessage { Success = false, Message = "Allowed Extensions are .jpg, .jpeg, .png , .pdf" };
            }
            if (model.File.Length > MaxContentLength)
            {
                return new ResultWithMessage { Success = false, Message = "Max Size Allowed is 5 M.B" };
            }
            var filePath = Path.Combine(path + "/" + model.FileName + extension);
            var fullfilePath = Path.Combine(fileHostServer + "/", filePath);
            string directory = Path.GetDirectoryName(fullfilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            FileStream stream = new FileStream(fullfilePath, FileMode.Create);
            await model.File.CopyToAsync(stream);
            stream.Close();
            return new ResultWithMessage { Success = true, Message = "/" + filePath };
        }

        public async Task<ResultWithMessage> UploadPdfFile(FileModel model, string path)
        {
            int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB
            IList<string> AllowedFileExtensions = new List<string> { ".pdf" };
            var fileHostServer = _configuration.GetValue<string>("FileHostServer");
            if (model.File == null)
            {
                return new ResultWithMessage { Success = false, Message = "No File Found !!" };
            }
            var extension = model.File.FileName.Substring(model.File.FileName.LastIndexOf('.')).ToLower();
            if (!AllowedFileExtensions.Contains(extension))
            {
                return new ResultWithMessage { Success = false, Message = "Allowed Extension is .pdf" };
            }
            if (model.File.Length > MaxContentLength)
            {
                return new ResultWithMessage { Success = false, Message = "Max Size Allowed is 5 M.B" };
            }
            var filePath = Path.Combine(path + "/" + model.FileName + extension);
            var fullfilePath = Path.Combine(fileHostServer + "/", filePath);
            string directory = Path.GetDirectoryName(fullfilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            FileStream stream = new FileStream(fullfilePath, FileMode.Create);
            await model.File.CopyToAsync(stream);
            stream.Close();
            return new ResultWithMessage { Success = true, Message = "/" + filePath };
        }

        public async Task<ResultWithMessage> UploadFile(OwnerFileModel model, string path)
        {
            int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB
            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".pdf" };
            var fileHostServer = _configuration.GetValue<string>("FileHostServer");
            if (model.File == null)
            {
                return new ResultWithMessage { Success = false, Message = "No File Found !!" };
            }
            var extension = model.File.FileName.Substring(model.File.FileName.LastIndexOf('.')).ToLower();
            if (!AllowedFileExtensions.Contains(extension))
            {
                return new ResultWithMessage { Success = false, Message = "Allowed Extensions are .jpg, .jpeg, .png , .pdf" };
            }
            if (model.File.Length > MaxContentLength)
            {
                return new ResultWithMessage { Success = false, Message = "Max Size Allowed is 5 M.B" };
            }
            var filename = DateTime.UtcNow.Year + ""
                          + DateTime.UtcNow.Month + ""
                          + DateTime.UtcNow.Day + ""
                          + DateTime.UtcNow.Hour + ""
                          + DateTime.UtcNow.Minute + ""
                          + DateTime.UtcNow.Second;
            var filePath = Path.Combine(path + "/" + model.OwnerId +
                "/" + model.TypeId +
                "/" + filename +
                extension);
            var fullfilePath = Path.Combine(fileHostServer + "/", filePath);
            string directory = Path.GetDirectoryName(fullfilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            FileStream stream = new FileStream(fullfilePath, FileMode.Create);
            await model.File.CopyToAsync(stream);
            stream.Close();
            return new ResultWithMessage { Success = true, Message = "/" + filePath };
        }

        public async Task<ResultWithMessage> UploadFile(OfficeFileModel model, string path)
        {
            int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB
            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".pdf" };
            var fileHostServer = _configuration.GetValue<string>("FileHostServer");
            if (model.File == null)
            {
                return new ResultWithMessage { Success = false, Message = "No File Found !!" };
            }
            var extension = model.File.FileName.Substring(model.File.FileName.LastIndexOf('.')).ToLower();
            if (!AllowedFileExtensions.Contains(extension))
            {
                return new ResultWithMessage { Success = false, Message = "Allowed Extensions are .jpg, .jpeg, .png , .pdf" };
            }
            if (model.File.Length > MaxContentLength)
            {
                return new ResultWithMessage { Success = false, Message = "Max Size Allowed is 5 M.B" };
            }
            var filename = DateTime.UtcNow.Year + ""
                          + DateTime.UtcNow.Month + ""
                          + DateTime.UtcNow.Day + ""
                          + DateTime.UtcNow.Hour + ""
                          + DateTime.UtcNow.Minute + ""
                          + DateTime.UtcNow.Second;
            var filePath = Path.Combine(path + "/" + model.OfficeId +
                "/" + model.TypeId +
                "/" + filename +
                extension);
            var fullfilePath = Path.Combine(fileHostServer + "/", filePath);
            string directory = Path.GetDirectoryName(fullfilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            FileStream stream = new FileStream(fullfilePath, FileMode.Create);
            await model.File.CopyToAsync(stream);
            stream.Close();
            return new ResultWithMessage { Success = true, Message = "/" + filePath };
        }

        public FilePathModel GetFilePath(string fileurl)
        {
            var path = new FilePathModel();
            var ContentList = new List<ContentType>();
            ContentList.Add(new ContentType { Extension = "jpg", cType = "image/jpeg" });
            ContentList.Add(new ContentType { Extension = "jpeg", cType = "image/jpeg" });
            ContentList.Add(new ContentType { Extension = "png", cType = "image/png" });
            ContentList.Add(new ContentType { Extension = "pdf", cType = "application/pdf" });
            ////'Images'
            //ContentList.Add(new ContentType { Extension = "bmp", cType = "image/bmp" });
            //ContentList.Add(new ContentType { Extension = "gif", cType = "image/gif" });
            //ContentList.Add(new ContentType { Extension = "jpeg", cType = "image/jpeg" });
            //ContentList.Add(new ContentType { Extension = "jpg", cType = "image/jpeg" });
            //ContentList.Add(new ContentType { Extension = "png", cType = "image/png" });
            //ContentList.Add(new ContentType { Extension = "tif", cType = "image/tiff" });
            //ContentList.Add(new ContentType { Extension = "tiff", cType = "image/tiff" });
            ////'Documents'
            //ContentList.Add(new ContentType { Extension = "doc", cType = "application/msword" });
            //ContentList.Add(new ContentType { Extension = "docx", cType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document" });
            //ContentList.Add(new ContentType { Extension = "pdf", cType = "application/pdf" });
            ////'Slideshows'
            //ContentList.Add(new ContentType { Extension = "ppt", cType = "application/vnd.ms-powerpoint" });
            //ContentList.Add(new ContentType { Extension = "pptx", cType = "application/vnd.openxmlformats-officedocument.presentationml.presentation" });
            //// 'Data'
            //ContentList.Add(new ContentType { Extension = "xlsx", cType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            //ContentList.Add(new ContentType { Extension = "xls", cType = "application/vnd.ms-excel" });
            //ContentList.Add(new ContentType { Extension = "csv", cType = "text/csv" });
            //ContentList.Add(new ContentType { Extension = "xml", cType = "text/xml" });
            //ContentList.Add(new ContentType { Extension = "txt", cType = "text/plain" });
            ////'Compressed Folders'
            //ContentList.Add(new ContentType { Extension = "zip", cType = "application/zip" });
            //// 'Audio'
            //ContentList.Add(new ContentType { Extension = "ogg", cType = "application/ogg" });
            //ContentList.Add(new ContentType { Extension = "mp3", cType = "audio/mpeg" });
            //ContentList.Add(new ContentType { Extension = "wma", cType = "audio/x-ms-wma" });
            //ContentList.Add(new ContentType { Extension = "wav", cType = "audio/x-wav" });
            ////'Video'
            //ContentList.Add(new ContentType { Extension = "wmv", cType = "audio/x-ms-wmv" });
            //ContentList.Add(new ContentType { Extension = "swf", cType = "application/x-shockwave-flash" });
            //ContentList.Add(new ContentType { Extension = "avi", cType = "video/avi" });
            //ContentList.Add(new ContentType { Extension = "mp4", cType = "video/mp4" });
            //ContentList.Add(new ContentType { Extension = "mpeg", cType = "video/mpeg" });
            //ContentList.Add(new ContentType { Extension = "mpg", cType = "video/mpeg" });
            //ContentList.Add(new ContentType { Extension = "qt", cType = "video/quicktime" });
            var extension = fileurl.Substring(fileurl.LastIndexOf('.') + 1).ToLower();
            path.Path = fileurl;
            path.ContentType = ContentList.FirstOrDefault(x => x.Extension == extension)?.cType;
            return path;
        }

        public async Task<ResultWithMessage> DeleteFile(string fileurl)
        {
            var fileHostServer = _configuration.GetValue<string>("FileHostServer");
            var fullfilePath = Path.Combine(fileHostServer + "/" + fileurl);
            if (File.Exists(fullfilePath))
            {
                File.Delete(fullfilePath);
                return new ResultWithMessage { Success = true, Message = $@"File {fileurl} Deleted !!!" };
            }
            return new ResultWithMessage { Success = false, Message = $@"File {fileurl} Not Found !!!" };
        }
    }
}