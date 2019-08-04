using System;
using System.Collections.Generic;
using System.Linq;
using CmsEngine.Attributes;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Data.ViewModels.DataTableViewModels;
using CmsEngine.Extensions;
using CmsEngine.Utils;
using Microsoft.Extensions.Logging;

namespace CmsEngine
{
    public sealed partial class CmsService
    {
        public PaginatedList<T> GetPagedPostsByStatusReadOnly<T>(DocumentStatus documentStatus, int pageIndex = 1) where T : IViewModel
        {
            var posts = GetDocumentsByStatus<Post>(documentStatus);

            _logger.LogInformation("CmsService > GetPagedPostsByStatusReadOnly(documentStatus: {0}, pageIndex: {1})", documentStatus, pageIndex);
            _logger.LogInformation("Posts loaded: {0}", posts.Count());

            return PreparePostsForPaging<T>(pageIndex, posts);
        }

        public PaginatedList<T> GetPagedPostsByCategoryReadOnly<T>(string categorySlug, int pageIndex = 1) where T : IViewModel
        {
            var posts = GetDocumentsByStatus<Post>(DocumentStatus.Published).Where(q => q.PostCategories
                                                                                         .Any(pc => pc.Category.Slug == categorySlug));

            _logger.LogInformation("CmsService > GetPagedPostsByCategoryReadOnly(categorySlug: {0}, pageIndex: {1})", categorySlug, pageIndex);
            _logger.LogInformation("Posts loaded: {0}", posts.Count());

            return PreparePostsForPaging<T>(pageIndex, posts);
        }

        public PaginatedList<T> GetPagedPostsByTagReadOnly<T>(string tagSlug, int pageIndex = 1) where T : IViewModel
        {
            var posts = GetDocumentsByStatus<Post>(DocumentStatus.Published).Where(q => q.PostTags.Any(pt => pt.Tag.Slug == tagSlug));

            _logger.LogInformation("CmsService > GetPagedPostsByTagReadOnly(tagSlug: {0}, pageIndex: {1})", tagSlug, pageIndex);
            _logger.LogInformation("Posts loaded: {0}", posts.Count());

            return PreparePostsForPaging<T>(pageIndex, posts);
        }

        public PaginatedList<T> GetPagedPostsFullTextSearch<T>(DocumentStatus documentStatus, int pageIndex = 1, string searchTerm = "")
            where T : IViewModel
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return GetPagedPostsByStatusReadOnly<T>(documentStatus, pageIndex);
            }

            var posts = GetDocumentsByStatus<Post>(documentStatus).Where(q => q.Title.Contains(searchTerm)
                                                                           || q.DocumentContent.Contains(searchTerm));

            _logger.LogInformation("CmsService > GetPagedPostsFullTextSearch(documentStatus: {0}, pageIndex: {1}, searchTerm: {2})",
                                   documentStatus, pageIndex, searchTerm);
            _logger.LogInformation("Posts loaded: {0}", posts.Count());

            return PreparePostsForPaging<T>(pageIndex, posts);
        }

        public IEnumerable<T> GetPostsByStatusReadOnly<T>(DocumentStatus documentStatus, int count = 0) where T : IViewModel
        {
            var posts = GetDocumentsByStatus<Post>(documentStatus, count);

            _logger.LogInformation("CmsService > GetPostsByStatusReadOnly(documentStatus: {0}, count: {1})", documentStatus, count);
            _logger.LogInformation("Posts loaded: {0}", posts.Count());

            return _mapper.Map<IEnumerable<Post>, IEnumerable<T>>(posts);
        }

        public IEnumerable<T> GetAllPostsReadOnly<T>(int count = 0) where T : IViewModel
        {
            IEnumerable<Post> listItems = GetAllReadOnly<Post>(count);

            _logger.LogInformation("CmsService > GetAllPostsReadOnly(count: {0})", count);
            _logger.LogInformation("Posts loaded: {0}", listItems.Count());

            return _mapper.Map<IEnumerable<Post>, IEnumerable<T>>(listItems);
        }

        public (IEnumerable<IViewModel> Data, int RecordsCount) GetPostsForDataTable(DataParameters parameters)
        {
            var items = _unitOfWork.Posts.Get();

            if (!string.IsNullOrWhiteSpace(parameters.Search.Value))
            {
                items = FilterPost(parameters.Search.Value, items);
            }

            items = OrderPost(parameters.Order[0].Column, parameters.Order[0].Dir, items);

            int recordsCount = items.Count();

            return (_mapper.Map<IEnumerable<Post>, IEnumerable<PostTableViewModel>>(items.Skip(parameters.Start).Take(parameters.Length).ToList()), recordsCount);
        }

        public IViewModel GetPostById(int id)
        {
            var item = _unitOfWork.Posts.GetById(id);

            _logger.LogInformation("CmsService > GetPostById(id: {0})", id);

            if (item != null)
            {
                _logger.LogInformation("Post: {0}", item.ToString());
            }

            return _mapper.Map<Post, PostViewModel>(item);
        }

        public IViewModel GetPostById(Guid id)
        {
            var item = _unitOfWork.Posts.GetById(id);

            _logger.LogInformation("CmsService > GetPostById(id: {0})", id);

            if (item != null)
            {
                _logger.LogInformation("Post: {0}", item.ToString());
            }

            return _mapper.Map<Post, PostViewModel>(item);
        }

        public IViewModel GetPostBySlug(string slug)
        {
            var item = _unitOfWork.Posts.Get(q => q.Slug == slug).SingleOrDefault();

            _logger.LogInformation("CmsService > GetPostById(slug: {0})", slug);

            if (item != null)
            {
                _logger.LogInformation("Post: {0}", item.ToString());
            }

            return _mapper.Map<Post, PostViewModel>(item);
        }

        public IEditModel SetupPostEditModel()
        {
            _logger.LogInformation("CmsService > SetupPostEditModel()");

            return new PostEditModel
            {
                Categories = PopulateCheckboxList<Category>(),
                Tags = PopulateSelectListItems<Tag>()
            };
        }

        public IEditModel SetupPostEditModel(int id)
        {
            var item = _unitOfWork.Posts.GetById(id);
            var editModel = _mapper.Map<Post, PostEditModel>(item);
            editModel.Categories = PopulateCheckboxList<Category>(editModel.SelectedCategories);
            editModel.Tags = PopulateSelectListItems<Tag>(editModel.SelectedTags);

            _logger.LogInformation("CmsService > SetupPostEditModel(id: {0})", id);
            _logger.LogInformation("Post: {0}", editModel.ToString());

            return editModel;
        }

        public IEditModel SetupPostEditModel(Guid id)
        {
            var item = _unitOfWork.Posts.GetById(id);
            var editModel = _mapper.Map<Post, PostEditModel>(item);
            editModel.Categories = PopulateCheckboxList<Category>(editModel.SelectedCategories);
            editModel.Tags = PopulateSelectListItems<Tag>(editModel.SelectedTags);

            _logger.LogInformation("CmsService > SetupPostEditModel(id: {0})", id);
            _logger.LogInformation("Post: {0}", editModel.ToString());

            return editModel;
        }

        public ReturnValue SavePost(IEditModel editModel)
        {
            _logger.LogInformation("CmsService > SavePost(editModel: {0})", editModel.ToString());

            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Post '{((PostEditModel)editModel).Title}' saved at {DateTime.Now.ToString("T")}"
            };

            try
            {
                PreparePostForSaving(editModel);

                _unitOfWork.Save();
                _logger.LogInformation("Post saved");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when saving category {0}", editModel.ToString());

                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while saving the post";
                returnValue.Exception = ex.Message;
                throw;
            }

            return returnValue;
        }

        public ReturnValue DeletePost(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var post = _unitOfWork.Posts.GetById(id);
                returnValue = Delete(post);

                if (!returnValue.IsError)
                {
                    returnValue.Message = $"Post '{post.Title}' deleted at {DateTime.Now.ToString("T")}.";
                }
                else
                {
                    returnValue.Message = "An error has occurred while deleting the post";
                }
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the post";
                throw;
            }

            return returnValue;
        }

        public ReturnValue DeletePost(int id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var post = _unitOfWork.Posts.GetById(id);
                returnValue = Delete(post);

                if (!returnValue.IsError)
                {
                    returnValue.Message = $"Post '{post.Title}' deleted at {DateTime.Now.ToString("T")}.";
                }
                else
                {
                    returnValue.Message = "An error has occurred while deleting the post";
                }
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the post";
                throw;
            }

            return returnValue;
        }

        private IQueryable<Post> FilterPost(string searchTerm, IQueryable<Post> items)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(PostTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = PrepareFilter<Post>(searchTerm, searchableProperties);
                items = items.Where(lambda);
            }

            return items;
        }

        private IQueryable<Post> OrderPost(int orderColumn, string orderDirection, IQueryable<Post> items)
        {
            try
            {
                switch (orderColumn)
                {
                    case 1:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.Title) : items.OrderByDescending(o => o.Title);
                        break;
                    case 2:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.Description) : items.OrderByDescending(o => o.Description);
                        break;
                    case 3:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.Slug) : items.OrderByDescending(o => o.Slug);
                        break;
                    //case 4:
                    //    items = orderDirection == "asc" ? items.OrderBy(o => o.Author.FullName) : items.OrderByDescending(o => o.Author.FullName);
                    //    break;
                    case 5:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.PublishedOn) : items.OrderByDescending(o => o.PublishedOn);
                        break;
                    case 6:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.Status) : items.OrderByDescending(o => o.Status);
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

        private void PreparePostForSaving(IEditModel editModel)
        {
            Post post;
            var postEditModel = (PostEditModel)editModel;

            if (editModel.IsNew)
            {
                _logger.LogInformation("New post");

                post = _mapper.Map<PostEditModel, Post>(postEditModel);
                post.WebsiteId = Instance.Id;

                _unitOfWork.Posts.Insert(post);
            }
            else
            {
                _logger.LogInformation("Update post");

                post = _unitOfWork.Posts.GetById(editModel.VanityId);
                _mapper.Map(postEditModel, post);

                _unitOfWork.Posts.Update(post);
            }

            PrepareRelatedAuthorsForPost(post);
            PrepareRelatedCategories(post, postEditModel);
            PrepareRelatedTags(post, postEditModel);
        }

        private void PrepareRelatedAuthorsForPost(Post post)
        {
            // TODO: Improve the logic of this method

            _logger.LogInformation("Prepare related authors for post");

            if (post.PostApplicationUsers == null || post.PostApplicationUsers.Count == 0)
            {
                _unitOfWork.GetRepositoryMany<PostApplicationUser>()
                           .InsertMany(new List<PostApplicationUser>
                           {
                               new PostApplicationUser{
                                   ApplicationUserId = CurrentUser.VanityId.ToString(),
                                   Post = post
                               }
                           });
            }
        }

        private void PrepareRelatedCategories(Post post, PostEditModel postEditModel)
        {
            // TODO: Improve the logic of this method

            _logger.LogInformation("Prepare related categories for post");

            IEnumerable<PostCategory> newItems = postEditModel.SelectedCategories?
                                                .Select(x => new PostCategory
                                                {
                                                    CategoryId = _unitOfWork.Categories.GetById(Guid.Parse(x)).Id,
                                                    Post = post
                                                }) ?? new List<PostCategory>();

            ICollection<PostCategory> currentItems = null;

            // Check current items
            if (post.PostCategories != null)
            {
                currentItems = post.PostCategories;
            }

            // Check if the values were assigned
            if (currentItems != null)
            {
                _unitOfWork.GetRepositoryMany<PostCategory>()
                           .DeleteMany(currentItems.Except(newItems, x => x.CategoryId));
                _unitOfWork.GetRepositoryMany<PostCategory>()
                           .InsertMany(newItems.Except(currentItems, x => x.CategoryId));
            }
        }

        private void PrepareRelatedTags(Post post, PostEditModel postEditModel)
        {
            // TODO: Improve the logic of this method

            _logger.LogInformation("Prepare related tags for post");

            IEnumerable<PostTag> newItems = postEditModel.SelectedTags?
                                                .Select(x => new PostTag
                                                {
                                                    TagId = _unitOfWork.Tags.GetById(Guid.Parse(x)).Id,
                                                    Post = post
                                                }) ?? new List<PostTag>();

            ICollection<PostTag> currentItems = null;

            // Check current items
            if (post.PostTags != null)
            {
                currentItems = post.PostTags;
            }

            // Check if the values were assigned
            if (currentItems != null)
            {
                _unitOfWork.GetRepositoryMany<PostTag>()
                           .DeleteMany(currentItems.Except(newItems, x => x.TagId));
                _unitOfWork.GetRepositoryMany<PostTag>()
                           .InsertMany(newItems.Except(currentItems, x => x.TagId));
            }
        }

        private PaginatedList<T> PreparePostsForPaging<T>(int page, IQueryable<Post> posts) where T : IViewModel
        {
            _logger.LogInformation("Prepare posts for paging");

            var count = posts.Count();
            var items = posts.Skip((page - 1) * Instance.ArticleLimit).Take(Instance.ArticleLimit).ToList();
            var mappedItems = _mapper.Map<IEnumerable<Post>, IEnumerable<T>>(items);

            return new PaginatedList<T>(mappedItems, count, page, Instance.ArticleLimit);
        }
    }
}
