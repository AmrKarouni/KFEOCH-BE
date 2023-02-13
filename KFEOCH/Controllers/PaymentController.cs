using DJH.KnetPipe;
using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Web;

namespace KFEOCH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IOfficeRequestService _requestService;
        private readonly IOfficeRegistrationService _registrationService;
        private readonly ApplicationDbContext _db;

        public PaymentController(IOfficeRequestService requestService,
                                 IOfficeRegistrationService registrationService,
                                 ApplicationDbContext db)
        {
            _requestService = requestService;
            _registrationService = registrationService;
            _db = db;
        }



        [HttpPost("request-success")]
        public async Task<IActionResult> RequestSuccess()
        {

            var account = new AccountConfig("278101", "Jtp8QnwB", "W7X5A95YG9C30B1K");
            var payment = new Payment(account);
            var html = "";
            var form = HttpContext.Request.Form["trandata"].ToString();
            string returndata = payment.Decrypt(form).ToString();
            var returnlist = returndata.Split('&').ToList();
            var amount = returnlist.FirstOrDefault(x => x.Contains("amt")).Split('=')[1].ToString();
            var officeIdString = returnlist.FirstOrDefault(x => x.Contains("trackid")).Split('=')[1].ToString();
            var officeSuccess = int.TryParse(officeIdString, out int officeId);
            var result = returnlist.FirstOrDefault(x => x.Contains("result")).Split('=')[1].ToString().ToLower();
            //var pt = returnlist.FirstOrDefault(x => x.Contains("udf5")).Split('=')[1].ToString();
            var lang = returnlist.FirstOrDefault(x => x.Contains("udf3")).Split('=')[1].ToString();
            var returnUrl = returnlist.FirstOrDefault(x => x.Contains("udf4")).Split('=')[1].ToString();
            returnUrl = returnUrl.Replace("_", ":").Replace(",", "/");
            var paymentId = returnlist.FirstOrDefault(x => x.Contains("paymentid")).Split('=')[1].ToString();
            var referenceId = returnlist.FirstOrDefault(x => x.Contains("ref")).Split('=')[1].ToString(); // long number ex: 100202108105521282
            var postDate = returnlist.FirstOrDefault(x => x.Contains("postdate")).Split('=')[1].ToString();
            var office = _db.Offices.Find(officeId);
            var typeIdString = returnlist.FirstOrDefault(x => x.Contains("udf1")).Split('=')[1].ToString();
            var typeIdSuccess = int.TryParse(typeIdString, out int typeId);
            var entityIdString = returnlist.FirstOrDefault(x => x.Contains("udf2")).Split('=')[1].ToString();
            var entityIdSuccess = int.TryParse(entityIdString, out int entityId);

            if (string.IsNullOrEmpty(referenceId) ||
               !(officeSuccess && typeIdSuccess && entityIdSuccess))
            {
                var htmlerror = GenerateFailedHtml(lang);
                return new ContentResult
                {
                    Content = htmlerror,
                    ContentType = "text/html"
                };
            }

            var type = _db.RequestTypes.Find(typeId);
            var entity = _db.CertificateEntities.Find(entityId);

            var req = new OfficeRequest(new OfficeRequestBindingModel
                                                { OfficeId = officeId,
                                                  RequestTypeId = typeId,
                                                  CertificateEntityId = entity != null ? entity.Id : null,
            },
                                                  Convert.ToDouble(amount),
                                                  paymentId);
            _db.OfficeRequests.Add(req);

            
            var paymentModel = new OfficePayment
            {
                Id = 0,
                OfficeId = officeId,
                TypeId = 2,
                RequestNameArabic = type?.NameArabic + (entity!= null ? ( " - " + entity?.NameArabic) : ""),
                RequestNameEnglish = type?.NameEnglish + (entity != null ? (" - " + entity?.NameEnglish) : ""), 
                PaymentDate = string.IsNullOrEmpty(postDate) ? DateTime.UtcNow : Convert.ToDateTime(postDate),
                Amount = Convert.ToDouble(amount),
                YearsCount = 0,
                IsPaid = true,
                PaymentCategoryArabic = "طلب شهادة",
                PaymentCategoryEnglish = "Certificate Request",
                PaymentNumber = paymentId,

            };
            _db.OfficePayments.Add(paymentModel);
            html = GenerateSuccessHtml(office.NameArabic,
                                            office.NameEnglish,
                                            Convert.ToDouble(amount),
                                            paymentId,
                                            string.IsNullOrEmpty(postDate) ? DateTime.UtcNow : Convert.ToDateTime(postDate),
                                            string.IsNullOrEmpty(result) ? "success" : result,
                                            type?.NameEnglish + (entity != null ? (" - " + entity?.NameEnglish) : ""),
                                            type?.NameArabic + (entity != null ? (" - " + entity?.NameArabic) : ""),
                                            referenceId,
                                            lang,
                                            returnUrl);




            _db.SaveChanges();
            var receipt = await _requestService.GenerateRequestReceipt(req.Id);
            if (receipt.Success == false)
            {
                return new ContentResult
                {
                    Content = "<p>Generate Receipt Failed </p>",
                    ContentType = "text/html"
                };
            }
            var certificate = await _requestService.GenerateRequestCertificate(req.Id);
            if (certificate.Success == false)
            {
                return new ContentResult
                {
                    Content = "<p>Generate Certificate Failed </p>",
                    ContentType = "text/html"
                };
            }
            return new ContentResult
            {
                Content = html,
                ContentType = "text/html"
            };

        }


        [HttpPost("renew-success")]
        public async Task<IActionResult> RenewSuccess()
        {

            var account = new AccountConfig("278101", "Jtp8QnwB", "W7X5A95YG9C30B1K");
            var payment = new Payment(account);
            var html = "";
            var form = HttpContext.Request.Form["trandata"].ToString();
            string returndata = payment.Decrypt(form).ToString();
            var returnlist = returndata.Split('&').ToList();
            var amount = returnlist.FirstOrDefault(x => x.Contains("amt")).Split('=')[1].ToString();
            var officeIdString = returnlist.FirstOrDefault(x => x.Contains("trackid")).Split('=')[1].ToString();
            var officeSuccess = int.TryParse(officeIdString, out int officeId);
            var result = returnlist.FirstOrDefault(x => x.Contains("result")).Split('=')[1].ToString().ToLower();
            //var pt = returnlist.FirstOrDefault(x => x.Contains("udf5")).Split('=')[1].ToString();
            var lang = returnlist.FirstOrDefault(x => x.Contains("udf3")).Split('=')[1].ToString();
            var returnUrl = returnlist.FirstOrDefault(x => x.Contains("udf4")).Split('=')[1].ToString();
            returnUrl = returnUrl.Replace("_", ":").Replace(",", "/");
            var paymentId = returnlist.FirstOrDefault(x => x.Contains("paymentid")).Split('=')[1].ToString();
            var referenceId = returnlist.FirstOrDefault(x => x.Contains("ref")).Split('=')[1].ToString(); // long number ex: 100202108105521282
            var postDate = returnlist.FirstOrDefault(x => x.Contains("postdate")).Split('=')[1].ToString();
            var office = _db.Offices.Find(officeId);

            if (string.IsNullOrEmpty(referenceId) ||!(officeSuccess))
            {
                var htmlerror = GenerateFailedHtml(lang);
                return new ContentResult
                {
                    Content = htmlerror,
                    ContentType = "text/html"
                };
            }

            var renewYearsString = returnlist.FirstOrDefault(x => x.Contains("udf1")).Split('=')[1].ToString();
            var renewYearsSuccess = int.TryParse(renewYearsString, out int renewYears);
            var missedYearsString = returnlist.FirstOrDefault(x => x.Contains("udf2")).Split('=')[1].ToString();
            var missedYearsSuccess = int.TryParse(missedYearsString, out int missedYears);
            var reqNameArabic = "";
            var reqNameEnglish = "";
            if (missedYears > 0)
            {
                reqNameArabic = "رسوم تجديد الاشتراك لفترة الانقطاع لمدة (" + missedYears + ") سنوات" +
                                    " - ";
                reqNameEnglish = "Missing Period Registration Fees For (" + missedYears + ") Years" +
                                    " - ";
            }
            var newPayment = new OfficePayment
            {
                Id = 0,
                OfficeId = office.Id,
                TypeId = 2,
                RequestNameArabic = reqNameArabic +
                                    "رسوم تجديد الاشتراك لمدة (" + renewYears + ") سنوات",
                RequestNameEnglish = reqNameEnglish +
                                     "Renew Registration Fees For (" + renewYears + ") Years",
                PaymentDate = string.IsNullOrEmpty(postDate) ? DateTime.UtcNow : Convert.ToDateTime(postDate),
                Amount = Convert.ToDouble(amount),
                YearsCount = renewYears + missedYears,
                MembershipEndDate = office.MembershipEndDate.Value.AddYears(renewYears + missedYears),
                IsPaid = true,
                PaymentCategoryArabic = "تجديد اشتراك",
                PaymentCategoryEnglish = "Renew Registration",
                PaymentNumber = paymentId,
            };
            _db.OfficePayments.Add(newPayment);
            office.MembershipEndDate = office.MembershipEndDate.Value.AddYears(renewYears + missedYears);
            office.IsActive = true;
            office.IsVerified = true;
            html = GenerateSuccessHtml(office.NameArabic,
                                            office.NameEnglish,
                                            Convert.ToDouble(amount),
                                            paymentId,
                                            string.IsNullOrEmpty(postDate) ? DateTime.UtcNow : Convert.ToDateTime(postDate),
                                            string.IsNullOrEmpty(result) ? "success" : result,
                                            "Renew Registration Fees" + " - " + "KFEOCH",
                                            "رسوم تجديد الاشتراك" + " - " + "الاتحاد الكويتي",
                                            referenceId,
                                            lang,
                                            returnUrl);


            _db.SaveChanges();
            var receipt = await _registrationService.GenerateRenewReceipt(newPayment.Id);
            if (receipt.Success == false)
            {
                return new ContentResult
                {
                    Content = "<p>Generate Receipt Failed </p>",
                    ContentType = "text/html"
                };
            }

            return new ContentResult
            {
                Content = html,
                ContentType = "text/html"
            };
        }



        private string GenerateFailedHtml(string lang)
        {
            return "<p>Payment Failed </p>";

        }

        private string GenerateSuccessHtml(string officeNameArabic,
                                           string officeNameEnglish,
                                           double amount,
                                           string paymentId,
                                           DateTime postDate,
                                           string result,
                                           string orderIdEn,
                                           string orderIdAr,
                                           string referenceId,
                                           string lang,
                                           string returnUrl)
        {
            if (lang == "ar")
            {
                return @$"<html>
                                <head>
                                <meta charset = 'UTF-8'>
                                </head>
                                <table style='border-collapse: collapse; width: 100%; direction : rtl'>
                                <tr>
                                <td align= center>
                                <div style='border: 2px outset #000080;width: 30%;    margin-bottom: 20px;
                                                                        position: relative;
                                                                        text-align: left;
                                                                        display: inline-block;
                                                                        padding: 44px;
                                                                        border-radius: 8px;
                                                                        height: auto;
                                                                        border: 2px solid #1976D2;
										                                align : center;'> 
                        <table style='border-collapse: collapse; width: 100%;'>
                        <tbody>
                        <tr>
                        <td style='width: 100%;' colspan='2'><img style='width: 80%; display: block; margin-left: auto; margin-right: auto;' src='https://kfeoch-api.techteec.net/logos/logo-horizontal.png' /></td>
                        </tr>
						<tr>
						<td colspan='2'> &nbsp;</td>
						</tr>
                        <tr >
                        <td style='width: 100%; text-align: center;' colspan='2'><span style='color: #1976D2;'><em><strong style = 'text-decoration : underline'>معلومات الدفع</strong></em></span></td>
                        </tr>
						<tr>
						<td colspan='2'> &nbsp;</td>
						</tr>
                        <td style='width: 34.7695%;'>اسم المكتب</td>
                        <td style='width: 65.2305%;'>{officeNameArabic}</td>
                        </tr>
                        <tr >
                        <td style='width: 34.7695%;'>المبلغ</td>
                        <td style='width: 65.2305%;'>{amount}  د.ك</td>
                        </tr>
                        <tr >
                        <td style='width: 34.7695%;'>معرف الدفع</td>
                        <td style='width: 65.2305%;'>{paymentId}</td>
                        </tr>
                        <tr >
                        <td style='width: 34.7695%;'>تاريخ الدفع</td>
                        <td style='width: 65.2305%;'>{postDate}</td>
                        </tr>
                        <tr >
                        <td style='width: 34.7695%;'>الحالة</td>
                        <td style='width: 65.2305%;'>{result}</td>
                        </tr>
                        <tr >
                        <td style='width: 34.7695%;'>رقم الطلب</td>
                        <td style='width: 65.2305%;'>{orderIdAr}</td>
                        </tr>
                        <tr>
                        <td style='width: 34.7695%;'>رقم المرجع</td>
                        <td style='width: 65.2305%;'>{referenceId}</td>
                        </tr>
						<tr>
						<td colspan='2'> &nbsp;</td>
						</tr>
                        <tr>
                        <td style='width: 100%; text-align: center;' colspan='2'><a href='{returnUrl}'> العودة إلى صفحة المكتب</a></td>
                        </tr>
                        </tbody>
                        </table>
                        </div>
                        </td>
                        </tr>
                        </table>
                        </html>";
            }

            return @$"<table style='border-collapse: collapse; width: 100%;'>
                                <tr>
                                <td align= center>
                                <div style='border: 2px outset #000080;width: 30%;    margin-bottom: 20px;
                                        position: relative;
                                        text-align: left;
                                        display: inline-block;
                                        padding: 44px;
                                        border-radius: 8px;
                                        height: auto;
                                        border: 2px solid #1976D2;
										align : center;'> 
                        <table style='border-collapse: collapse; width: 100%;'>
                        <tbody>
                        <tr>
                        <td style='width: 100%;' colspan='2'><img style='width: 80%; display: block; margin-left: auto; margin-right: auto;' src='https://kfeoch-api.techteec.net/logos/logo-horizontal.png' /></td>
                        </tr>
						<tr>
						<td colspan='2'> &nbsp;</td>
						</tr>
                        <tr >
                        <td style='width: 100%; text-align: center;' colspan='2'><span style='color: #1976D2;'><em><strong style = 'text-decoration : underline'>Payment Result</strong></em></span></td>
                        </tr>
						<tr>
						<td colspan='2'> &nbsp;</td>
						</tr>
                        <td style='width: 34.7695%;'>Office Name</td>
                        <td style='width: 65.2305%;'>{officeNameEnglish}</td>
                        </tr>
                        <tr >
                        <td style='width: 34.7695%;'>Amount</td>
                        <td style='width: 65.2305%;'>{amount} KWD</td>
                        </tr>
                        <tr >
                        <td style='width: 34.7695%;'>Payment Id</td>
                        <td style='width: 65.2305%;'>{paymentId}</td>
                        </tr>
                        <tr >
                        <td style='width: 34.7695%;'>Payment Date</td>
                        <td style='width: 65.2305%;'>{postDate}</td>
                        </tr>
                        <tr >
                        <td style='width: 34.7695%;'>Result</td>
                        <td style='width: 65.2305%;'>{result}</td>
                        </tr>
                        <tr >
                        <td style='width: 34.7695%;'>Order Id</td>
                        <td style='width: 65.2305%;'>{orderIdEn}</td>
                        </tr>
                        <tr>
                        <td style='width: 34.7695%;'>Reference Id</td>
                        <td style='width: 65.2305%;'>{referenceId}</td>
                        </tr>
						<tr>
						<td colspan='2'> &nbsp;</td>
						</tr>
                        <tr>
                        <td style='width: 100%; text-align: center;' colspan='2'><a href='{returnUrl}'> Back To Office Page </a></td>
                        </tr>
                        </tbody>
                        </table>
                        </div>
                        </td>
                        </tr>
                        </table>";
        }
    }
}
