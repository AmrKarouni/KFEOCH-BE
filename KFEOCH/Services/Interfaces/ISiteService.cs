using KFEOCH.Models.Site;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface ISiteService
    {
        List<Article> GetPublishedArticles(FilterModel model);
        List<SiteOfficeViewModel> GetOffices(FilterModel model);
    }
}
