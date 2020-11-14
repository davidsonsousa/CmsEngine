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
                Instance.PageTitle = page == 1
                                        ? $"Blog - {Instance.Name}"
                                        : $"Blog - {Instance.Name} - Page {page}";
            }
            else
            {
                Instance.PageTitle = $"Results for '{q}' - {Instance.Name}";
            }

            return View(Instance);
        }

        public async Task<IActionResult> Post(string slug)
        {
            Instance.SelectedDocument = await _postService.GetBySlug(slug);

            if (Instance.SelectedDocument == null)
            {
                return NotFound();
            }

            Instance.PageTitle = $"{Instance.SelectedDocument.Title} - {Instance.Name}";
            return View(Instance);
        }

        public async Task<IActionResult> Category(string slug, int page = 1)
        {
            Instance.PagedPosts = await _postService.GetPublishedByCategoryForPagination(slug, page);
            string selectedCategory = Instance.PagedPosts.SelectMany(p => p.Categories.Where(c => c.Slug == slug).Select(x => x.Name)).FirstOrDefault();

            if (selectedCategory == null)
            {
                return NotFound();
            }

            Instance.PageTitle = $"{selectedCategory} - {Instance.Name}";
            return View("Index", Instance);
        }

        public async Task<IActionResult> Tag(string slug, int page = 1)
        {
            Instance.PagedPosts = await _postService.GetPublishedByTagForPagination(slug, page);
            string selectedTag = Instance.PagedPosts.SelectMany(p => p.Tags.Where(t => t.Slug == slug).Select(x => x.Name)).FirstOrDefault();

            if (selectedTag == null)
            {
                return NotFound();
            }

            Instance.PageTitle = $"#{selectedTag} - {Instance.Name}";
            return View("Index", Instance);
        }

        public async Task<IActionResult> Feed()
        {
            var feed = await _xmlService.GenerateFeed();
            return Content(feed.ToString(), "text/xml");
        }
    }
}
