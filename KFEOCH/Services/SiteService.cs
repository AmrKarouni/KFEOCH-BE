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

        public ResultWithMessage GetOffices(FilterModel model)
        {
            var list = new List<SiteOfficeViewModel>();
            var offices = _db.Offices?.Include(x => x.Type)
                                      .Where(x => x.IsActive == true && x.ShowInHome == true)
                                      .ToList();
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            if (!string.IsNullOrEmpty(model.SearchQuery))
            {
                    offices = offices?.Where(x => x.NameArabic.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                  x.NameEnglish.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                  x.Email.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                  x.PhoneNumber.ToLower().Contains(model.SearchQuery.ToLower())).ToList();
            }
            var dataSize = offices.Count();
            foreach (var o in offices)
            {

                if (!string.IsNullOrEmpty(o.LogoUrl))
                {
                    o.LogoUrl = hostpath + o.LogoUrl;
                }
            }
            var sortProperty = typeof(SiteOfficeViewModel).GetProperty(model?.Sort ?? "Id");
            if (model?.Order == "desc")
            {
                list = offices?.Select(o => new SiteOfficeViewModel(o)).OrderByDescending(x => sortProperty.GetValue(x)).ToList();
            }
            else
            {
                list = offices?.Select(o => new SiteOfficeViewModel(o)).OrderBy(x => sortProperty.GetValue(x)).ToList();
            }

            var result = list.Skip(model.PageSize * model.PageIndex).Take(model.PageSize).ToList();
            
            return new ResultWithMessage
            {
                Success = true,
                Message = "",
                Result = new ObservableData(result, dataSize)
            };
        }

        public ResultWithMessage GetOfficeForGuest(int id)
        {
            var office = _db.Offices.Include(x => x.Type)
                                        .Include(x => x.Area)
                                        .Include(x => x.Governorate)
                                        .Include(x => x.Country)
                                        .Include(x => x.Entity)
                                        .Include(x => x.LegalEntity)
                                        .FirstOrDefault(x => x.Id == id);
            if (office == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Office Not Found",
                    MessageEnglish = "Office Not Found",
                    MessageArabic = "المكتب غير موجود",
                };
            }
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var resOffice = new OfficeGuestViewModel(office);
            if (!string.IsNullOrEmpty(resOffice.LogoUrl))
            {
                resOffice.LogoUrl = hostpath + resOffice.LogoUrl;
            }
            return new ResultWithMessage
            {
                Success = true,
                Result = resOffice
            };

        }

        public List<Page> GetAllPages()
        {
            var pages = _db.Pages.Include(x => x.Posts).Where(x => x.IsPublished == true && x.IsDeleted == false).ToList();
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            foreach (var page in pages)
            {
                if (page != null && !string.IsNullOrEmpty(page.ImageUrl))
                {
                    page.ImageUrl = hostpath + page.ImageUrl;
                    page.ThumbnailUrl = hostpath + page.ThumbnailUrl;
                }
            }
            return pages ?? new List<Page>();
        }

        public ResultWithMessage GetPageByUrl(string url)
        {
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var page = _db.Pages.Include(x => x.Posts.OrderBy(o => o.Order)).ThenInclude(x => x.Category)
                                .FirstOrDefault(x => x.HostUrl == url);
            var res = new PageViewModel
            {
                Id = page.Id,
                TitleArabic = page.TitleArabic,
                TitleEnglish = page.TitleEnglish,
                SubtitleArabic = page.SubtitleArabic,
                SubtitleEnglish = page.SubtitleEnglish,
                BodyArabic = page.BodyArabic,
                BodyEnglish = page.BodyEnglish,
                ImageUrl = string.IsNullOrEmpty(page.ImageUrl) ? null : hostpath + page.ImageUrl,
                ThumbnailUrl = string.IsNullOrEmpty(page.ThumbnailUrl) ? null : hostpath + page.ThumbnailUrl,
                Template = page.Template,
                CreatedDate = page.CreatedDate,
                PublishDate = page.PublishDate,
                HostUrl = page.HostUrl,
                IsPublished = page.IsPublished,
                IsDeleted = page.IsDeleted,
                Posts = page.Posts.Select(po => new PostWithoutBodyViewModel
                {
                    Id = po.Id,
                    TitleArabic = po.TitleArabic,
                    TitleEnglish = po.TitleEnglish,
                    SubtitleArabic = po.SubtitleArabic,
                    SubtitleEnglish = po.SubtitleEnglish,
                    BodyArabic = null,
                    BodyEnglish = null,
                    PageId = po.PageId,
                    CategoryId = po.CategoryId,
                    CategoryNameArabic = po.Category?.NameArabic,
                    CategoryNameEnglish = po.Category?.NameEnglish,
                    ImageUrl = string.IsNullOrEmpty(po.ImageUrl) ? null : hostpath + po.ImageUrl,
                    ThumbnailUrl = string.IsNullOrEmpty(po.ThumbnailUrl) ? null : hostpath + po.ThumbnailUrl,
                    CreatedDate = po.CreatedDate,
                    PublishDate = po.PublishDate,
                    Url = po.Url,
                    IsPublished = po.IsPublished,
                    IsDeleted = po.IsDeleted,
                    Order = po.Order
                })
            };
            if (res == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "No Page Found !!!",
                    MessageEnglish = "No Page Found !!!",
                    MessageArabic = "لم يتم العثور على الصفحة !!!",
                };
            }

            return new ResultWithMessage { Success = true, Result = res };
        }

        public ResultWithMessage PutPage(int id, PageBindingModel model)
        {
            if (id != model.Id)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Invalid Model !!!",
                    MessageEnglish = "Invalid Model !!!",
                    MessageArabic = "نموزج غير صالح !!!",
                };
            }
            var page = _db.Pages.FirstOrDefault(x => x.Id == model.Id);
            if (page == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Page Not Found !!!",
                    MessageEnglish = "Page Not Found !!!",
                    MessageArabic = " لم يتم العثور على الصفحة !!!",
                };
            }
            page.TitleArabic = model.TitleArabic;
            page.TitleEnglish = model.TitleEnglish;
            page.SubtitleArabic = model.SubtitleArabic;
            page.SubtitleEnglish = model.SubtitleEnglish;
            page.BodyArabic = model.BodyArabic;
            page.BodyEnglish = model.BodyEnglish;
            page.PublishDate = model.PublishDate;
            page.IsPublished = model.IsPublished;
            _db.Entry(page).State = EntityState.Modified;
            _db.SaveChanges();
            if (!string.IsNullOrEmpty(page.ImageUrl))
            {
                var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                page.ImageUrl = hostpath + page.ImageUrl;
                page.ThumbnailUrl = hostpath + page.ThumbnailUrl;
            }
            return new ResultWithMessage
            {
                Success = true,
                Result = model
            };
        }

        public async Task<ResultWithMessage> UploadPageImage(ImageModel model)
        {
            var page = _db.Pages.FirstOrDefault(x => x.Id == model.Id);
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            if (page == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Page Not Found !!!",
                    MessageEnglish = "Page Not Found !!!",
                    MessageArabic = " لم يتم العثور على الصفحة !!!",
                };
            }
            var uploadResult = await _fileService.UploadPageImage(model, "pages");
            if (!uploadResult.Success)
            {
                return new ResultWithMessage { Success = false, Message = $@"Upload Image Failed !!!" };
            }
            page.ImageUrl = uploadResult.MessageEnglish;
            page.ThumbnailUrl = uploadResult.MessageArabic;
            _db.Entry(page).State = EntityState.Modified;
            _db.SaveChanges();
            var result = new { ImageUrl = hostpath + uploadResult.Message };
            return new ResultWithMessage { Success = true, Result = result };
        }

        public ResultWithMessage GetPostById(int id)
        {
            var post = _db.Posts.Include(x => x.Sections.OrderBy(o => o.Order)).FirstOrDefault(x => x.Id == id);
            if (post == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "No Post Found !!!",
                    MessageEnglish = "No Post Found !!!",
                    MessageArabic = "لم يتم العثور على المنشور !!!",
                };
            }
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            foreach (var section in post.Sections)
            {
                if (!string.IsNullOrEmpty(section.ImageUrl))
                {
                    section.ImageUrl = hostpath + section.ImageUrl;
                    section.ThumbnailUrl = hostpath + section.ThumbnailUrl;
                }
            }
            if (post != null && !string.IsNullOrEmpty(post.ImageUrl))
            {
                post.ImageUrl = hostpath + post.ImageUrl;
                post.ThumbnailUrl = hostpath + post.ThumbnailUrl;
            }
            return new ResultWithMessage { Success = true, Result = post };
        }

        public async Task<ResultWithMessage> AddPostAsync(PostBindingModel model)
        {
            var newpost = new Post(model);
            var max = _db.Posts.Where(x => x.PageId == model.PageId).Max(x => x.Order) ?? -1;
            newpost.Order = max + 1;
            await _db.Posts.AddAsync(newpost);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = newpost };
        }

        public async Task<ResultWithMessage> AddPostTitleAsync(PostTitleBindingModel model)
        {
            var newpost = new Post(model);
            var max = _db.Posts.Where(x => x.PageId == model.PageId).Max(x => x.Order) ?? -1;
            newpost.Order = max + 1;
            await _db.Posts.AddAsync(newpost);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = newpost };
        }

        public ResultWithMessage PutPost(int id, PostBindingModel model)
        {
            if (id != model.Id)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Invalid Model !!!",
                    MessageEnglish = "Invalid Model !!!",
                    MessageArabic = "نموزج غير صالح !!!",
                };
            }
            var post = _db.Posts.FirstOrDefault(x => x.Id == model.Id);
            if (post == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "No Post Found !!!",
                    MessageEnglish = "No Post Found !!!",
                    MessageArabic = "لم يتم العثور على المنشور !!!",
                };
            }
            post.TitleArabic = model.TitleArabic;
            post.TitleEnglish = model.TitleEnglish;
            post.SubtitleArabic = model.SubtitleArabic;
            post.SubtitleEnglish = model.SubtitleEnglish;
            post.BodyArabic = model.BodyArabic;
            post.BodyEnglish = model.BodyEnglish;
            post.PublishDate = model.PublishDate;
            post.IsPublished = model.IsPublished;
            post.CategoryId = model.CategoryId;
            //post.Order = model.Order;
            _db.Entry(post).State = EntityState.Modified;
            _db.SaveChanges();
            if (!string.IsNullOrEmpty(post.ImageUrl))
            {
                var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                post.ImageUrl = hostpath + post.ImageUrl;
                post.ThumbnailUrl = hostpath + post.ThumbnailUrl;
            }
            return new ResultWithMessage
            {
                Success = true,
                Result = model
            };
        }

        public async Task<ResultWithMessage> DeletePost(int id)
        {
            var post = _db.Posts.FirstOrDefault(x => x.Id == id);
            if (post == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "No Post Found !!!",
                    MessageEnglish = "No Post Found !!!",
                    MessageArabic = "لم يتم العثور على المنشور !!!",
                };
            }
            if (!string.IsNullOrEmpty(post.ImageUrl))
            {
                var deletedimage = await _fileService.DeleteFile(post.ImageUrl); 
                var deletedThumbnail = await _fileService.DeleteFile(post.ThumbnailUrl);

            }

            _db.Posts.Remove(post);
            var posts = _db.Posts.Where(x => x.Order > post.Order).ToList();
            foreach (var p in posts)
            {
                p.Order = p.Order - 1;
            }
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = post };
        }

        public async Task<ResultWithMessage> UploadPostImage(ImageModel model)
        {
            var post = _db.Posts.FirstOrDefault(x => x.Id == model.Id);
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            if (post == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "No Post Found !!!",
                    MessageEnglish = "No Post Found !!!",
                    MessageArabic = "لم يتم العثور على المنشور !!!",
                };
            }
            var uploadResult = await _fileService.UploadPageImage(model, "pages/posts");
            if (!uploadResult.Success)
            {
                return new ResultWithMessage { Success = false, Message = $@"Upload Image Failed !!!" };
            }
            post.ImageUrl = uploadResult.MessageEnglish;
            post.ThumbnailUrl = uploadResult.MessageArabic;
            _db.Entry(post).State = EntityState.Modified;
            _db.SaveChanges();
            var result = new { ImageUrl = hostpath + uploadResult.Message };
            return new ResultWithMessage { Success = true, Result = result };
        }

        public ResultWithMessage ReorderPost(int pageId, int previousIndex, int currentIndex)
        {
            var page = _db.Pages.Include(x => x.Posts).ThenInclude(x => x.Sections).FirstOrDefault(x => x.Id == pageId);

            if (page == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "No Page Found !!!",
                    MessageEnglish = "No Page Found !!!",
                    MessageArabic = "لم يتم العثور على الصفحة !!!",
                };
            }
            var post = page.Posts.FirstOrDefault(x => x.Order == previousIndex);
            if (post == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "No Post Found !!!",
                    MessageEnglish = "No Post Found !!!",
                    MessageArabic = "لم يتم العثور على المنشور !!!",
                };
            }
            foreach (var currentpost in page.Posts.Where(x => x.Order > post.Order))
            {
                currentpost.Order = currentpost.Order - 1;
            }

            foreach (var currentpost in page.Posts.Where(x => x.Order >= currentIndex && x.Id != post.Id))
            {
                currentpost.Order = currentpost.Order + 1;
            }
            post.Order = currentIndex;
            _db.Entry(page).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage
            {
                Success = true,
                Result = true
            };
        }

        public ResultWithMessage GetSectionById(int id)
        {
            var section = _db.Sections.FirstOrDefault(x => x.Id == id);
            if (section == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "No Section Found !!!",
                    MessageEnglish = "No Section Found !!!",
                    MessageArabic = "لم يتم العثور على القسم !!!",
                };
            }
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            if (section != null && !string.IsNullOrEmpty(section.ImageUrl))
            {
                section.ImageUrl = hostpath + section.ImageUrl;
                section.ThumbnailUrl = hostpath + section.ThumbnailUrl;
            }
            return new ResultWithMessage { Success = true, Result = section };
        }

        public async Task<ResultWithMessage> AddSectionAsync(SectionBindingModel model)
        {
            var newsection = new Section(model);
            var max = _db.Sections.Where(x => x.PostId == model.PostId).Max(x => x.Order) ?? -1;
            newsection.Order = max + 1;
            await _db.Sections.AddAsync(newsection);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = newsection };
        }

        public async Task<ResultWithMessage> AddSectionWithImageAsync(SectionWithImageBindingModel model)
        {
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var newsection = new Section(model);
            var max = _db.Sections.Where(x => x.PostId == model.PostId).Max(x => x.Order) ?? -1;
            newsection.Order = max + 1;
            var uploadResult = await _fileService.UploadImage(model.Image, "pages/posts/sections");
            if (!uploadResult.Success)
            {
                return new ResultWithMessage { Success = false, Message = $@"Upload Image Failed !!!" };
            }
            newsection.ImageUrl = uploadResult.MessageEnglish;
            newsection.ThumbnailUrl = uploadResult.MessageArabic;
            await _db.Sections.AddAsync(newsection);
            _db.SaveChanges();
            newsection.ImageUrl = hostpath + newsection.ImageUrl;
            newsection.ThumbnailUrl = hostpath + newsection.ThumbnailUrl;
            return new ResultWithMessage { Success = true, Result = newsection };
        }

        public ResultWithMessage PutSection(int id, SectionBindingModel model)
        {
            if (id != model.Id)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "Invalid Model !!!",
                    MessageEnglish = "Invalid Model !!!",
                    MessageArabic = "نموزج غير صالح !!!",
                };
            }
            var section = _db.Sections.FirstOrDefault(x => x.Id == model.Id);
            if (section == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "No Section Found !!!",
                    MessageEnglish = "No Section Found !!!",
                    MessageArabic = "لم يتم العثور على القسم !!!",
                };
            }
            section.TitleArabic = model.TitleArabic;
            section.TitleEnglish = model.TitleEnglish;
            section.SubtitleArabic = model.SubtitleArabic;
            section.SubtitleEnglish = model.SubtitleEnglish;
            section.BodyArabic = model.BodyArabic;
            section.BodyEnglish = model.BodyEnglish;
            section.PublishDate = model.PublishDate;
            section.IsPublished = model.IsPublished;
            //section.Order = model.Order;
            _db.Entry(section).State = EntityState.Modified;
            _db.SaveChanges();
            if (!string.IsNullOrEmpty(section.ImageUrl))
            {
                var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                section.ImageUrl = hostpath + section.ImageUrl;
                section.ThumbnailUrl = hostpath + section.ThumbnailUrl;
            }
            return new ResultWithMessage
            {
                Success = true,
                Result = model
            };
        }

        public async Task<ResultWithMessage> DeleteSection(int id)
        {
            var section = _db.Sections.FirstOrDefault(x => x.Id == id);
            if (section == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "No Section Found !!!",
                    MessageEnglish = "No Section Found !!!",
                    MessageArabic = "لم يتم العثور على القسم !!!",
                };
            }
            if (!string.IsNullOrEmpty(section.ImageUrl))
            {
                var deletedimage = await _fileService.DeleteFile(section.ImageUrl);
                var deletedThumbnail = await _fileService.DeleteFile(section.ThumbnailUrl);
                
            }
            _db.Sections.Remove(section);
            var sections = _db.Sections.Where(x => x.Order > section.Order).ToList();
            foreach (var s in sections)
            {
                s.Order = s.Order - 1;
            }
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = section };
        }

        public async Task<ResultWithMessage> UploadSectionImage(ImageModel model)
        {
            var section = _db.Sections.FirstOrDefault(x => x.Id == model.Id);
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            if (section == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "No Section Found !!!",
                    MessageEnglish = "No Section Found !!!",
                    MessageArabic = "لم يتم العثور على القسم !!!",
                };
            }
            var uploadResult = await _fileService.UploadPageImage(model, "pages/posts/sections");
            if (!uploadResult.Success)
            {
                return new ResultWithMessage { Success = false, Message = $@"Upload Image Failed !!!" };
            }
            section.ImageUrl = uploadResult.MessageEnglish;
            section.ThumbnailUrl = uploadResult.MessageArabic;
            _db.Entry(section).State = EntityState.Modified;
            _db.SaveChanges();
            var result = new { ImageUrl = hostpath + uploadResult.Message };
            return new ResultWithMessage { Success = true, Result = result };
        }

        public ResultWithMessage ReorderSection(int postId, int previousIndex, int currentIndex)
        {
            var post = _db.Posts.Include(x => x.Sections).FirstOrDefault(x => x.Id == postId);

            if (post == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "No Post Found !!!",
                    MessageEnglish = "No Post Found !!!",
                    MessageArabic = "لم يتم العثور على المنشور !!!",
                };
            }
            var section = post.Sections.FirstOrDefault(x => x.Order == previousIndex);
            if (section == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = "No Section Found !!!",
                    MessageEnglish = "No Section Found !!!",
                    MessageArabic = "لم يتم العثور على القسم !!!",
                };
            }
            foreach (var currentpost in post.Sections.Where(x => x.Order > section.Order))
            {
                currentpost.Order = currentpost.Order - 1;
            }

            foreach (var currentpost in post.Sections.Where(x => x.Order >= currentIndex && x.Id != section.Id ))
            {
                currentpost.Order = currentpost.Order + 1;
            }
            section.Order = currentIndex;
            _db.Entry(post).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage
            {
                Success = true,
                Result = true
            };

        }

        public List<PostCategory> GetAllPostCategories()
        {
            var list = _db.PostCategories?.Where(x => x.IsDeleted == false).ToList();
            return list ?? new List<PostCategory>();
        }
        
        public async Task<ResultWithMessage> AddPostCategoryAsync(PostCategory model)
        {
            var postcategory = _db.PostCategories.FirstOrDefault(x => (x.NameArabic == model.NameArabic)
                                  || (x.NameEnglish == model.NameEnglish));
            if (postcategory != null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Post Category {model.NameEnglish} Already Exist !!!",
                    MessageEnglish = $@"Post Category{model.NameEnglish} Already Exist !!!",
                    MessageArabic = $@"التصنيف {model.NameArabic} موجود مسبقاً !!!"
                };
            }
            await _db.PostCategories.AddAsync(model);
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = model };
        }
        
        public ResultWithMessage PutPostCategory(int id, PostCategory model)
        {
            if (id != model.Id)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Invalid Model !!!",
                    MessageEnglish = $@"Invalid Model !!!",
                    MessageArabic = "نموذج غير صالح !!!"
                };
            }
            var postcategory = _db.PostCategories?.FirstOrDefault(x => x.Id == model.Id);
            _db.Entry(postcategory).State = EntityState.Detached;
            if (postcategory == null)
            {
                return new ResultWithMessage
                {
                    Success = false,
                    Message = $@"Office Entity Not Found !!!",
                    MessageEnglish = $@"Office Entity Not Found !!!",
                    MessageArabic = "التصنيف غير موجود !!!"
                };
            }
            postcategory = model;
            _db.Entry(postcategory).State = EntityState.Modified;
            _db.SaveChanges();
            return new ResultWithMessage { Success = true, Result = postcategory };

        }
        
        public ResultWithMessage GetPostsByFilter(FilterModel model)
        {
            var list = new List<PostViewModel>();
            var posts = _db.Posts?.Include(x => x.Page)
                                        .Include(x => x.Category)
                                        .Include(x => x.Sections)
                                        .Where(x => x.IsPublished && !x.IsDeleted).ToList();

            if (!string.IsNullOrEmpty(model.SearchQuery))
            {
                if (int.TryParse(model.SearchQuery, out int pageid))
                {
                    posts = posts?.Where(x => x.PageId == pageid).ToList();
                }
                else
                {
                    posts = posts?.Where(x => x.TitleArabic.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                 x.TitleEnglish.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                 x.Page.TitleArabic.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                 x.Page.TitleEnglish.ToLower().Contains(model.SearchQuery.ToLower()) ||

                                                 (x.SubtitleArabic != null &&
                                                                    (x.SubtitleArabic.ToLower().Contains(model.SearchQuery.ToLower()))) ||
                                                 (x.SubtitleEnglish != null &&
                                                                    (x.SubtitleEnglish.ToLower().Contains(model.SearchQuery.ToLower()))) ||
                                                 (x.BodyArabic != null &&
                                                                    (x.BodyArabic.ToLower().Contains(model.SearchQuery.ToLower()))) ||
                                                 (x.BodyEnglish != null &&
                                                                    (x.BodyEnglish.ToLower().Contains(model.SearchQuery.ToLower()))) ||


                                                 (x.Page.SubtitleArabic != null &&
                                                                    (x.Page.SubtitleArabic.ToLower().Contains(model.SearchQuery.ToLower()))) ||
                                                 (x.Page.SubtitleEnglish != null &&
                                                                    (x.Page.SubtitleEnglish.ToLower().Contains(model.SearchQuery.ToLower()))) ||
                                                 (x.Page.BodyArabic != null &&
                                                                    (x.Page.BodyArabic.ToLower().Contains(model.SearchQuery.ToLower()))) ||
                                                 (x.Page.BodyArabic != null &&
                                                                    (x.Page.BodyArabic.ToLower().Contains(model.SearchQuery.ToLower()))) ||
                                                 (x.Category != null &&
                                                                    (x.Category.NameArabic.ToLower().Contains(model.SearchQuery.ToLower()) ||
                                                                     x.Category.NameEnglish.ToLower().Contains(model.SearchQuery.ToLower())))).ToList();
                }
            }

            var dataSize = posts.Count();
            var sortProperty = typeof(PostViewModel).GetProperty(model?.Sort ?? "Id");
            if (model?.Order == "desc")
            {
                list = posts?.Select(o => new PostViewModel(o)).OrderBy(x => x.PageId).ThenByDescending(x => sortProperty.GetValue(x)).ToList();
            }
            else
            {
                list = posts?.Select(o => new PostViewModel(o)).OrderBy(x => x.PageId).OrderBy(x => sortProperty.GetValue(x)).ToList();
            }

            var result = list.Skip(model.PageSize * model.PageIndex).Take(model.PageSize).ToList();
            return new ResultWithMessage
            {
                Success = true,
                Message = "",
                Result = new ObservableData(result, dataSize)
            };
        }

        public ResultWithMessage GetLastNews()
        {
            var hostpath = $@"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var newsPage= _db.Pages.FirstOrDefault(x => x.TitleEnglish.ToLower() == "news");
            var list = new List<PostViewModel>();
            var posts = _db.Posts?.Include(x => x.Page)
                                        .Include(x => x.Category)
                                        .Include(x => x.Sections)
                                        .Where(x => x.IsPublished && !x.IsDeleted && x.PageId == newsPage.Id).ToList();
            list = posts?.Select(o => new PostViewModel(o)).OrderByDescending(x => x.PublishDate).ToList();
            var result = list.Take(6).ToList();
            
            foreach (var item in result)
            {
                if (!string.IsNullOrEmpty(item.ImageUrl))
                {
                    item.ImageUrl = hostpath + item.ImageUrl;
                    item.ThumbnailUrl = hostpath + item.ThumbnailUrl;
                }
            }
            return new ResultWithMessage
            {
                Success = true,
                Result = new ObservableData(result,6)
            };
        }


    }
}
