﻿using KFEOCH.Models;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IOfficeDocumentService
    {
        OfficeWithDocuments GetAllDocumentsByOfficeId(int officeid);
        Task<ResultWithMessage> PostOfficeDocumentAsync(OfficeFileModel model);
        FilePathModel GetDocumentUrl(int documentid);
        FileBytesModel GetDocument(int documentid);
        FileBytesModel GetForm(int typeid);
        Task<ResultWithMessage> DeleteDocumentAsync(int documentid);
    }
}
