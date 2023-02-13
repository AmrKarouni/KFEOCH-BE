using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Dictionaries;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KFEOCH.Services
{
    public class OfficeRequestService : IOfficeRequestService
    {
        private readonly ApplicationDbContext _db;
        private readonly IKnetPaymentService _knetPaymentService;
        private readonly TimeZoneInfo timezone;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OfficeRequestService(ApplicationDbContext db, IKnetPaymentService knetPaymentService,IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _knetPaymentService = knetPaymentService;
            timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResultWithMessage> InitialOfficeRequestAsync(OfficeRequestBindingModel model)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, model.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }

            var office = _db.Offices?.FirstOrDefault(x => x.Id == model.OfficeId);
            if (office == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Not Found",
                    MessageEnglish = $@"Office Not Found",
                    MessageArabic = $@"مكتب غير موجود",
                };
            }
            var requesttype = _db.RequestTypes?.FirstOrDefault(x => x.Id == model.RequestTypeId);
            if (requesttype == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Request Type Not Found",
                    MessageEnglish = $@"Request Type Not Found",
                    MessageArabic = $@"نوع الطلب غير موجود",
                };
            }
            var certifatentity = new CertificateEntity();
            if (!string.IsNullOrEmpty(model.CertificateEntityId.ToString()))
            {
                certifatentity = _db.CertificateEntities?.FirstOrDefault(x => x.Id == model.CertificateEntityId);
                if (certifatentity == null)
                {
                    return new ResultWithMessage
                    {
                        Success = false,
                        Message = $@"Certificate Entity Not Found",
                        MessageEnglish = $@"Certificate Entity Not Found",
                        MessageArabic = $@"كيان الشهادة غير موجود",
                    };
                }
            }


            var paymentResult = _knetPaymentService.GenerateRequestPayment(office.Id,
                                                       requesttype.Amount,
                                                       requesttype.Id,
                                                       certifatentity.Id,
                                                       model.Lang,
                                                       model.ReturnUrl);
            if (paymentResult == "Failed")
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Error Connecting to Payment Gateway ",
                    MessageEnglish = "Error Connecting to Payment Gateway ",
                    MessageArabic = "حدث خطأ في الاتصال مع مخدم الدفع"
                };
            }
            return new ResultWithMessage { Success = true, Result = new { PaymentUrl = paymentResult } };
        }

        public async Task<ResultWithMessage> GetRequestById(int id)
        {
            var req = _db.OfficeRequests?.Include(x => x.Office)
                                         .Include(x => x.RequestType)
                                         .Include(x => x.CertificateEntity).FirstOrDefault(x => x.Id == id);
            if (req == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Request Not Found",
                    MessageEnglish = $@"Request Not Found",
                    MessageArabic = $@"طلب غير موجود",
                };
            }
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, req.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
            var viewmodel = new OfficeRequestViewModel(req);
            return new ResultWithMessage { Success = true, Result = viewmodel };
        }

        public ResultWithMessage GetAllPendingRequests()
        {
            var reqs = _db.OfficeRequests?.Include(x => x.Office)
                                          .Include(x => x.RequestType)
                                          .Include(x => x.CertificateEntity)
                                          .Where(x => x.IsCanceled == false &&
                                                      x.IsApproved == false &&
                                                      x.IsDone == false &&
                                                      x.IsDone == false)
                                         .OrderBy(x => x.CreatedDate)
                                         .Select(x => new OfficeRequestViewModel(x))
                                         .ToList();
            return new ResultWithMessage { Success = true, Result = reqs };
        }

        public async Task<ResultWithMessage> GetAllRequestsByOfficeId(int officeid)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, officeid);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
            var office = _db.Offices.FirstOrDefault(x => x.Id == officeid);
            if (office == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Not Found",
                    MessageEnglish = $@"Office Not Found",
                    MessageArabic = $@"مكتب غير موجود",
                };
            }
            var payments = _db.OfficePayments.Where(x => x.OfficeId == officeid).ToList();
            var reqs = _db.OfficeRequests?.Include(x => x.Office)
                                          .Include(x => x.RequestType)
                                          .Include(x => x.CertificateEntity)
                                          .Where(x => x.OfficeId == officeid)
                                          .OrderByDescending(x => x.CreatedDate)
                                          .Select(x => new OfficeRequestViewModel(x, payments))
                                          .ToList();

            return new ResultWithMessage { Success = true, Result = reqs };
        }

        public ResultWithMessage PutRequest(int id, OfficeRequest model)
        {
            if (id != model.Id)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Invalid Model !!!",
                    MessageEnglish = "Invalid Model !!!",
                    MessageArabic = "نموزج غير صالح !!!",
                };
            }
            var req = _db.OfficeRequests?.Include(x => x.Office)
                                          .Include(x => x.RequestType)
                                          .Include(x => x.CertificateEntity)
                                          .FirstOrDefault(x => x.Id == id);

            if (req == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Request Not Found",
                    MessageEnglish = $@"Request Not Found",
                    MessageArabic = $@"طلب غير موجود",
                };
            }
            req = model;
            _db.Entry(req).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = req };
        }

        public async Task<ResultWithMessage> GenerateRequestReceipt(int id)
        {
            var req = _db.OfficeRequests.FirstOrDefault(x => x.Id == id);
            if (req == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Request Not Found",
                    MessageEnglish = "Request Not Found",
                    MessageArabic = "طلب غير موجود"
                };
            }
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, req.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
            var payment = _db.OfficePayments?.Include(x => x.Office)
                                            .FirstOrDefault(x => x.PaymentNumber == req.PaymentNumber);
            if (payment == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Payment Not Found",
                    MessageEnglish = "Payment Not Found",
                    MessageArabic = "دفعة غير موجودة"
                };
            }
            var html = $@"<html>

                        <head>
                            <meta charset='UTF-8'>
                        </head>
                        <table width=100%>
                            <tr>
                                <td colspan='4' align='center' style='font-weight: 600; '>

                                    <table style='width : 50mm; '>
                                        <tr>
                                            <td colspan='2' align='center' style='font-weight: 600;'>
                                                <div style='width: 50mm '>
                                                    <p style='margin-bottom: 2px;'>سند قبض</p>
                                                    <p style='margin-top: 2px; margin-bottom: 2px;'>Receipt Voucher </p>
                                                    <hr>
                                                </div>
                                        <tr>
                                        <tr>
                                            <td align='left' style='font-weight: 600;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>Knet</p>
                                                </div>
                                            </td>
                                            <td align='right' style='font-weight: 600;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>كي نت</p>
                                                </div>
                                            </td>
                                        <tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td colspan='4' align='right'>
                                    <table>
                                        <tr>
                                            <td align='left' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>No.:</p>
                                                </div>
                                            </td>
                                            <td align='center' style='padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px; width: 4cm;'>{payment.Id}</p>
                                                </div>
                                            </td>
                                            <td align='left' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>:رقم السند</p>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td colspan='3' align='right'>
                                    <table>
                                        <tr>
                                            <td align='left' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>Date and Time</p>
                                                </div>
                                            </td>
                                            <td align='center' style='padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px; width: 4cm;'>
                                                       {TimeZoneInfo.ConvertTimeFromUtc(payment.PaymentDate, timezone).ToShortTimeString()}
                                                        <span>
                                                            &nbsp;
                                                        </span>
                                                        {TimeZoneInfo.ConvertTimeFromUtc(payment.PaymentDate, timezone).ToShortDateString()}
                                                    </p>
                                                </div>
                                            </td>
                                            <td align='right' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>التاريخ و الوقت</p>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align='left' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>Membership No. </p>
                                                </div>
                                            </td>
                                            <td align='center' style='padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px; width: 4cm;'>{payment.Office.LicenseId}</p>
                                                </div>
                                            </td>
                                            <td align='right' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>رقم العضــوية</p>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align='right'>
                                    <table>
                                        <tr>
                                            <td align='left' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>Received From Mr.\ Messrs:</p>
                                                </div>
                                            </td>
                                            <td colspan='2' align='center' style='padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px; width: 8cm;'>{payment.Office.NameArabic}</p>
                                                </div>
                                            </td>
                                            <td align='right' style='font-weight: 600; padding-top: 1cm;'>
                                                <div>
                                                    <p style='margin-bottom: 2px;'>:وصلنا من السيد / السادة</p>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td align='center' style=' padding-top: 1cm;'>

                                    <table
                                        style='border: 1px solid black;border-collapse: collapse; width: 95%; border-bottom: none; border-left: none; border-right: none;'>
                                        <tr style='border-bottom: 1px solid black;'>
                                            <th align='center'
                                                style='font-weight: 600; background-color:#ccc9c9; border-right:  1px solid black; border-left: 1px solid black;'>

                                                <p>م</p>
                                                <p>No.</p>

                                            </th>
                                            <th colspan='2' align='center'
                                                style='font-weight: 600; background-color:#ccc9c9; border-right:  1px solid black;'>

                                                <p style='width: 8cm;'>البيــــــان</p>
                                                <p style='width: 8cm;'>Description</p>

                                            </th>
                                            <th align='center'
                                                style='font-weight: 600; background-color:#ccc9c9; border-right:  1px solid black;'>

                                                <p>المبلغ</p>
                                                <p>Amount</p>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td align='center' style='border-right:  1px solid black;  border-left: 1px solid black;'>
                                                <div>
                                                    <p>1</p>
                                                </div>
                                            </td>
                                            <td colspan='2' align='right' style=' border-right: 1px solid black; padding-right: 10px;'>
                                                <div>
                                                    <p style='width: 8cm;'>{payment.RequestNameArabic}</p>
                                                </div>
                                            </td>
                                            <td align='center'
                                                style='border-left:  1px solid black;  border-right:  1px solid black;  padding-right: 10px;'>
                                                <div>
                                                    <p>{string.Format("{0:F2}", payment.Amount)}</p>
                                                </div>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan='3' align='right'
                                                style='font-weight: 600; border-top:  1px solid black; padding-right: 10px;'>
                                                <div>
                                                    <p style='width: 8cm;'> دينار كويتي فقط لاغير </p>
                                                </div>
                                            </td>
                                            <td align='center' style='font-weight: 700; border-top:  1px solid black;'>
                                                <div>
                                                    <p>{string.Format("{0:F2}", payment.Amount)}</p>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td style='font-weight: 600; padding-top: 3cm;  padding-left: 1cm; '>
                                    <p>
                                        <span>
                                            Recept
                                        </span>
                                        <span>
                                            &nbsp;
                                        </span>
                                        <span>
                                            &nbsp;
                                        </span>
                                        <span>
                                            المستلم
                                        </span>
                                    </p>
                                </td>
                            </tr>
                        </table>

                        </html>";
            payment.HtmlBody = html;
            _db.Entry(payment).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = new { Html = html } };

        }

        public async Task<ResultWithMessage> GenerateRequestCertificate(int id)
        {
            var req = _db.OfficeRequests.Include(x => x.Office)
                                        .Include(x => x.CertificateEntity)
                                        .FirstOrDefault(x => x.Id == id);
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, req.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
            if (req == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Request Not Found",
                    MessageEnglish = "Request Not Found",
                    MessageArabic = "طلب غير موجود"
                };
            }
            var payment = _db.OfficePayments?.FirstOrDefault(x => x.PaymentNumber == req.PaymentNumber);
            if (payment == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Payment Not Found",
                    MessageEnglish = "Payment Not Found",
                    MessageArabic = "دفعة غير موجودة"
                };
            }
            var owners = _db.OfficeOwners.Where(x => x.OfficeId == req.OfficeId).Select(x => x.NameArabic).ToList();

            var ownername = String.Join(", ", owners.ToArray());
            var html = "";
            if (req.RequestTypeId == 1)
            {
                html = $@"<html style='direction: rtl;'>
                    <head>
                            <meta charset='UTF-8'>
                    </head>

                    <body>
                            <table style='padding-top: 1cm; padding-bottom: 1cm;  width : 95%; '>
                                    <tr>
                                            <td align='center' style='font-weight: bold; font-size: large;width: 80%'>
                                                    <div>
                                                            <p>شهادة لمن يهمه الأمر</p>
                                                    </div>
                                            </td>
                                            <td align='left' style='font-size: medium;'>

                                                    <span>رقم مسلسل :</span>
                                                    <span>{req.Id}</span>
                                            </td>
                                    </tr>
                            </table>
                            <p style='font-weight: bold; font-size: large;'>
                                    يشهد اتحاد المكاتب الهندسية و الدور الاستشارية الكويتية بأن السادة :
                            </p>
                            <p style='font-size: medium; padding-right: 15px;'>
                                    {req.Office?.NameArabic}
                            </p>
                            <p>
                            <table style='border: 0.5px solid gray;border-collapse: separate collapse; width: 95%; height: 50px;'>
                                    <tr>
                                            <td style='font-size: small; border-left: 1px solid gray; width: 20%;'>
                                                    <p>
                                                            العائد لصاحبه المهندس /
                                                    </p>
                                            </td>
                                            <td>
                                                    <p style=' font-size: medium;'>
                                                            {ownername}
                                                    </p>
                                            </td>
                                    </tr>
                            </table>
                            </p>
                            <p style='font-size: medium;'>
                                    <span>
                                            مسجلين لدى اتحاد المكاتب الهندسية و الدور الاستشارية الكويتية تحت رقم /
                                    </span>
                                    <span> {req.Office.LicenseId} </span>
                            </p>
                            <p style='font-size: medium;'>
                                    <span>
                                            و ان عضويتهم تنتهي بتاريخ :
                                    </span>
                                    <span> {TimeZoneInfo.ConvertTimeFromUtc(req.Office.MembershipEndDate.Value, timezone).ToShortDateString()} </span>
                            </p>
                            <p style='font-size: medium;'>
                                    <span>
                                            و قد اعطيت لهم هذه الشهادة لتقديمها الى :
                                    </span>
                                    <span> {req.CertificateEntity.NameArabic} </span>
                            </p>
                            <p style='font-size: medium;'>
                                    <span>
                                            دون ادنى مسئولية على الاتحاد تجاه الغير .
                                    </span>
                            </p>
                            <p style='font-size: medium;'>
                                    <span>
                                            حررت هذه الشهادة بتاريخ :
                                    </span>
                                    <span>{TimeZoneInfo.ConvertTimeFromUtc(req.CreatedDate.Value, timezone).ToShortDateString()}</span>
                            </p>
                          <div align=left style='font-size: large;font-weight: bold; padding-left: 100px;'>
                            <table style='height: 3cm; align-content: left;font-size: large;font-weight: bold;'>
                                <tr>
                                  <td>
                                    <p align=center>اتحاد المكاتب الهندسية</p>
                                    <p align=center>و الدور الاستشارية الكويتية</p>
                                  </td>  
                                </tr>
                                 <tr style='height: 2cm;'>
                                    </tr>

                            </table>
                        </div>
                        <p style='font-size: medium; '>
                                <table style='width: 30%;'>
                                    <tr>
                                            <td>
                                                    <p style='font-size: medium; width: 80%;'>رقم الايصال </p>
                                            </td>
                                            <td>
                                                    <p style='font-size: medium;'>
                                                            <span>:</span>
                                                            <span>
                                                                    {payment.Id}
                                                            </span>
                                                    </p>
                                            </td>
                                    </tr>
                                    <tr>
                                            <td>
                                                    <p style='font-size: medium;width: 80%;'>تاريخ الايصال</p>
                                            </td>

                                            <td>
                                                    <p style='font-size: medium;'></p>
                                                    <span>:</span>
                                                    <span>
                                                            {TimeZoneInfo.ConvertTimeFromUtc(payment.PaymentDate, timezone).ToShortDateString()}
                                                    </span>
                                            </td>
                                    </tr>
                            </table>
                            </p>
                            
                            <p style='font-weight: bold;  font-size: small;'>
                            <p>
                                    - هذه الشهادة صادرة من النظام اللالي و ليست بحاجة الى ختم او توقيع و للتحقق من بيان المستخرج يرجى مسح QR
                                    Code
                            </p>
                            <p>- الشهادة صالحة لمدة ثلاثة شهور من تاريخ اصدارها</p>
                            </p>
                    </body>

                    </html>";
            }
            if (req.RequestTypeId == 2)
            {
                html = $@"<html style='direction: rtl;'>
                    <head>
                            <meta charset='UTF-8'>
                    </head>

                    <body>
                            <table align='center' style='padding-top: 1cm; padding-bottom: 1cm;  width : 20%; '>
                                    <tr>
                                            <td align='center' style='font-weight: bold; font-size: large;width: 100%'>
                                                    <div>
                                                            <p>شهادة تحديث بيانات</p>
                                                    </div>
                                                    <hr>
                                            </td>
                                    </tr>
                            </table>
                            <p style='font-size: medium;'>
                                    يشهد اتحاد المكاتب الهندسية و الدور الاستشارية الكويتية بأن :

                            </p>
                            <p style='font-weight: bold; font-size: large;padding-right: 15px;'>
                                    <span>مكتب / دار</span>
                                    <span style='width: 50px;'> {req.Office.NameArabic}</span>
                                    <span>للاستشارات الهندسية.</span>

                            </p>
                            <p style='font-weight: bold; font-size: large;padding-right: 130px;'>
                                    <span>العائد لصاحبه المهندس /</span>
                                    <span style='width: 100px;'> {ownername}</span>
                            </p>

                            <p style='font-size: medium;'>
                                    <span>و رقم الهاتف الأرضي :</span>
                                    <span style='width: 100px;'> {req.Office.PhoneNumber}</span>

                            </p>
                            <p style='font-size: medium;'>
                                    <span>و عنوانه :</span>
                                    <span style='width: 100px;'> {req.Office.Address}</span>

                            </p>
                            <p style='font-size: medium;'>
                                    <span>الرقم الاّلي للعنوان :</span>
                                    <span style='width: 100px;'> {req.Office.AutoNumberOne}</span>

                            </p>
                            <p style='font-size: medium;'>
                                    <span>وانه مسجل لدينا برقم</span>
                                    <span>(
                                    <span style='width: 50px;'> {req.Office.LicenseId}</span>
                                    <span> )</span>
                                    <span>و العضوية صالحة حتى تاريخ </span>
                                    <span style='width: 50px;'> {TimeZoneInfo.ConvertTimeFromUtc(req.Office.MembershipEndDate.Value, timezone).ToShortDateString()} </span>
                                    <span>.</span>
                            </p>

                            <p style='font-size: medium;'>
                                    و قد أعطيت له هذه الشهادة لتقديمها إلى لجنة تنظيم مزاولة المهنة - بلدية الكويت دون أدنى مسئولية على
                                    الاتحاد تجاه الغير.
                            </p>
                            <p style='font-size: medium;'>
                                    و يتعهد بالالتزام باحترام أصول المهنة المذكورة في النظام الأساسي و اللائحة الداخلية للاتحاد و لائحة
                                    تنظيم مزاولة المهنة و أي مستجدات عليها و إخطار الاتحاد فور حدوث أي تغير يطرأ على البيانات الواردة أعلاه
                                    و تمكين الااتحاد من الكشف و الاطلاع و الزيارة في أي وقت دون سابق اخطار للتأكد من عدم مخالفة القوانين و
                                    اللوائح للمنظمة للعمل الاستشاري الهندسي.
                            </p>


                            <p style=' font-size: medium;padding-right: 15px;'>
                                    <span>التاريخ :</span>
                                    <span style='width: 50px;'>{TimeZoneInfo.ConvertTimeFromUtc(req.CreatedDate.Value, timezone).ToShortDateString()}</span>
                            </p>

                             <div align=left style='font-size: large;font-weight: bold; padding-left: 100px;'>
                            <table style='height: 3cm; align-content: left;font-size: large;font-weight: bold;'>
                                <tr>
                                  <td>
                                    <p align=center>اتحاد المكاتب الهندسية</p>
                                    <p align=center>و الدور الاستشارية الكويتية</p>
                                  </td>  
                                </tr>
                                 <tr style='height: 2cm;'>
                                    </tr>

                            </table>
                        </div>
                    </body>

                    </html>";
            }

            req.HtmlBody = html;
            _db.Entry(req).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = new { Html = html } };

        }

        public async Task<ResultWithMessage> GetAllOfficeRequestPayments(int officeid)
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, officeid);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
            var officepayments = _db.OfficePayments?
                                 .Include(x => x.Type)
                                 .Where(x => x.OfficeId == officeid && x.PaymentCategoryEnglish.StartsWith("Certificate Request"))
                                 .OrderByDescending(x => x.PaymentDate).Select(x => new OfficePaymentWithTypeViewModel(x)).ToList();
            return new ResultWithMessage { Success = true, Result = officepayments };
        }

        public async Task<ResultWithMessage> GetRequestReceipt(int id)
        {
            var req = _db.OfficeRequests.FirstOrDefault(x => x.Id == id);
            if (req == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Request Not Found",
                    MessageEnglish = "Request Not Found",
                    MessageArabic = "طلب غير موجود"
                };
            }

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, req.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }

            var payment = _db.OfficePayments?.Include(x => x.Office)
                                            .FirstOrDefault(x => x.PaymentNumber == req.PaymentNumber);
            if (payment == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Payment Not Found",
                    MessageEnglish = "Payment Not Found",
                    MessageArabic = "دفعة غير موجودة"
                };
            }
            return new ResultWithMessage { Success = true, Result = new { Html = string.IsNullOrEmpty(payment.HtmlBody) ? "" : payment.HtmlBody } };
        }

        public async Task<ResultWithMessage> GetPaymentReceipt(int id)
        {
            var payment = _db.OfficePayments?.FirstOrDefault(x => x.Id == id);
            if (payment == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Payment Not Found",
                    MessageEnglish = "Payment Not Found",
                    MessageArabic = "دفعة غير موجودة"
                };
            }

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, payment.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }
            return new ResultWithMessage { Success = true, Result = new { Html = string.IsNullOrEmpty(payment.HtmlBody) ? "" : payment.HtmlBody } };
        }

        public async Task<ResultWithMessage> GetRequestCertificate(int id)
        {
            var request = _db.OfficeRequests?.FirstOrDefault(x => x.Id == id);
            if (request == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Request Not Found",
                    MessageEnglish = "Request Not Found",
                    MessageArabic = "طلب غير موجود"
                };
            }

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
            var can = await _userService.CanManipulateOffice(principal, request.OfficeId);
            if (!can)
            {
                return new ResultWithMessage { Success = false, Message = $@"Unauthorized" };
            }

            bool HasQr = false;
            if (request.RequestTypeId == 1)
            {
                HasQr = true;
            }
            return new ResultWithMessage { 
                        Success = true,
                        Result = new { 
                                        Html = string.IsNullOrEmpty(request.HtmlBody) ? "" : request.HtmlBody,
                                        HasQr = HasQr
                        }
            };
        }
    }
}
