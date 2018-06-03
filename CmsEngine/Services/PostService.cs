using System;
using System.Collections.Generic;
using System.Linq;
using CmsEngine.Attributes;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Extensions;
using CmsEngine.Utils;

namespace CmsEngine
{
    public sealed partial class CmsService
    {
        #region Get

        public IEnumerable<IViewModel> GetAllPostsReadOnly(int count = 0)
        {
            IEnumerable<Post> listItems;

            try
            {
                listItems = _unitOfWork.Posts.GetReadOnly(q => q.IsDeleted == false, count);
            }
            catch
            {
                throw;
            }

            return _mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(listItems);
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

        #endregion

        #region Setup

        public IEditModel SetupPostEditModel()
        {
            var editModel = new PostEditModel
            {
                Categories = this.PopulateCheckboxList<Category>()
            };

            return editModel;
        }

        public IEditModel SetupPostEditModel(int id)
        {
            var item = this.GetById<Post>(id, "PostCategories.Category");
            var editModel = _mapper.Map<Post, PostEditModel>(item);
            editModel.Categories = this.PopulateCheckboxList<Category>(editModel.SelectedCategories);

            return editModel;
        }

        public IEditModel SetupPostEditModel(Guid id)
        {
            var item = this.GetById<Post>(id, "PostCategories.Category");
            var editModel = _mapper.Map<Post, PostEditModel>(item);
            editModel.Categories = this.PopulateCheckboxList<Category>(editModel.SelectedCategories);

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
            var items = (IEnumerable<PostViewModel>)listItems;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(PostViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = this.PrepareFilter<Post>(searchTerm, searchableProperties);

                // TODO: There must be a way to improve this
                var tempItems = _mapper.Map<IEnumerable<PostViewModel>, IEnumerable<Post>>(items);
                items = _mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(tempItems.Where(lambda));
            }

            return items;
        }

        public IEnumerable<IViewModel> OrderPost(int orderColumn, string orderDirection, IEnumerable<IViewModel> listItems)
        {
            try
            {
                var listPosts = _mapper.Map<IEnumerable<IViewModel>, IEnumerable<PostViewModel>>(listItems);

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
                post.WebsiteId = _instanceId;

                _unitOfWork.Posts.Insert(post);

                PrepareRelatedCategories(post, postEditModel);
            }
            else
            {
                post = this.GetById<Post>(editModel.VanityId, "PostCategories.Category");
                _mapper.Map(postEditModel, post);

                _unitOfWork.Posts.Update(post);

                PrepareRelatedCategories(post, postEditModel);
            }
        }

        private void PrepareRelatedCategories(Post post, PostEditModel postEditModel)
        {
            // TODO: Improve the logic of this method

            IEnumerable<PostCategory> newItems = postEditModel.SelectedCategories?
                                                .Select(x => new PostCategory
                                                {
                                                    CategoryId = GetById<Category>(Guid.Parse(x)).Id,
                                                    Post = post
                                                });

            // Check if new items are null
            if (newItems == null)
            {
                newItems = new List<PostCategory>();
            }

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

        #endregion
    }
}
