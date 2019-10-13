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
    public class PostService : Service, IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork uow, IHttpContextAccessor hca, ILoggerFactory loggerFactory, IMemoryCache memoryCache)
                          : base(uow, hca, loggerFactory, memoryCache)
        {
            _unitOfWork = uow;
        }

        public async Task<ReturnValue> Delete(Guid id)
        {
            var item = await _unitOfWork.Posts.GetByIdAsync(id);

            var returnValue = new ReturnValue($"Post '{item.Title}' deleted at {DateTime.Now.ToString("T")}.");

            try
            {
                _unitOfWork.Posts.Delete(item);
                await _unitOfWork.Save();
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
            var items = await _unitOfWork.Posts.GetByMultipleIdsAsync(ids);

            var returnValue = new ReturnValue($"Posts deleted at {DateTime.Now.ToString("T")}.");

            try
            {
                _unitOfWork.Posts.DeleteRange(items);
                await _unitOfWork.Save();
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
                items = items.Where(items.GetSearchExpression(searchValue, searchableProperties).Compile());
            }
            return items;
        }

        public async Task<PostViewModel> GetBySlug(string slug)
        {
            logger.LogInformation($"PostService > GetBySlug({slug})");
            var item = await _unitOfWork.Posts.GetBySlug(slug);
            return item.MapToViewModel();
        }

        public async Task<IEnumerable<PostEditModel>> GetPublishedOrderedByDate(int count = 0)
        {
            logger.LogInformation("PostService > GetByStatus(count: {0})", count);
            var items = await _unitOfWork.Posts.GetByStatusOrderByDescending(DocumentStatus.Published);
            logger.LogInformation("Posts loaded: {0}", items.Count());

            return items.MapToEditModel();
        }

        public async Task<(IEnumerable<PostTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters)
        {
            var items = await _unitOfWork.Posts.GetAllAsync();
            int recordsTotal = items.Count();
            if (!string.IsNullOrWhiteSpace(parameters.Search.Value))
            {
                items = FilterForDataTable(parameters.Search.Value, items);
            }
            items = OrderForDataTable(parameters.Order[0].Column, parameters.Order[0].Dir, items);
            return (items.MapToTableViewModel(), recordsTotal, items.Count());
        }

        public async Task<PaginatedList<PostViewModel>> GetPublishedByCategoryForPagination(string categorySlug, int page = 1)
        {
            logger.LogInformation("CmsService > GetPublishedByCategoryForPagination(categorySlug: {0}, page: {1})", categorySlug, page);
            var posts = await _unitOfWork.Posts.GetPublishedByCategoryForPagination(categorySlug, page, Instance.ArticleLimit);
            return new PaginatedList<PostViewModel>(posts.Items.MapToViewModelWithCategories(), posts.Count, page, Instance.ArticleLimit);
        }

        public async Task<PaginatedList<PostViewModel>> GetPublishedByTagForPagination(string tagSlug, int page = 1)
        {
            logger.LogInformation("CmsService > GetPublishedByTagForPagination(tagSlug: {0}, page: {1})", tagSlug, page);
            var posts = await _unitOfWork.Posts.GetPublishedByTagForPagination(tagSlug, page, Instance.ArticleLimit);
            return new PaginatedList<PostViewModel>(posts.Items.MapToViewModelWithTags(), posts.Count, page, Instance.ArticleLimit);
        }

        public async Task<PaginatedList<PostViewModel>> GetPublishedForPagination(int page = 1)
        {
            logger.LogInformation("CmsService > GetPublishedForPagination(page: {0})", page);
            var posts = await _unitOfWork.Posts.GetPublishedForPagination(page, Instance.ArticleLimit);
            return new PaginatedList<PostViewModel>(posts.Items.MapToViewModelWithAuthor(), posts.Count, page, Instance.ArticleLimit);
        }

        public async Task<IEnumerable<PostViewModel>> GetPublishedLatestPosts(int count)
        {
            logger.LogInformation("CmsService > GetPublishedLatestPosts(count: {0})", count);
            var posts = await _unitOfWork.Posts.GetPublishedLatestPosts(count);
            return posts.MapToViewModelLatestPosts();
        }

        public async Task<PaginatedList<PostViewModel>> FindPublishedForPaginationOrderByDateDescending(string searchTerm = "", int page = 1)
        {
            logger.LogInformation("CmsService > FindPublishedForPaginationOrderByDateDescending(page: {0}, searchTerm: {1})", page, searchTerm);
            var posts = await _unitOfWork.Posts.FindPublishedForPaginationOrderByDateDescending(page, searchTerm, Instance.ArticleLimit);
            return new PaginatedList<PostViewModel>(posts.Items.MapToViewModelWithAuthor(), posts.Count, page, Instance.ArticleLimit);
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
            logger.LogInformation("PostService > Save(PostEditModel: {0})", postEditModel.ToString());

            var returnValue = new ReturnValue($"Post '{postEditModel.Title}' saved at {DateTime.Now.ToString("T")}.");

            try
            {
                if (postEditModel.IsNew)
                {
                    logger.LogInformation("New post");
                    var post = postEditModel.MapToModel();
                    post.WebsiteId = Instance.Id;

                    await unitOfWork.Posts.Insert(post);
                }
                else
                {
                    logger.LogInformation("Update post");
                    var post = postEditModel.MapToModel(await unitOfWork.Posts.GetByIdAsync(postEditModel.VanityId));
                    post.WebsiteId = Instance.Id;

                    _unitOfWork.Posts.Update(post);
                }

                await _unitOfWork.Save();
                logger.LogInformation("Post saved");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                returnValue.SetErrorMessage("An error has occurred while saving the post");
            }

            return returnValue;
        }

        public PostEditModel SetupEditModel()
        {
            logger.LogInformation("PostService > SetupEditModel()");
            return new PostEditModel();
        }

        public async Task<PostEditModel> SetupEditModel(Guid id)
        {
            var item = await _unitOfWork.Posts.GetByIdAsync(id);
            logger.LogInformation("PostService > SetupPostEditModel(id: {0})", id);
            logger.LogInformation("Post: {0}", item.ToString());
            return item.MapToEditModel();
        }
    }
}
