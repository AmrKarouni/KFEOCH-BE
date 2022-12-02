using KFEOCH.Models;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IOfficeService
    {
        Office GetById(int id);
        Task<ResultWithMessage> PutOfficeAsync(int id, Office model);
        Task<ResultWithMessage> UploadLogo(FileModel model);
        Task<ResultWithMessage> DeleteLogoAsync(int id);
    }
}
