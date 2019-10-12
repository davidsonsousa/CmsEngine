using System.Linq;
using System.Threading.Tasks;
using CmsEngine.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Ui.Controllers
{
    public class BlogController : BaseController
    {
        private readonly IPostService _postService;
        private readonly IXmlService _xmlService;

        public BlogController(ILoggerFactory loggerFactory, ICategoryService categoryService, IPageService pageService, IPostService postService,
                              ITagService tagService, IXmlService xmlService, IService service)
                             : base(loggerFactory, service, categoryService, pageService, postService, tagService)
        {
            _postService = postService;
            _xmlService = xmlService;
        }

        public IActionResult Index(int page = 1, string q = "")
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                if (page == 1)
                {
                    instance.PageTitle = $"Blog - {instance.Name}";
                }
                else
                {
                    instance.PageTitle = $"Blog - {instance.Name} - Page {page}";
                }
            }
            else
            {
                instance.PageTitle = $"Results for '{q}' - {instance.Name}";
            }

            return View(instance);
        }

        public async Task<IActionResult> Post(string slug)
        {
            instance.SelectedDocument = await _postService.GetBySlug(slug);

            if (instance.SelectedDocument == null)
            {
                return NotFound();
            }

            instance.PageTitle = $"{instance.SelectedDocument.Title} - {instance.Name}";
            return View(instance);
        }

        public async Task<IActionResult> Category(string slug, int page = 1)
        {
            instance.PagedPosts = await _postService.GetPublishedByCategoryForPagination(slug, page);
            string selectedCategory = instance.PagedPosts.SelectMany(p => p.Categories.Where(c => c.Slug == slug).Select(x => x.Name)).FirstOrDefault();

            if (selectedCategory == null)
            {
                return NotFound();
            }

            instance.PageTitle = $"{selectedCategory} - {instance.Name}";
            return View("Index", instance);
        }

        public async Task<IActionResult> Tag(string slug, int page = 1)
        {
            instance.PagedPosts = await _postService.GetPublishedByTagForPagination(slug, page);
            string selectedTag = instance.PagedPosts.SelectMany(p => p.Tags.Where(t => t.Slug == slug).Select(x => x.Name)).FirstOrDefault();

            if (selectedTag == null)
            {
                return NotFound();
            }

            instance.PageTitle = $"#{selectedTag} - {instance.Name}";
            return View("Index", instance);
        }

        public async Task<IActionResult> Feed()
        {
            var feed = await _xmlService.GenerateFeed();
            return Content(feed.ToString(), "text/xml");
        }
    }
}
