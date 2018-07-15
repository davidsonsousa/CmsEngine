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
using Microsoft.EntityFrameworkCore;

namespace CmsEngine
{
    public sealed partial class CmsService
    {
        #region Get

        public PaginatedList<T> GetPagedPostsByStatusReadOnly<T>(DocumentStatus documentStatus, int pageIndex = 1) where T : IViewModel
        {
            var posts = GetDocumentsByStatus<Post>(documentStatus);
            return PreparePostsForPaging<T>(pageIndex, posts);
        }

        public PaginatedList<T> GetPagedPostsByCategoryReadOnly<T>(string categorySlug, int pageIndex = 1) where T : IViewModel
        {
            var posts = GetDocumentsByStatus<Post>(DocumentStatus.Published)
                            .Where(q => q.PostCategories.Any(pc => pc.Category.Slug == categorySlug));
            return PreparePostsForPaging<T>(pageIndex, posts);
        }

        public PaginatedList<T> GetPagedPostsByTagReadOnly<T>(string tagSlug, int pageIndex = 1) where T : IViewModel
        {
            var posts = GetDocumentsByStatus<Post>(DocumentStatus.Published)
                            .Where(q => q.PostTags.Any(pt => pt.Tag.Slug == tagSlug));
            return PreparePostsForPaging<T>(pageIndex, posts);
        }

        public PaginatedList<T> GetPagedPostsFullTextSearch<T>(DocumentStatus documentStatus, int pageIndex = 1, string searchTerm = "")
            where T : IViewModel
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return GetPagedPostsByStatusReadOnly<T>(documentStatus, pageIndex);
            }

            var posts = GetDocumentsByStatus<Post>(documentStatus)
                            .Where(q => q.Title.Contains(searchTerm) || q.DocumentContent.Contains(searchTerm));

            return PreparePostsForPaging<T>(pageIndex, posts);
        }

        public IEnumerable<T> GetPostsByStatusReadOnly<T>(DocumentStatus documentStatus, int count = 0) where T : IViewModel
        {
            var posts = GetDocumentsByStatus<Post>(documentStatus, count);
            return _mapper.Map<IEnumerable<Post>, IEnumerable<T>>(posts);
        }

        public IEnumerable<T> GetAllPostsReadOnly<T>(int count = 0) where T : IViewModel
        {
            IEnumerable<Post> listItems = GetAllReadOnly<Post>(count);
            return _mapper.Map<IEnumerable<Post>, IEnumerable<T>>(listItems);
        }

        public IViewModel GetPostById(int id)
        {
            var item = this.GetById<Post>(id);
            return _mapper.Map<Post, PostViewModel>(item);
        }

        public IViewModel GetPostById(Guid id)
        {
            var item = this.GetById<Post>(id);
            return _mapper.Map<Post, PostViewModel>(item);
        }

        public IViewModel GetPostBySlug(string slug)
        {
            var item = GetAll<Post>()
                       .Include(p => p.PostCategories).ThenInclude(pc => pc.Category)
                       .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
                       .Where(q => q.Slug == slug).SingleOrDefault();
            return _mapper.Map<Post, PostViewModel>(item);
        }

        #endregion

        #region Setup

        public IEditModel SetupPostEditModel()
        {
            return new PostEditModel
            {
                Categories = this.PopulateCheckboxList<Category>(),
                Tags = this.PopulateSelectListItems<Tag>()
            };
        }

        public IEditModel SetupPostEditModel(int id)
        {
            var item = this.GetById<Post>(id);
            var editModel = _mapper.Map<Post, PostEditModel>(item);
            editModel.Categories = this.PopulateCheckboxList<Category>(editModel.SelectedCategories);
            editModel.Tags = this.PopulateSelectListItems<Tag>(editModel.SelectedTags);

            return editModel;
        }

        public IEditModel SetupPostEditModel(Guid id)
        {
            var item = this.GetById<Post>(id);
            var editModel = _mapper.Map<Post, PostEditModel>(item);
            editModel.Categories = this.PopulateCheckboxList<Category>(editModel.SelectedCategories);
            editModel.Tags = this.PopulateSelectListItems<Tag>(editModel.SelectedTags);

            return editModel;
        }

        #endregion

        #region Save

        public ReturnValue SavePost(IEditModel editModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Post '{((PostEditModel)editModel).Title}' saved at {DateTime.Now.ToString("T")}"
            };

            try
            {
                PreparePostForSaving(editModel);

                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while saving the post";
                returnValue.Exception = ex.Message;
                throw;
            }

            return returnValue;
        }

        #endregion

        #region Delete

        public ReturnValue DeletePost(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var post = this.GetAll<Post>().Where(q => q.VanityId == id).FirstOrDefault();
                returnValue = this.Delete(post);

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
                var post = this.GetAll<Post>().Where(q => q.Id == id).FirstOrDefault();
                returnValue = this.Delete(post);

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

        #endregion

        #region DataTable

        public IEnumerable<IViewModel> FilterPost(string searchTerm, IEnumerable<IViewModel> listItems)
        {
            var items = (IEnumerable<PostTableViewModel>)listItems;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(PostTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = this.PrepareFilter<Post>(searchTerm, searchableProperties);

                // TODO: There must be a way to improve this
                var tempItems = _mapper.Map<IEnumerable<PostTableViewModel>, IEnumerable<Post>>(items);
                items = _mapper.Map<IEnumerable<Post>, IEnumerable<PostTableViewModel>>(tempItems.Where(lambda));
            }

            return items;
        }

        public IEnumerable<IViewModel> OrderPost(int orderColumn, string orderDirection, IEnumerable<IViewModel> listItems)
        {
            try
            {
                var listPosts = _mapper.Map<IEnumerable<IViewModel>, IEnumerable<PostTableViewModel>>(listItems);

                switch (orderColumn)
                {
                    case 1:
                    case 0:
                    default:
                        listItems = orderDirection == "asc" ? listPosts.OrderBy(o => o.Title) : listPosts.OrderByDescending(o => o.Title);
                        break;
                }
            }
            catch
            {
                throw;
            }

            return listItems;
        }

        #endregion

        #region Helpers

        private void PreparePostForSaving(IEditModel editModel)
        {
            Post post;
            var postEditModel = (PostEditModel)editModel;

            // The field is varchar(20)
            postEditModel.Author = CurrentUser.FullName.Length > 20
                                   ? CurrentUser.FullName.Substring(0, 20)
                                   : CurrentUser.FullName;
            postEditModel.AuthorId = CurrentUser.VanityId.ToString();

            if (editModel.IsNew)
            {
                post = _mapper.Map<PostEditModel, Post>(postEditModel);
                post.WebsiteId = Instance.Id;

                _unitOfWork.Posts.Insert(post);
            }
            else
            {
                post = this.GetById<Post>(editModel.VanityId);
                _mapper.Map(postEditModel, post);

                _unitOfWork.Posts.Update(post);
            }

            PrepareRelatedCategories(post, postEditModel);
            PrepareRelatedTags(post, postEditModel);
        }

        private void PrepareRelatedCategories(Post post, PostEditModel postEditModel)
        {
            // TODO: Improve the logic of this method

            IEnumerable<PostCategory> newItems = postEditModel.SelectedCategories?
                                                .Select(x => new PostCategory
                                                {
                                                    CategoryId = GetById<Category>(Guid.Parse(x)).Id,
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
                _unitOfWork.GetRepository<PostCategory>()
                           .DeleteMany(currentItems.Except(newItems, x => x.CategoryId));
                _unitOfWork.GetRepository<PostCategory>()
                           .InsertMany(newItems.Except(currentItems, x => x.CategoryId));
            }
        }

        private void PrepareRelatedTags(Post post, PostEditModel postEditModel)
        {
            // TODO: Improve the logic of this method

            IEnumerable<PostTag> newItems = postEditModel.SelectedTags?
                                                .Select(x => new PostTag
                                                {
                                                    TagId = GetById<Tag>(Guid.Parse(x)).Id,
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
                _unitOfWork.GetRepository<PostTag>()
                           .DeleteMany(currentItems.Except(newItems, x => x.TagId));
                _unitOfWork.GetRepository<PostTag>()
                           .InsertMany(newItems.Except(currentItems, x => x.TagId));
            }
        }

        private PaginatedList<T> PreparePostsForPaging<T>(int page, IQueryable<Post> posts) where T : IViewModel
        {
            var count = posts.Count();
            var items = posts.Skip((page - 1) * Instance.ArticleLimit).Take(Instance.ArticleLimit).ToList();
            var mappedItems = _mapper.Map<IEnumerable<Post>, IEnumerable<T>>(items);

            return new PaginatedList<T>(mappedItems, count, page, Instance.ArticleLimit);
        }

        #endregion
    }
}
