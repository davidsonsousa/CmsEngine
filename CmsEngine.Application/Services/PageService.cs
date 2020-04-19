using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsEngine.Application.Attributes;
using CmsEngine.Application.EditModels;
using CmsEngine.Application.Extensions;
using CmsEngine.Application.Extensions.Mapper;
using CmsEngine.Application.ViewModels;
using CmsEngine.Application.ViewModels.DataTableViewModels;
using CmsEngine.Core;
using CmsEngine.Data;
using CmsEngine.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Application.Services
{
    public class PageService : Service, IPageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PageService(IUnitOfWork uow, IHttpContextAccessor hca, ILoggerFactory loggerFactory, IMemoryCache memoryCache)
                          : base(uow, hca, loggerFactory, memoryCache)
        {
            _unitOfWork = uow;
        }

        public async Task<ReturnValue> Delete(Guid id)
        {
            var item = await _unitOfWork.Pages.GetByIdAsync(id);

            var returnValue = new ReturnValue($"Page '{item.Title}' deleted at {DateTime.Now.ToString("T")}.");

            try
            {
                _unitOfWork.Pages.Delete(item);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                returnValue.SetErrorMessage("An error has occurred while deleting the page");
            }

            return returnValue;
        }

        public async Task<ReturnValue> DeleteRange(Guid[] ids)
        {
            var items = await _unitOfWork.Pages.GetByMultipleIdsAsync(ids);

            var returnValue = new ReturnValue($"Pages deleted at {DateTime.Now.ToString("T")}.");

            try
            {
                _unitOfWork.Pages.DeleteRange(items);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                returnValue.SetErrorMessage("An error has occurred while deleting the pages");
            }

            return returnValue;
        }

        public IEnumerable<Page> FilterForDataTable(string searchValue, IEnumerable<Page> items)
        {
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                var searchableProperties = typeof(PageTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));
                items = items.Where(items.GetSearchExpression(searchValue, searchableProperties).Compile());
            }
            return items;
        }

        public async Task<IEnumerable<PageViewModel>> GetAllPublished()
        {
            logger.LogInformation("PageService > GetPagesByStatusReadOnly()");
            var items = await _unitOfWork.Pages.GetByStatusOrderByDescending(DocumentStatus.Published);
            logger.LogInformation("Pages loaded: {0}", items.Count());
            return items.MapToViewModel();
        }

        public async Task<PageViewModel> GetBySlug(string slug)
        {
            logger.LogInformation($"PageService > GetBySlug({slug})");
            var item = await _unitOfWork.Pages.GetBySlug(slug);
            return item?.MapToViewModel();
        }

        public async Task<(IEnumerable<PageTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters)
        {
            var items = await _unitOfWork.Pages.GetForDataTable();
            int recordsTotal = items.Count();
            if (!string.IsNullOrWhiteSpace(parameters.Search.Value))
            {
                items = FilterForDataTable(parameters.Search.Value, items);
            }
            items = OrderForDataTable(parameters.Order[0].Column, parameters.Order[0].Dir, items);
            return (items.MapToTableViewModel(), recordsTotal, items.Count());
        }

        public IEnumerable<Page> OrderForDataTable(int column, string direction, IEnumerable<Page> items)
        {
            try
            {
                switch (column)
                {
                    case 1:
                        items = direction == "asc" ? items.OrderBy(o => o.Title) : items.OrderByDescending(o => o.Title);
                        break;
                    case 2:
                        items = direction == "asc" ? items.OrderBy(o => o.Description) : items.OrderByDescending(o => o.Description);
                        break;
                    case 3:
                        items = direction == "asc" ? items.OrderBy(o => o.Slug) : items.OrderByDescending(o => o.Slug);
                        break;
                    //case 4:
                    //    items = direction == "asc" ? items.OrderBy(o => o.Author.FullName) : items.OrderByDescending(o => o.Author.FullName);
                    //    break;
                    case 5:
                        items = direction == "asc" ? items.OrderBy(o => o.PublishedOn) : items.OrderByDescending(o => o.PublishedOn);
                        break;
                    case 6:
                        items = direction == "asc" ? items.OrderBy(o => o.Status) : items.OrderByDescending(o => o.Status);
                        break;
                    default:
                        items = items.OrderByDescending(o => o.PublishedOn);
                        break;
                }
            }
            catch
            {
                throw;
            }

            return items;
        }

        public async Task<ReturnValue> Save(PageEditModel pageEditModel)
        {
            logger.LogInformation("PageService > Save(PageEditModel: {0})", pageEditModel.ToString());

            var returnValue = new ReturnValue($"Page '{pageEditModel.Title}' saved.");

            try
            {
                if (pageEditModel.IsNew)
                {
                    logger.LogInformation("New page");
                    var page = pageEditModel.MapToModel();
                    page.WebsiteId = Instance.Id;

                    await PrepareRelatedProperties(page);
                    await unitOfWork.Pages.Insert(page);
                }
                else
                {
                    logger.LogInformation("Update page");
                    var page = pageEditModel.MapToModel(await unitOfWork.Pages.GetForSavingById(pageEditModel.VanityId));
                    page.WebsiteId = Instance.Id;

                    _unitOfWork.Pages.RemoveRelatedItems(page);
                    await PrepareRelatedProperties(page);
                    _unitOfWork.Pages.Update(page);
                }

                await _unitOfWork.Save();
                logger.LogInformation("Page saved");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                returnValue.SetErrorMessage("An error has occurred while saving the page");
            }

            return returnValue;
        }

        public PageEditModel SetupEditModel()
        {
            logger.LogInformation("PageService > SetupEditModel()");
            return new PageEditModel();
        }

        public async Task<PageEditModel> SetupEditModel(Guid id)
        {
            logger.LogInformation("PageService > SetupPageEditModel(id: {0})", id);
            var item = await _unitOfWork.Pages.GetByIdAsync(id);
            logger.LogInformation("Page: {0}", item.ToString());
            return item?.MapToEditModel();
        }

        private async Task PrepareRelatedProperties(Page page)
        {
            var user = await GetCurrentUserAsync();
            page.PageApplicationUsers.Clear();
            page.PageApplicationUsers.Add(new PageApplicationUser
            {
                PageId = page.Id,
                ApplicationUserId = user.Id
            });
        }
    }
}
