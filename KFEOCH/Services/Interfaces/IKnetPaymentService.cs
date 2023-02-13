namespace KFEOCH.Services.Interfaces
{
    public interface IKnetPaymentService
    {
        string GenerateRequestPayment(int officeId,
                                           double amount,
                                           int typeId,
                                           int entityId,
                                           string? lang,
                                           string? returnUrl);
        string GenerateRenewPayment(int officeId,
                                       double amount,
                                       double renewyears,
                                       double missedyears,
                                       string lang,
                                       string? returnUrl);
    }
}
