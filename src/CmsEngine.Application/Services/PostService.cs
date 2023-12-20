namespace CmsEngine.Application.Services;

public class PostService : Service, IPostService
{
    public PostService(IUnitOfWork uow, IHttpContextAccessor hca, ILoggerFactory loggerFactory, IMemoryCache memoryCache)
                      : base(uow, hca, loggerFactory, memoryCache)
    {
    }

    public async Task<ReturnValue> Delete(Guid id)
    {
        var item = await unitOfWork.Posts.GetByIdAsync(id);
        Guard.Against.Null(item);

        var returnValue = new ReturnValue($"Post '{item.Title}' deleted at {DateTime.Now.ToString("T")}.");

        try
        {
            unitOfWork.Posts.Delete(item);
            await unitOfWork.Save();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            returnValue.SetErrorMessage("An error has occurred while deleting the post");
        }

        return returnValue;
    }

    public async Task<ReturnValue> DeleteRange(Guid[] ids)
    {
        var items = await unitOfWork.Posts.GetByMultipleIdsAsync(ids);

        var returnValue = new ReturnValue($"Posts deleted at {DateTime.Now.ToString("T")}.");

        try
        {
            unitOfWork.Posts.DeleteRange(items);
            await unitOfWork.Save();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            returnValue.SetErrorMessage("An error has occurred while deleting the posts");
        }

        return returnValue;
    }

    public IEnumerable<Post> FilterForDataTable(string searchValue, IEnumerable<Post> items)
    {
        if (!string.IsNullOrWhiteSpace(searchValue))
        {
            var searchableProperties = typeof(PostTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));
            var searchExpression = items.GetSearchExpression(searchValue, searchableProperties);
            Guard.Against.Null(searchExpression);

            items = items.Where(searchExpression.Compile());
        }

        return items;
    }

    public async Task<PostViewModel> GetBySlug(string slug)
    {
        logger.LogDebug("PostService > GetBySlug({slug})", slug);
        var item = await unitOfWork.Posts.GetBySlug(slug);
        Guard.Against.Null(item, nameof(item), $"Post not found. Slug: {slug}");

        return item.MapToViewModel(Instance.DateFormat);
    }

    public async Task<IEnumerable<PostEditModel>> GetPublishedOrderedByDate(int count = 0)
    {
        logger.LogDebug("PostService > GetByStatus(count: {0})", count);
        var items = await unitOfWork.Posts.GetByStatusOrderByDescending(DocumentStatus.Published);
        logger.LogDebug("Posts loaded: {0}", items.Count());

        return items.MapToEditModel();
    }

    public async Task<(IEnumerable<PostTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters)
    {
        var items = await unitOfWork.Posts.GetForDataTable();
        int recordsTotal = items.Count();
        if (!string.IsNullOrWhiteSpace(parameters.Search?.Value))
        {
            items = FilterForDataTable(parameters.Search.Value, items);
        }

        Guard.Against.Null(parameters.Order);
        items = OrderForDataTable(parameters.Order[0].Column, parameters.Order[0].Dir, items);
        return (items.MapToTableViewModel(), recordsTotal, items.Count());
    }

    public async Task<PaginatedList<PostViewModel>> GetPublishedByCategoryForPagination(string categorySlug, int page = 1)
    {
        logger.LogDebug("CmsService > GetPublishedByCategoryForPagination(categorySlug: {0}, page: {1})", categorySlug, page);
        var posts = await unitOfWork.Posts.GetPublishedByCategoryForPagination(categorySlug, page, Instance.ArticleLimit);
        return new PaginatedList<PostViewModel>(posts.Items.MapToViewModelForPartialView(Instance.DateFormat), posts.Count, page, Instance.ArticleLimit);
    }

    public async Task<PaginatedList<PostViewModel>> GetPublishedByTagForPagination(string tagSlug, int page = 1)
    {
        logger.LogDebug("CmsService > GetPublishedByTagForPagination(tagSlug: {0}, page: {1})", tagSlug, page);
        var posts = await unitOfWork.Posts.GetPublishedByTagForPagination(tagSlug, page, Instance.ArticleLimit);
        return new PaginatedList<PostViewModel>(posts.Items.MapToViewModelForPartialViewForTags(Instance.DateFormat), posts.Count, page, Instance.ArticleLimit);
    }

    public async Task<PaginatedList<PostViewModel>> GetPublishedForPagination(int page = 1)
    {
        logger.LogDebug("CmsService > GetPublishedForPagination(page: {0})", page);
        var posts = await unitOfWork.Posts.GetPublishedForPagination(page, Instance.ArticleLimit);
        return new PaginatedList<PostViewModel>(posts.Items.MapToViewModelForPartialView(Instance.DateFormat), posts.Count, page, Instance.ArticleLimit);
    }

    public async Task<IEnumerable<PostViewModel>> GetPublishedLatestPosts(int count)
    {
        logger.LogDebug("CmsService > GetPublishedLatestPosts(count: {0})", count);
        return (await unitOfWork.Posts.GetPublishedLatestPosts(count)).MapToViewModelForPartialView(Instance.DateFormat);
    }

    public async Task<PaginatedList<PostViewModel>> FindPublishedForPaginationOrderByDateDescending(string searchTerm = "", int page = 1)
    {
        logger.LogDebug("CmsService > FindPublishedForPaginationOrderByDateDescending(page: {0}, searchTerm: {1})", page, searchTerm);
        var posts = await unitOfWork.Posts.FindPublishedForPaginationOrderByDateDescending(page, searchTerm, Instance.ArticleLimit);
        return new PaginatedList<PostViewModel>(posts.Items.MapToViewModelForPartialView(Instance.DateFormat), posts.Count, page, Instance.ArticleLimit);
    }

    public IEnumerable<Post> OrderForDataTable(int column, string direction, IEnumerable<Post> items)
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

    public async Task<ReturnValue> Save(PostEditModel postEditModel)
    {
        logger.LogDebug("PostService > Save(PostEditModel: {0})", postEditModel.ToString());
        var returnValue = new ReturnValue($"Post '{postEditModel.Title}' saved.");

        try
        {
            if (postEditModel.IsNew)
            {
                logger.LogDebug("New post");
                var post = postEditModel.MapToModel();
                post.WebsiteId = Instance.Id;

                await PrepareRelatedPropertiesAsync(postEditModel, post);
                await unitOfWork.Posts.Insert(post);
            }
            else
            {
                logger.LogDebug("Update post");
                var post = await unitOfWork.Posts.GetForSavingById(postEditModel.VanityId);
                Guard.Against.Null(post, nameof(post), $"Post not found. VanityId: {postEditModel.VanityId}");

                var postToUpdate = postEditModel.MapToModel(post);
                postToUpdate.WebsiteId = Instance.Id;

                unitOfWork.Posts.RemoveRelatedItems(postToUpdate);
                await PrepareRelatedPropertiesAsync(postEditModel, postToUpdate);
                unitOfWork.Posts.Update(postToUpdate);
            }

            await unitOfWork.Save();
            logger.LogDebug("Post saved");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            returnValue.SetErrorMessage("An error has occurred while saving the post");
        }

        return returnValue;
    }

    public async Task<PostEditModel> SetupEditModel()
    {
        logger.LogDebug("PostService > SetupEditModel()");
        return new PostEditModel
        {
            Categories = (await unitOfWork.Categories.GetAllAsync()).MapToViewModelSimple().PopulateCheckboxList(),
            Tags = (await unitOfWork.Tags.GetAllAsync()).MapToViewModelSimple().PopulateSelectList()
        };
    }

    public async Task<PostEditModel> SetupEditModel(Guid id)
    {
        logger.LogDebug("PostService > SetupPostEditModel(id: {id})", id);
        var item = await unitOfWork.Posts.GetForEditingById(id);
        Guard.Against.Null(item, nameof(item), $"Post not found. Vanity id: {id}");

        logger.LogDebug("Post: {item}", item.ToString());

        var postEditModel = item.MapToEditModel();
        postEditModel.Categories = (await unitOfWork.Categories.GetAllAsync()).MapToViewModelSimple().PopulateCheckboxList(postEditModel.SelectedCategories);
        postEditModel.Tags = (await unitOfWork.Tags.GetAllAsync()).MapToViewModelSimple().PopulateSelectList(postEditModel.SelectedTags);

        return postEditModel;
    }

    private async Task PrepareRelatedPropertiesAsync(PostEditModel postEditModel, Post post)
    {
        post.PostCategories.Clear();
        if (postEditModel.SelectedCategories != null)
        {
            var categoryIds = await unitOfWork.Categories.GetIdsByMultipleGuidsAsync(postEditModel.SelectedCategories.ToList().ConvertAll(Guid.Parse));
            foreach (var categoryId in categoryIds)
            {
                post.PostCategories.Add(new PostCategory
                {
                    PostId = post.Id,
                    CategoryId = categoryId
                });
            }
        }

        post.PostTags.Clear();
        if (postEditModel.SelectedTags != null)
        {
            var tagIds = await unitOfWork.Tags.GetIdsByMultipleGuidsAsync(postEditModel.SelectedTags.ToList().ConvertAll(Guid.Parse));
            foreach (var tagId in tagIds)
            {
                post.PostTags.Add(new PostTag
                {
                    PostId = post.Id,
                    TagId = tagId
                });
            }
        }

        var user = await GetCurrentUserAsync();
        post.PostApplicationUsers.Clear();
        post.PostApplicationUsers.Add(new PostApplicationUser
        {
            PostId = post.Id,
            ApplicationUserId = user.Id
        });
    }
}
