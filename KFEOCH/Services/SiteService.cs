using KFEOCH.Contexts;
using KFEOCH.Models;
using KFEOCH.Models.Site;
using KFEOCH.Models.Views;
using KFEOCH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KFEOCH.Services
{
    public class SiteService : ISiteService
    {
        private readonly ApplicationDbContext _db;
        public SiteService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Article> GetPublishedArticles(FilterModel model)
        {
            var articles = _db.Articles?.Where(x => (x.IsDeleted == false) && (x.ShowInHome == true))
                                        .Skip((model.PageIndex) * (model.PageSize))
                                        .Take(model.PageSize);
            var sortProperty = typeof(Article).GetProperty(model?.Sort ?? "PublishDate");                   
            if (model?.Order == "asc")
            {
                articles?.OrderBy(x => sortProperty.GetValue(x));
            }
            articles?.OrderByDescending(x => sortProperty.GetValue(x));
            return articles?.ToList() ?? new List<Article>();
        }

        public List<SiteOfficeViewModel> GetOffices(FilterModel model)
        {
            var list = new List<SiteOfficeViewModel>();
            var offices = _db.Offices?.Include(x => x.Type)
                                      .Where(x => x.IsActive == true && x.ShowInHome == true)
                                      .Skip((model.PageIndex) * (model.PageSize))
                                      .Take(model.PageSize);
            var sortProperty = typeof(Office).GetProperty(model?.Sort ?? "NameArabic");
            if (offices == null)
            {
                return list;
            }
            if (model?.Order == "asc")
            {
                offices?.OrderBy(x => sortProperty.GetValue(x));
            }
            offices?.OrderByDescending(x => sortProperty.GetValue(x));
            list = offices?.Select(x => new SiteOfficeViewModel(x)).ToList();
            return list ?? new List<SiteOfficeViewModel>();
        }

        public async Task<ResultWithMessage> PostPostTypeAsync(PostType model)
        {
            var type = _db.PostTypes?.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish));
            if (type != null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Post Type {model.NameArabic} Already Exist !!!" };
            }
            await _db.PostTypes.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }

        public async Task<ResultWithMessage> DeletePostTypeAsync(int id)
        {
            var type = _db.PostTypes?.Find(id);
            if (type == null)
            {
                return new ResultWithMessage { Success = false, Message = $@"Post Type Not Found !!!" };
            }
            _db.PostTypes?.Remove(type);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Message = $@"Post Type Deleted !!!" };
        }


    }
}
