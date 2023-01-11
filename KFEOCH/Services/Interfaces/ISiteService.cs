using KFEOCH.Models.Site;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface ISiteService
    {
        List<SiteOfficeViewModel> GetOffices(FilterModel model);
        ResultWithMessage GetAllPostTypes();
        Task<ResultWithMessage> AddPostTypeAsync(PostType model);
        Task<ResultWithMessage> DeletePostTypeAsync(int id);
        Task<ResultWithMessage> AddPostAsync(PostBindingModel model);
        Post GetPostById(int id);
        ResultWithMessage GetAllPostsByTypeId(int id);
    }
}
