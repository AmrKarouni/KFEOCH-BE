using KFEOCH.Models.Site;
using KFEOCH.Models.Views;

namespace KFEOCH.Services.Interfaces
{
    public interface ISiteService
    {
        ResultWithMessage GetOffices(FilterModel model);
        ResultWithMessage GetOfficeForGuest(int id);
        List<Page> GetAllPages();
        ResultWithMessage GetPageByUrl(string url);
        ResultWithMessage PutPage(int id, PageBindingModel model);
        Task<ResultWithMessage> UploadPageImage(ImageModel model);
        ResultWithMessage GetPostById(int id);
        Task<ResultWithMessage> AddPostAsync(PostBindingModel model);
        Task<ResultWithMessage> AddPostTitleAsync(PostTitleBindingModel model);
        ResultWithMessage PutPost(int id, PostBindingModel model);
        Task<ResultWithMessage> DeletePost(int id);
        Task<ResultWithMessage> UploadPostImage(ImageModel model);
        ResultWithMessage ReorderPost(int pageId, int previousIndex, int currentIndex);

        ResultWithMessage GetSectionById(int id);
        Task<ResultWithMessage> AddSectionAsync(SectionBindingModel model);
        Task<ResultWithMessage> AddSectionWithImageAsync(SectionWithImageBindingModel model);
        ResultWithMessage PutSection(int id, SectionBindingModel model);
        Task<ResultWithMessage> DeleteSection(int id);
        Task<ResultWithMessage> UploadSectionImage(ImageModel model);
        ResultWithMessage ReorderSection(int postId, int previousIndex, int currentIndex);
        List<PostCategory> GetAllPostCategories();
        Task<ResultWithMessage> AddPostCategoryAsync(PostCategory model);
        ResultWithMessage PutPostCategory(int id, PostCategory model);
        ResultWithMessage GetPostsByFilter(FilterModel model);

        ResultWithMessage GetLastNews();
    }
}
