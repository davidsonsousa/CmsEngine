namespace CmsEngine.Application.Services;

public class PageService : Service, IPageService
{
    public PageService(IUnitOfWork uow, IHttpContextAccessor hca, ILoggerFactory loggerFactory, ICacheService cacheService)
                      : base(uow, hca, loggerFactory, cacheService)
    {
    }

    public async Task<ReturnValue> Delete(Guid id)
    {
        var item = await unitOfWork.Pages.GetByIdAsync(id);
        Guard.Against.Null(item);

        var returnValue = new ReturnValue($"Page '{item.Title}' deleted at {DateTime.Now:T}.");

        try
        {
            unitOfWork.Pages.Delete(item);
            await unitOfWork.Save();
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
        var items = await unitOfWork.Pages.GetByMultipleIdsAsync(ids);

        var returnValue = new ReturnValue($"Pages deleted at {DateTime.Now:T}.");

        try
        {
            unitOfWork.Pages.DeleteRange(items);
            await unitOfWork.Save();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            returnValue.SetErrorMessage("An error has occurred while deleting the pages");
        }

        return returnValue;
    }

    public IQueryable<Page> FilterForDataTable(string searchValue, IQueryable<Page> items)
    {
        if (!string.IsNullOrWhiteSpace(searchValue))
        {
            var searchableProperties = typeof(PageTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));
            var searchExpression = items.GetSearchExpression(searchValue, searchableProperties);
            Guard.Against.Null(searchExpression);

            items = items.Where(searchExpression);
        }

        return items;
    }

    public async Task<int> GetPageCount()
    {
        logger.LogDebug("PageService > GetPageCount()");
        var items = await unitOfWork.Pages.CountAsync();
        logger.LogDebug("Page count: {0}", items);
        return items;
    }

    public async Task<IEnumerable<PageViewModel>> GetAllPublished()
    {
        logger.LogDebug("PageService > GetPagesByStatusReadOnly()");
        var items = await unitOfWork.Pages.GetByStatusOrderByDescending(DocumentStatus.Published);
        logger.LogDebug("Pages loaded: {0}", items.Count());
        return items.MapToViewModel(Instance.DateFormat);
    }

    public async Task<PageViewModel> GetBySlug(string slug)
    {
        logger.LogDebug($"PageService > GetBySlug({slug})");
        var item = await unitOfWork.Pages.GetBySlug(slug);
        Guard.Against.Null(item);

        return item.MapToViewModel(Instance.DateFormat);
    }

    public (IEnumerable<PageTableViewModel> Data, int RecordsTotal, int RecordsFiltered) GetForDataTable(DataParameters parameters)
    {
        var items = unitOfWork.Pages.GetForDataTable();
        var recordsTotal = items.Count();
        if (!string.IsNullOrWhiteSpace(parameters.Search?.Value))
        {
            items = FilterForDataTable(parameters.Search.Value, items);
        }

        Guard.Against.Null(parameters.Order);
        items = OrderForDataTable(parameters.Order[0].Column, parameters.Order[0].Dir, items);
        return (items.MapToTableViewModel(), recordsTotal, items.Count());
    }

    public IQueryable<Page> OrderForDataTable(int column, string direction, IQueryable<Page> items)
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

        return items;
    }

    public async Task<ReturnValue> Save(PageEditModel pageEditModel)
    {
        logger.LogDebug("PageService > Save(PageEditModel: {0})", pageEditModel.ToString());

        var returnValue = new ReturnValue($"Page '{pageEditModel.Title}' saved.");

        try
        {
            if (pageEditModel.IsNew)
            {
                logger.LogDebug("New page");
                var page = pageEditModel.MapToModel();
                page.WebsiteId = Instance.Id;

                await PrepareRelatedPropertiesAsync(page);
                await unitOfWork.Pages.Insert(page);
            }
            else
            {
                logger.LogDebug("Update page");
                var page = await unitOfWork.Pages.GetForSavingById(pageEditModel.VanityId);
                Guard.Against.Null(page, nameof(page), $"Page not found. VanityId: {pageEditModel.VanityId}");

                var pageToUpdate = pageEditModel.MapToModel(page);
                pageToUpdate.WebsiteId = Instance.Id;

                unitOfWork.Pages.RemoveRelatedItems(pageToUpdate);
                await PrepareRelatedPropertiesAsync(pageToUpdate);
                unitOfWork.Pages.Update(pageToUpdate);
            }

            await unitOfWork.Save();
            logger.LogDebug("Page saved");
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
        logger.LogDebug("PageService > SetupEditModel()");
        return new PageEditModel();
    }

    public async Task<PageEditModel> SetupEditModel(Guid id)
    {
        logger.LogDebug("PageService > SetupPageEditModel(id: {id})", id);
        var item = await unitOfWork.Pages.GetByIdAsync(id);
        Guard.Against.Null(item, nameof(item), $"Page not found. Vanity id: {id}");

        logger.LogDebug("Page: {item}", item.ToString());

        return item.MapToEditModel();
    }

    private async Task PrepareRelatedPropertiesAsync(Page page)
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
