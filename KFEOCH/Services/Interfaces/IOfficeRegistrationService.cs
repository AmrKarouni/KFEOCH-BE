using KFEOCH.Models.Binding;
using KFEOCH.Models.Dictionaries;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface IOfficeRegistrationService
    {
        ResultWithMessage GetRnewFieldsByOfficeId(int id);
        ResultWithMessage RenewOffice(OfficeRenewBindingModel model);
    }
}
