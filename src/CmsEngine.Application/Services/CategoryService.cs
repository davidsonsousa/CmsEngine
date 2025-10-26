namespace CmsEngine.Application.Services;

public class CategoryService : Service, ICategoryService
{
    public CategoryService(IUnitOfWork uow, IHttpContextAccessor hca, ILoggerFactory loggerFactory, ICacheService cacheService)
                          : base(uow, hca, loggerFactory, cacheService)
    {
    }

    public async Task<ReturnValue> Delete(Guid id)
    {
        var item = await unitOfWork.Categories.GetByIdAsync(id);
        Guard.Against.Null(item);

        var returnValue = new ReturnValue($"Category '{item.Name}' deleted at {DateTime.Now:T}.");

        try
        {
            unitOfWork.Categories.Delete(item);
            await unitOfWork.Save();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            returnValue.SetErrorMessage("An error has occurred while deleting the category");
        }

        return returnValue;
    }

    public async Task<ReturnValue> DeleteRange(Guid[] ids)
    {
        var items = await unitOfWork.Categories.GetByMultipleIdsAsync(ids);

        var returnValue = new ReturnValue($"Categories deleted at {DateTime.Now:T}.");

        try
        {
            unitOfWork.Categories.DeleteRange(items);
            await unitOfWork.Save();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            returnValue.SetErrorMessage("An error has occurred while deleting the categories");
        }

        return returnValue;
    }

    public IQueryable<Category> FilterForDataTable(string searchValue, IQueryable<Category> items)
    {
        if (!string.IsNullOrWhiteSpace(searchValue))
        {
            var searchableProperties = typeof(CategoryTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));
            var searchExpression = items.GetSearchExpression(searchValue, searchableProperties);
            Guard.Against.Null(searchExpression);

            items = items.Where(searchExpression);
        }

        return items;
    }

    public async Task<int> GetCategoryCount()
    {
        logger.LogDebug("CategoryService > GetCategoryCount()");
        var items = await unitOfWork.Categories.CountAsync();
        logger.LogDebug("Category count: {0}", items);
        return items;
    }

    public async Task<IEnumerable<CategoryViewModel>> GetCategoriesWithPost()
    {
        logger.LogDebug("CategoryService > GetCategoriesWithPost()");
        var items = await unitOfWork.Categories.GetCategoriesWithPostOrderedByName();
        logger.LogDebug("Categories loaded: {0}", items.Count());
        return items.MapToViewModelWithPost(Instance.DateFormat);
    }

    public async Task<IEnumerable<CategoryViewModel>> GetCategoriesWithPostCount()
    {
        logger.LogDebug("CategoryService > GetCategoriesWithPostCount()");
        var items = await unitOfWork.Categories.GetCategoriesWithPostCountOrderedByName();
        logger.LogDebug("Categories loaded: {0}", items.Count());
        return items.MapToViewModelWithPostCount();
    }

    public (IEnumerable<CategoryTableViewModel> Data, int RecordsTotal, int RecordsFiltered) GetForDataTable(DataParameters parameters)
    {
        var items = unitOfWork.Categories.GetAll();
        var recordsTotal = items.Count();

        if (!string.IsNullOrWhiteSpace(parameters.Search?.Value))
        {
            items = FilterForDataTable(parameters.Search.Value, items);
        }

        Guard.Against.Null(parameters.Order);
        items = OrderForDataTable(parameters.Order[0].Column, parameters.Order[0].Dir, items);

        return (items.MapToTableViewModel(), recordsTotal, items.Count());
    }

    public IQueryable<Category> OrderForDataTable(int column, string direction, IQueryable<Category> items)
    {
        switch (column)
        {
            case 1:
                items = direction == "asc" ? items.OrderBy(o => o.Name) : items.OrderByDescending(o => o.Name);
                break;
            case 2:
                items = direction == "asc" ? items.OrderBy(o => o.Slug) : items.OrderByDescending(o => o.Slug);
                break;
            case 3:
                items = direction == "asc" ? items.OrderBy(o => o.Description) : items.OrderByDescending(o => o.Description);
                break;
            default:
                items = items.OrderBy(o => o.Name);
                break;
        }

        return items;
    }

    public async Task<ReturnValue> Save(CategoryEditModel categoryEditModel)
    {
        logger.LogDebug("CmsService > Save(CategoryEditModel: {0})", categoryEditModel.ToString());

        var returnValue = new ReturnValue($"Category '{categoryEditModel.Name}' saved.");

        try
        {
            if (categoryEditModel.IsNew)
            {
                logger.LogDebug("New category");
                var category = categoryEditModel.MapToModel();
                category.WebsiteId = Instance.Id;

                await unitOfWork.Categories.Insert(category);
            }
            else
            {
                logger.LogDebug("Update category");
                var category = await unitOfWork.Categories.GetByIdAsync(categoryEditModel.VanityId);
                Guard.Against.Null(category, nameof(category), $"Category not found. VanityId: {categoryEditModel.VanityId}");

                var categoryToUpdate = categoryEditModel.MapToModel(category);
                categoryToUpdate.WebsiteId = Instance.Id;

                unitOfWork.Categories.Update(categoryToUpdate);
            }

            await unitOfWork.Save();
            logger.LogDebug("Category saved");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            returnValue.SetErrorMessage("An error has occurred while saving the category");
        }

        return returnValue;
    }

    public CategoryEditModel SetupEditModel()
    {
        logger.LogDebug("CmsService > SetupEditModel()");
        return new CategoryEditModel();
    }

    public async Task<CategoryEditModel> SetupEditModel(Guid id)
    {
        logger.LogDebug("CmsService > SetupCategoryEditModel(id: {0})", id);
        var item = await unitOfWork.Categories.GetByIdAsync(id);
        Guard.Against.Null(item, nameof(item), $"Category not found. Vanity id: {id}");

        logger.LogDebug("Category: {0}", item.ToString());

        return item.MapToEditModel();
    }
}
