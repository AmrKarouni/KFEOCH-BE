using DJH.KnetPipe;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace KFEOCH.Services
{
    public class KnetPaymentService : IKnetPaymentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KnetPaymentService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string  GenerateRequestPayment (int officeId,
                                       double amount,
                                       int typeId,
                                       int entityId,
                                       string lang,
                                       string? returnUrl)
        {
            var account = new AccountConfig("278101", "Jtp8QnwB", "W7X5A95YG9C30B1K");
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var respUrl = hostpath + "/api/Payment/request-success";
            var respUrlError = hostpath + "/api/Payment/Error";
            var paymentRequest = new PaymentRequest(officeId.ToString(), respUrl, respUrlError, Convert.ToDecimal(amount),
                udf1: typeId.ToString(),
                udf2: entityId.ToString(),
                udf3: lang,
                udf4: returnUrl,
                udf5: "Request",
                environment: DJH.KnetPipe.Environment.Live);
            var benefit = new Payment(account, paymentRequest);
            var respo = benefit.Generate();
            if (respo.Success)
            {
                return respo.RedirectLink ;
            }
            else
            {
                return "Failed ";
            }

        }

        public string GenerateRenewPayment(int officeId,
                                       double amount,
                                       double renewyears,
                                       double missedyears,
                                       string lang,
                                       string? returnUrl)
        {
            var account = new AccountConfig("278101", "Jtp8QnwB", "W7X5A95YG9C30B1K");
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var respUrl = hostpath + "/api/Payment/renew-success";
            var respUrlError = hostpath + "/api/Payment/Error";
            var paymentRequest = new PaymentRequest(officeId.ToString(), respUrl, respUrlError, Convert.ToDecimal(amount),
                udf1: renewyears.ToString(),
                udf2: missedyears.ToString(),
                udf3: lang,
                udf4: returnUrl,
                udf5: "Renew",
                environment: DJH.KnetPipe.Environment.Live);
            var benefit = new Payment(account, paymentRequest);
            var respo = benefit.Generate();
            if (respo.Success)
            {
                return respo.RedirectLink;
            }
            else
            {
                return "Failed ";
            }

        }
    }
}
