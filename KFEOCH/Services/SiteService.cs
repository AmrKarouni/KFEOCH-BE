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
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SiteService(ApplicationDbContext db, IFileService fileService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
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

        public ResultWithMessage GetAllPostTypes ()
        {
            var types = _db.PostTypes.Where(x => x.IsDeleted == false).ToList();
            return new ResultWithMessage
            {
                Success = true,
                Result = types ?? new List<PostType>()
            };
        }

        public async Task<ResultWithMessage> AddPostTypeAsync(PostType model)
        {
            var type = _db.PostTypes?.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                   || (x.NameEnglish == model.NameEnglish));
            if (type != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Post Type {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Post Type {model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"نوع المنشور {model.NameArabic} موجود مسبقاً !!!"
                };
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
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Post Type Not Found !!!",
                    MessageEnglish = $@"Post Type Not Found !!!",
                    MessageArabic = $@"نوع المنشور غير موجود !!!"
                };
            }
            _db.PostTypes?.Remove(type);
            _db.SaveChanges();
            return new ResultWithMessage
            {
                Success = true,
                Message = "Post Type Deleted !!!",
                MessageEnglish = "Post Type Deleted !!!",
                MessageArabic = $@"تم حذف نوع المنشور !!!"
            };
        }


        public async Task<ResultWithMessage> AddPostAsync(PostBindingModel model)
        {
            var type = _db.PostTypes?.Find(model.PageId);
            if (type == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Post Type Not Found !!!",
                    MessageEnglish = $@"Post Type Not Found !!!",
                    MessageArabic = $@"نوع المنشور غير موجود !!!"
                };
            }
            var oldpost = _db.Posts.Where(x => x.PageId == model.PageId
                                          && x.IsPublished == true
                                          && (x.TitleArabic == model.TitleArabic || x.TitleEnglish == model.TitleEnglish)).ToList();
            _db.Posts.RemoveRange(oldpost);

            var post = new Post(model);
            await _db.Posts.AddAsync(post);
            if (model.Image != null)
            {
                var imagemodel = new PostFileModel();
                imagemodel.PostId = post.Id;
                imagemodel.PageId = post.PageId;
                imagemodel.Image = model.Image;
                var uploadResult = await _fileService.UploadPostImage(imagemodel, "posts");
                if (!uploadResult.Success)
                {
                    return new ResultWithMessage
                    {
                        Success = false,
                        Message = $@"Upload Image Failed !!!",
                        MessageEnglish = $@"Upload Image Failed !!!",
                        MessageArabic = $@"فشل في تحميل الصورة !!!"
                    };
                }
                post.ImageUrl = uploadResult.Message;
                post.ThumbnailUrl = uploadResult.Message;
            }
            _db.SaveChanges();
            return new ResultWithMessage { Success = true,
                Message = $@"Post Added!!!",
                MessageEnglish = $@"Post Added!!!",
                MessageArabic = $@"تمت إضافة المنشور !!!"
            };
        }

        public Post GetPostById(int id)
        {
            var post = _db.Posts?.Include(x => x.Sections).FirstOrDefault(x => x.Id == id);
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            if (post != null && post.ImageUrl != null)
            {
                post.ImageUrl = hostpath + post.ImageUrl;
                post.ThumbnailUrl = hostpath + post.ThumbnailUrl;
            }
            return post ?? new Post();
        }

        public ResultWithMessage GetAllPostsByTypeId(int id)
        {
            var posts = _db.PostTypes.Include(x => x.Pages).ThenInclude(x => x.Posts).Where(x => x.Id == id).ToList();
            return new ResultWithMessage { Success = true, Result = posts };
        }
    }
}
