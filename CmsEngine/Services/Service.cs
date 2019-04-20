using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using CmsEngine.Attributes;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Data.ViewModels.DataTableViewModels;
using CmsEngine.Extensions;
using CmsEngine.Helpers;
using CmsEngine.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using static System.Web.HttpUtility;

namespace CmsEngine
{
    public sealed partial class CmsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        private InstanceViewModel _instance;

        public UserViewModel CurrentUser
        {
            get
            {
                var task = Task.Run(() => GetUserByUsername(_httpContextAccessor.HttpContext.User.Identity.Name));
                return (UserViewModel)task.Result;
            }
        }

        public InstanceViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    try
                    {
                        var website = _unitOfWork.Websites.Get(q => q.SiteUrl == _httpContextAccessor.HttpContext.Request.Host.Host).SingleOrDefault();
                        if (website != null)
                        {
                            _instance = new InstanceViewModel
                            {
                                Id = website.Id,
                                Name = website.Name,
                                Description = website.Description,
                                Tagline = website.Tagline,
                                HeaderImage = website.HeaderImage,
                                Culture = website.Culture,
                                UrlFormat = website.UrlFormat,
                                DateFormat = website.DateFormat,
                                SiteUrl = website.SiteUrl,
                                ArticleLimit = website.ArticleLimit,
                                PageTitle = website.Name,
                                ContactDetails = new ContactDetailsViewModel
                                {
                                    Address = website.Address,
                                    Phone = website.Phone,
                                    Email = website.Email,
                                },
                                ApiDetails = new ApiDetailsViewModel
                                {
                                    FacebookAppId = website.FacebookAppId,
                                    FacebookApiVersion = website.FacebookApiVersion,
                                    DisqusShortName = website.DisqusShortName
                                },
                                SocialMedia = new SocialMediaViewModel
                                {
                                    Facebook = website.Facebook,
                                    Twitter = website.Twitter,
                                    Instagram = website.Instagram,
                                    LinkedIn = website.LinkedIn
                                }
                            };
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }

                return _instance;
            }
        }

        public CmsService(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca, UserManager<ApplicationUser> userManager, ILogger logger)
        {
            _unitOfWork = uow;
            _mapper = mapper;
            _httpContextAccessor = hca;
            _userManager = userManager;
            _logger = logger;
        }

        private IQueryable<TModel> GetDocumentsByStatus<TModel>(DocumentStatus documentStatus, int count = 0) where TModel : Document
        {
            try
            {
                var query = _unitOfWork.GetRepository<TModel>()
                                  .Get(q => q.IsDeleted == false && q.Status == documentStatus)
                                  .OrderByDescending(o => o.PublishedOn)
                                  .AsQueryable();

                if (count > 0)
                {
                    query = query.Take(count);
                }

                return query;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<IViewModel> GetAllReadOnly<TModel, TViewModel>() where TModel : BaseModel where TViewModel : BaseViewModel
        {
            IEnumerable<TModel> listItems;

            try
            {
                listItems = _unitOfWork.GetRepository<TModel>().GetReadOnly(q => q.IsDeleted == false);
            }
            catch
            {
                throw;
            }

            return _mapper.Map<IEnumerable<TModel>, IEnumerable<TViewModel>>(listItems);
        }

        public int CountRecords<T>() where T : BaseModel
        {
            try
            {
                return _unitOfWork.GetRepository<T>().GetReadOnly(q => q.IsDeleted == false).Count();
            }
            catch
            {
                throw;
            }
        }

        public ReturnValue BulkDelete<T>(Guid[] id) where T : BaseModel
        {
            var returnValue = new ReturnValue();
            try
            {
                var itemsToDelete = _unitOfWork.GetRepository<T>().GetReadOnly(q => id.Contains(q.VanityId));
                _unitOfWork.GetRepositoryMany<T>().UpdateMany(itemsToDelete.Select(x =>
                                                                            {
                                                                                x.IsDeleted = true;
                                                                                return x;
                                                                            }));

                _unitOfWork.Save();

                returnValue.IsError = false;
                returnValue.Message = $"Selected items deleted at {DateTime.Now.ToString("T")}.";
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the selected items";
                throw;
            }

            return returnValue;
        }

        public TableViewModel BuildDataTable<T>(IEnumerable<IViewModel> listItems, int start, int size) where T : BaseModel
        {
            var listString = new List<List<string>>();

            foreach (var item in listItems.Skip(start).Take(size))
            {
                // Get the properties which should appear in the DataTable
                var itemProperties = item.GetType()
                                         .GetProperties()
                                         .Where(p => Attribute.IsDefined(p, typeof(ShowOnDataTable)))
                                         .OrderBy(o => o.GetCustomAttributes(false).OfType<ShowOnDataTable>().First().Order);

                // An empty value must *always* be the first property because of the checkboxes
                var listPropertes = new List<string> { string.Empty };

                // Loop through and add the properties found
                foreach (var property in itemProperties)
                {
                    listPropertes.Add(PrepareProperty(item, property));
                }

                // VanityId must *always* be the last property
                listPropertes.Add(item.VanityId.ToString());

                listString.Add(listPropertes);
            }

            return new TableViewModel
            {
                Data = listString,
                RecordsTotal = this.CountRecords<T>(),
                RecordsFiltered = listItems.Count(),
                Draw = 0
            };
        }

        public XDocument GenerateFeed()
        {
            var articleList = new List<XElement>();

            foreach (var item in _unitOfWork.Posts.Get().OrderByDescending(o => o.PublishedOn))
            {
                string url = FormatUrl("feed", item.Slug);
                articleList.Add(new XElement("item",
                                          new XElement("title", item.Title),
                                          new XElement("link", url),
                                          new XElement("description", item.DocumentContent),
                                          new XElement("pubDate", item.PublishedOn.ToString("r")),
                                          new XElement("guid", url)));
            }

            return new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("rss",
                             new XElement("channel",
                                          new XElement("title", Instance.Name),
                                          new XElement("link", FormatUrl(string.Empty)),
                                          new XElement("description", Instance.Description),
                                          new XElement("language", Instance.Culture.ToLowerInvariant()),
                                          new XElement("generator", "MultiCMS"),
                                          articleList
                                          ),
                             new XAttribute("version", "2.0")));
        }

        public XDocument GenerateSitemap()
        {
            var items = new List<SitemapViewModel>();

            items.AddRange(_unitOfWork.Posts
                                      .Get()
                                      .OrderByDescending(o => o.PublishedOn)
                                      .Select(x => new SitemapViewModel
                                      {
                                          Url = FormatUrl("post", x.Slug),
                                          PublishedOn = x.PublishedOn.ToString("yyyy-MM-dd")
                                      }));

            items.AddRange(_unitOfWork.Pages
                                      .Get()
                                      .OrderByDescending(o => o.PublishedOn)
                                      .Select(x => new SitemapViewModel
                                      {
                                          Url = FormatUrl("post", x.Slug),
                                          PublishedOn = x.PublishedOn.ToString("yyyy-MM-dd")
                                      }));

            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            return new XDocument(new XDeclaration("1.0", "utf-8", null),
                                         new XElement(ns + "urlset",
                                                      from item in items
                                                      select new XElement(ns + "url",
                                                                new XElement(ns + "loc", item.Url),
                                                                new XElement(ns + "lastmod", item.PublishedOn),
                                                                new XElement(ns + "changefreq", "monthly")
                                                             )));

        }

        private IEnumerable<CheckboxEditModel> PopulateCheckboxList<T>(IEnumerable<string> selectedItems = null) where T : BaseModel
        {
            var checkBoxList = _unitOfWork.GetRepository<T>()
                                          .GetReadOnly(q => q.IsDeleted == false)
                                          .Select(x => new CheckboxEditModel
                                          {
                                              Label = x.GetType().GetProperty("Name").GetValue(x).ToString(),
                                              Value = x.VanityId.ToString(),
                                              Enabled = true,
                                              Selected = (selectedItems?.Contains(x.VanityId.ToString()) ?? false)
                                          });

            return checkBoxList.OrderBy(o => o.Label);
        }

        private IEnumerable<SelectListItem> PopulateSelectListItems<T>(IEnumerable<string> selectedItems = null) where T : BaseModel
        {
            var selectListItems = _unitOfWork.GetRepository<T>()
                                      .GetReadOnly(q => q.IsDeleted == false)
                                      .Select(x => new SelectListItem
                                      {
                                          Text = x.GetType().GetProperty("Name").GetValue(x).ToString(),
                                          Value = x.VanityId.ToString(),
                                          Disabled = false,
                                          Selected = (selectedItems?.Contains(x.VanityId.ToString()) ?? false)
                                      });

            return selectListItems.OrderBy(o => o.Text);
        }

        private IEnumerable<T> GetAllReadOnly<T>(int count = 0) where T : BaseModel
        {
            IEnumerable<T> listItems;

            try
            {
                var query = _unitOfWork.GetRepository<T>().Get(q => q.IsDeleted == false);

                if (count > 0)
                {
                    query = query.Take(count);
                }

                listItems = query.ToList();
            }
            catch
            {
                throw;
            }

            return listItems;
        }

        private ReturnValue Delete<T>(T item) where T : BaseModel
        {
            _logger.LogInformation("CmsService > Delete<{0}>({1})", item.GetType().BaseType.Name, item.ToString());

            var returnValue = new ReturnValue();
            try
            {
                if (item != null)
                {
                    item.IsDeleted = true;
                    _unitOfWork.GetRepository<T>().Update(item);
                }

                _unitOfWork.Save();
                returnValue.IsError = false;

                _logger.LogInformation("Item marked as deleted");
            }
            catch
            {
                _logger.LogError("Error when marking item as deleted");
                returnValue.IsError = true;
                throw;
            }

            return returnValue;
        }

        private string PrepareProperty(IViewModel item, PropertyInfo property)
        {
            string propertyValue;

            switch (property.PropertyType.Name)
            {
                case "DocumentStatus":
                    GeneralStatus generalStatus;
                    string documentStatus = item.GetType().GetProperty(property.Name).GetValue(item)?.ToString() ?? "";
                    switch (documentStatus)
                    {
                        case "Published":
                            generalStatus = GeneralStatus.Success;
                            break;
                        case "PendingApproval":
                            generalStatus = GeneralStatus.Warning;
                            break;
                        default:
                            generalStatus = GeneralStatus.Info;
                            break;
                    }

                    propertyValue = $"<span class=\"label label-{generalStatus.ToString().ToLowerInvariant()}\">{documentStatus.ToEnum<DocumentStatus>().GetName()}</status-label>";
                    break;
                case "UserViewModel":
                    var author = ((UserViewModel)item.GetType().GetProperty(property.Name).GetValue(item));
                    propertyValue = HtmlEncode(author?.FullName) ?? "";
                    break;
                default:
                    propertyValue = HtmlEncode(item.GetType().GetProperty(property.Name).GetValue(item)?.ToString()) ?? "";
                    break;
            }

            return propertyValue;
        }

        private Func<T, bool> PrepareFilter<T>(string searchTerm, IEnumerable<PropertyInfo> searchableProperties)
        {
            var expressionFilter = new List<ExpressionFilter>();

            foreach (var property in searchableProperties)
            {
                expressionFilter.Add(new ExpressionFilter
                {
                    PropertyName = property.Name,
                    Operation = Operations.Contains,
                    Value = searchTerm
                });
            }

            return ExpressionBuilder.GetExpression<T>(expressionFilter, LogicalOperators.Or).Compile();
        }

        private string FormatUrl(string type, string slug = "")
        {
            string url = "";

            if (!string.IsNullOrWhiteSpace(Instance.UrlFormat))
            {
                url = Instance.UrlFormat.Replace("[site_url]", Instance.SiteUrl)
                                                  .Replace("[culture]", Instance.Culture)
                                                  .Replace("[short_culture]", Instance.Culture.Substring(0, 2))
                                                  .Replace("[type]", type)
                                                  .Replace("[slug]", slug);
            }

            url = url.EndsWith("/") ? url.Substring(0, url.LastIndexOf('/')) : url;

            return url;
        }
    }
}
