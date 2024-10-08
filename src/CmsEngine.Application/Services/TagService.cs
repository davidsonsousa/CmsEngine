namespace CmsEngine.Application.Services;

public class TagService : Service, ITagService
{
    public TagService(IUnitOfWork uow, IHttpContextAccessor hca, ILoggerFactory loggerFactory, IMemoryCache memoryCache)
                     : base(uow, hca, loggerFactory, memoryCache)
    {
    }

    public async Task<ReturnValue> Delete(Guid id)
    {
        var item = await unitOfWork.Tags.GetByIdAsync(id);
        Guard.Against.Null(item);

        var returnValue = new ReturnValue($"Tag '{item.Name}' deleted at {DateTime.Now.ToString("T")}.");

        try
        {
            unitOfWork.Tags.Delete(item);
            await unitOfWork.Save();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            returnValue.SetErrorMessage("An error has occurred while deleting the tag");
        }

        return returnValue;
    }

    public async Task<ReturnValue> DeleteRange(Guid[] ids)
    {
        var items = await unitOfWork.Tags.GetByMultipleIdsAsync(ids);

        var returnValue = new ReturnValue($"Tags deleted at {DateTime.Now.ToString("T")}.");

        try
        {
            unitOfWork.Tags.DeleteRange(items);
            await unitOfWork.Save();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            returnValue.SetErrorMessage("An error has occurred while deleting the tags");
        }

        return returnValue;
    }

    public IEnumerable<Tag> FilterForDataTable(string searchValue, IEnumerable<Tag> items)
    {
        if (!string.IsNullOrWhiteSpace(searchValue))
        {
            var searchableProperties = typeof(TagTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));
            var searchExpression = items.GetSearchExpression(searchValue, searchableProperties);
            Guard.Against.Null(searchExpression);

            items = items.Where(searchExpression.Compile());
        }
        return items;
    }

    public async Task<(IEnumerable<TagTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters)
    {
        var items = await unitOfWork.Tags.GetAllAsync();
        int recordsTotal = items.Count();
        if (!string.IsNullOrWhiteSpace(parameters.Search?.Value))
        {
            items = FilterForDataTable(parameters.Search.Value, items);
        }

        Guard.Against.Null(parameters.Order);
        items = OrderForDataTable(parameters.Order[0].Column, parameters.Order[0].Dir, items);
        return (items.MapToTableViewModel(), recordsTotal, items.Count());
    }

    public async Task<int> GetTagCount()
    {
        logger.LogDebug("TagService > GetTagCount()");
        var items = await unitOfWork.Tags.CountAsync();
        logger.LogDebug("Tag count: {0}", items);
        return items;
    }

    public async Task<IEnumerable<TagViewModel>> GetAllTags()
    {
        logger.LogDebug("TagService > GetAllTags()");
        var items = await unitOfWork.Tags.GetAllAsync();
        logger.LogDebug("Tags loaded: {0}", items.Count());
        return items.MapToViewModel();
    }

    public IEnumerable<Tag> OrderForDataTable(int column, string direction, IEnumerable<Tag> items)
    {
        try
        {
            switch (column)
            {
                case 1:
                    items = direction == "asc" ? items.OrderBy(o => o.Name) : items.OrderByDescending(o => o.Name);
                    break;
                case 2:
                    items = direction == "asc" ? items.OrderBy(o => o.Slug) : items.OrderByDescending(o => o.Slug);
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

    public async Task<ReturnValue> Save(TagEditModel tagEditModel)
    {
        logger.LogDebug("CmsService > Save(TagEditModel: {0})", tagEditModel.ToString());

        var returnValue = new ReturnValue($"Tag '{tagEditModel.Name}' saved.");

        try
        {
            if (tagEditModel.IsNew)
            {
                logger.LogDebug("New tag");
                var tag = tagEditModel.MapToModel();
                tag.WebsiteId = Instance.Id;

                await unitOfWork.Tags.Insert(tag);
            }
            else
            {
                logger.LogDebug("Update tag");
                var tag = await unitOfWork.Tags.GetByIdAsync(tagEditModel.VanityId);
                Guard.Against.Null(tag, nameof(tag), $"Tag not found. VanityId: {tagEditModel.VanityId}");

                var tagToUpdate = tagEditModel.MapToModel(tag);
                tagToUpdate.WebsiteId = Instance.Id;

                unitOfWork.Tags.Update(tagToUpdate);
            }

            await unitOfWork.Save();
            logger.LogDebug("Tag saved");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            returnValue.SetErrorMessage("An error has occurred while saving the tag");
        }

        return returnValue;
    }

    public TagEditModel SetupEditModel()
    {
        logger.LogDebug("CmsService > SetupEditModel()");
        return new TagEditModel();
    }

    public async Task<TagEditModel> SetupEditModel(Guid id)
    {
        logger.LogDebug("CmsService > SetupTagEditModel(id: {id})", id);
        var item = await unitOfWork.Tags.GetByIdAsync(id);
        Guard.Against.Null(item, nameof(item), $"Tag not found. Vanity id: {id}");

        logger.LogDebug("Tag: {item}", item.ToString());

        return item.MapToEditModel();
    }
}
