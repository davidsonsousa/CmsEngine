namespace CmsEngine.Application.Services;

public class WebsiteService : Service, IWebsiteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _memoryCache;

    public WebsiteService(IUnitOfWork uow, IHttpContextAccessor hca, ILoggerFactory loggerFactory, IMemoryCache memoryCache)
                         : base(uow, hca, loggerFactory, memoryCache)
    {
        _unitOfWork = uow;
        _memoryCache = memoryCache;
    }

    public async Task<ReturnValue> Delete(Guid id)
    {
        var item = await _unitOfWork.Websites.GetByIdAsync(id);
        Guard.Against.Null(item);

        var returnValue = new ReturnValue($"Website '{item.Name}' deleted at {DateTime.Now:T}.");

        try
        {
            _unitOfWork.Websites.Delete(item);
            await _unitOfWork.Save();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            returnValue.SetErrorMessage("An error has occurred while deleting the website");
        }

        return returnValue;
    }

    public async Task<ReturnValue> DeleteRange(Guid[] ids)
    {
        var items = await _unitOfWork.Websites.GetByMultipleIdsAsync(ids);

        var returnValue = new ReturnValue($"Websites deleted at {DateTime.Now:T}.");

        try
        {
            _unitOfWork.Websites.DeleteRange(items);
            await _unitOfWork.Save();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            returnValue.SetErrorMessage("An error has occurred while deleting the websites");
        }

        return returnValue;
    }

    public IEnumerable<Website> FilterForDataTable(string searchValue, IEnumerable<Website> items)
    {
        if (!string.IsNullOrWhiteSpace(searchValue))
        {
            var searchableProperties = typeof(WebsiteTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));
            var searchExpression = items.GetSearchExpression(searchValue, searchableProperties);
            Guard.Against.Null(searchExpression);

            items = items.Where(searchExpression.Compile());
        }

        return items;
    }

    public async Task<(IEnumerable<WebsiteTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters)
    {
        var items = await _unitOfWork.Websites.GetForDataTable();
        var recordsTotal = items.Count();
        if (!string.IsNullOrWhiteSpace(parameters.Search?.Value))
        {
            items = FilterForDataTable(parameters.Search.Value, items);
        }

        Guard.Against.Null(parameters.Order);
        items = OrderForDataTable(parameters.Order[0].Column, parameters.Order[0].Dir, items);
        return (items.MapToTableViewModel(), recordsTotal, items.Count());
    }

    public IEnumerable<Website> OrderForDataTable(int column, string direction, IEnumerable<Website> items)
    {
        try
        {
            switch (column)
            {
                case 1:
                    items = direction == "asc" ? items.OrderBy(o => o.Name) : items.OrderByDescending(o => o.Name);
                    break;
                case 2:
                    items = direction == "asc" ? items.OrderBy(o => o.Tagline) : items.OrderByDescending(o => o.Tagline);
                    break;
                case 3:
                    items = direction == "asc" ? items.OrderBy(o => o.Culture) : items.OrderByDescending(o => o.Culture);
                    break;
                default:
                    items = items.OrderBy(o => o.Name);
                    break;
            }
        }
        catch
        {
            throw;
        }

        return items;
    }

    public async Task<ReturnValue> Save(WebsiteEditModel websiteEditModel)
    {
        logger.LogDebug("CmsService > Save(WebsiteEditModel: {websiteEditModel})", websiteEditModel.ToString());

        var returnValue = new ReturnValue($"Website '{websiteEditModel.Name}' saved.");

        try
        {
            if (websiteEditModel.IsNew)
            {
                logger.LogDebug("New website");
                var website = websiteEditModel.MapToModel();

                await unitOfWork.Websites.Insert(website);
            }
            else
            {
                logger.LogDebug("Update website");
                var website = await unitOfWork.Websites.GetByIdAsync(websiteEditModel.VanityId);
                Guard.Against.Null(website, nameof(website), $"Website not found. VanityId: {websiteEditModel.VanityId}");

                var websiteToUpdate = websiteEditModel.MapToModel(website);

                _unitOfWork.Websites.Update(websiteToUpdate);
            }

            await _unitOfWork.Save();

            SaveInstanceToCache(websiteEditModel);
            logger.LogDebug("Website saved");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            returnValue.SetErrorMessage("An error has occurred while saving the website");
        }

        return returnValue;
    }

    public WebsiteEditModel SetupEditModel()
    {
        logger.LogDebug("CmsService > SetupEditModel()");
        return new WebsiteEditModel();
    }

    public async Task<WebsiteEditModel?> SetupEditModel(Guid id)
    {
        logger.LogDebug("CmsService > SetupEditModel(id: {id})", id);
        var item = await _unitOfWork.Websites.GetByIdAsync(id);
        logger.LogDebug("Website: {item}", item?.ToString());
        return item?.MapToEditModel();
    }
}
