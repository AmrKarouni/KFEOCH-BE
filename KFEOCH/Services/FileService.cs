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
        public ResultWithMessage UploadFile(FileModel model)
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
            var filePath = Path.Combine(@"../App_Media/", model.FileName + extension);
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            FileStream FS = new FileStream(filePath, FileMode.Create);
            model.File.CopyToAsync(FS);
            FS.Close();
            return new ResultWithMessage { Success = true, Message = filePath };
        }
    }
}
