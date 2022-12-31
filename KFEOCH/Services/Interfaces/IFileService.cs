using KFEOCH.Models;
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
        FilePathModel GetFilePath(string fileurl);
        Task<ResultWithMessage> DeleteFile(string fileurl);
    }
}
