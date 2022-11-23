using KFEOCH.Models;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;

namespace KFEOCH.Services
{
    public class FileService : IFileService
    {

        public FileService()
        {
        }
        public async Task<ResultWithMessage> UploadFile(FileModel model, string path)
        {
            int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB
            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".png", ".pdf" };
            if (model.File == null)
            {
                return new ResultWithMessage { Success = false, Message = "No File Found !!" };
            }
            var extension = model.File.FileName.Substring(model.File.FileName.LastIndexOf('.')).ToLower();
            if (!AllowedFileExtensions.Contains(extension))
            {
                return new ResultWithMessage { Success = false, Message = "Allowed Extensions are .jpg, .png , .pdf" };
            }
            if (model.File.Length > MaxContentLength)
            {
                return new ResultWithMessage { Success = false, Message = "Max Size Allowed is 5 M.B" };
            }
            var filePath = Path.Combine(path + "/" + model.FileName + extension);
            var fullfilePath = Path.Combine(@"./App_Media/", filePath);
            string directory = Path.GetDirectoryName(fullfilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            FileStream FS = new FileStream(fullfilePath, FileMode.Create);
            await model.File.CopyToAsync(FS);
            FS.Close();
            return new ResultWithMessage { Success = true, Message = "/" + filePath };
        }
    }
}