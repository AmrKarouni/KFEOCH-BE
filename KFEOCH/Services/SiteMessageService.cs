using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KFEOCH.Services
{
    public class SiteMessageService : ISiteMessageService
    {
        private readonly ApplicationDbContext _db;
        public SiteMessageService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<SiteMessage> GetAllMessages()
        {
            var messages = _db.SiteMessages.OrderByDescending(x => x.CreatedDate).ToList();
            if (messages == null)
            {
                return new List<SiteMessage>();
            }
            return messages;

        }


        public ResultWithMessage GetAllMessagesWithFilter(FilterModel model)
        {
            var list = new List<SiteMessage>();
            var messages = _db.SiteMessages.OrderByDescending(x => x.CreatedDate).ToList();
            if (!string.IsNullOrEmpty(model.SearchQuery))
            {
                messages = messages?.Where(x => x.Name.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                              x.Subject.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                              x.Email.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                              x.PhoneNumber.ToLower().Contains(model.SearchQuery.ToLower())).ToList();
            }

            var dataSize = messages.Count();
            var sortProperty = typeof(SiteMessage).GetProperty(model?.Sort ?? "CreatedDate");
            if (model?.Order == "asc")
            {
                list = messages?.OrderBy(x => sortProperty.GetValue(x)).ToList();
            }
            else
            {
                list = messages?.OrderByDescending(x => sortProperty.GetValue(x)).ToList();
            }

            var result = list.Skip(model.PageSize * model.PageIndex).Take(model.PageSize).ToList();
            return new ResultWithMessage
            {
                Success = true,
                Message = "",
                Result = new ObservableData(result, dataSize)
            };

        }


        public List<SiteMessage> GetAllUnredMessages()
        {
            var messages = _db.SiteMessages.Where(x => x.IsRead == false).OrderByDescending(x => x.CreatedDate).ToList();
            if (messages == null)
            {
                return new List<SiteMessage>();
            } 
            return messages;
        }

        public ResultWithMessage GetMessageById(int id)
        {
            var message = _db.SiteMessages.FirstOrDefault(x => x.Id == id);
            if (message == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Message Not Found",
                    MessageEnglish = "Message Not Found",
                    MessageArabic = "رسالة غير موجودة",
                };
            }
            return new ResultWithMessage
            {
                Success = true,
                Result = message,
            };
        }

        public async Task<ResultWithMessage> AddMessageAsync(SiteMessageBindingModel model)
        {
            var message = new SiteMessage(model);
            await _db.SiteMessages.AddAsync(message);
            _db.SaveChanges();
             return new ResultWithMessage
            {
                Success = true,
                Result = message,
            };
        }

        public ResultWithMessage MarkMessageAsRead(int id)
        {
            var message = _db.SiteMessages.FirstOrDefault(x => x.Id == id);
            if (message == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Message Not Found",
                    MessageEnglish = "Message Not Found",
                    MessageArabic = "رسالة غير موجودة",
                };
            }

            message.IsRead = true;
            message.ReadDate = DateTime.UtcNow;
            _db.Entry(message).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage
            {
                Success = true,
                Result = message,
            };
        }

        public ResultWithMessage MarkMessageAsUnread(int id)
        {
            var message = _db.SiteMessages.FirstOrDefault(x => x.Id == id);
            if (message == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Message Not Found",
                    MessageEnglish = "Message Not Found",
                    MessageArabic = "رسالة غير موجودة",
                };
            }

            message.IsRead = false;
            message.ReadDate = null;
            _db.Entry(message).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage
            {
                Success = true,
                Result = message,
            };
        }
    }
}
