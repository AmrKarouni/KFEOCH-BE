using KFEOCH.Models;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IOfficeRequestService
    {
        Task<ResultWithMessage> InitialOfficeRequestAsync(OfficeRequestBindingModel model);
        Task<ResultWithMessage> GetRequestById(int id);
        ResultWithMessage GetAllPendingRequests();
        Task<ResultWithMessage> GetAllRequestsByOfficeId(int officeid);
        ResultWithMessage PutRequest(int id, OfficeRequest model);
        Task<ResultWithMessage> GenerateRequestReceipt(int id);
        Task<ResultWithMessage> GetAllOfficeRequestPayments(int officeid);
        Task<ResultWithMessage> GetRequestReceipt(int id);
        Task<ResultWithMessage> GetPaymentReceipt(int id);
        Task<ResultWithMessage> GenerateRequestCertificate(int id);
        Task<ResultWithMessage> GetRequestCertificate(int id);
    }
}
