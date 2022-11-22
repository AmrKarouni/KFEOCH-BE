using KFEOCH.Models;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IFileService
    {
        Task<ResultWithMessage> UploadFile(FileModel model,string path);
    }
}
