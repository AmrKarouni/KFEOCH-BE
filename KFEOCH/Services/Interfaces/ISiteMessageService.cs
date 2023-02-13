using KFEOCH.Models;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface ISiteMessageService
    {
        List<SiteMessage> GetAllMessages();
        ResultWithMessage GetAllMessagesWithFilter(FilterModel model);
        List<SiteMessage> GetAllUnredMessages();
        ResultWithMessage GetMessageById(int id);
        Task<ResultWithMessage> AddMessageAsync(SiteMessageBindingModel model);
        ResultWithMessage MarkMessageAsRead(int id);
        ResultWithMessage MarkMessageAsUnread(int id);
    }
}
