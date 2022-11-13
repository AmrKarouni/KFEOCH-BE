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
    }
}
