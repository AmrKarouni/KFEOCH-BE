using KFEOCH.Models;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IFileService
    {
        ResultWithMessage UploadFile(FileModel model,string path);
    }
}
