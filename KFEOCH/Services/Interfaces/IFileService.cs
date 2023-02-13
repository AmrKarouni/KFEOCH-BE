using KFEOCH.Models;
using KFEOCH.Models.Site;
using KFEOCH.Models.Views;
using Microsoft.AspNetCore.Mvc;

namespace KFEOCH.Services.Interfaces
{
    public interface IFileService
    {
        Task<ResultWithMessage> UploadFile(FileModel model,string path);
        Task<ResultWithMessage> UploadPdfFile(FileModel model, string path);
        Task<ResultWithMessage> UploadFile(OwnerFileModel model, string path);
        Task<ResultWithMessage> UploadFile(OfficeFileModel model, string path);
        Task<ResultWithMessage> UploadPageImage(ImageModel model, string path);
        Task<ResultWithMessage> UploadImage(IFormFile image, string path);
        FilePathModel GetFilePath(string fileurl);
        Task<ResultWithMessage> DeleteFile(string fileurl);
        FileBytesModel ExportToExcel(IEnumerable<object> model);
    }
}
