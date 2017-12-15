using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CmsEngine.Attributes;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Utils;

namespace CmsEngine
{
    public sealed partial class CmsService
    {
        #region Get

        public IEnumerable<IViewModel> GetAllPostsReadOnly()
        {
            IEnumerable<Post> listItems;

            try
            {
                listItems = _unitOfWork.Posts.GetReadOnly(q => q.IsDeleted == false);
            }
            catch
            {
                throw;
            }

            return Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(listItems);
        }

        public IViewModel GetPostById(int id)
        {
            var item = this.GetById<Post>(id);
            return Mapper.Map<Post, PostViewModel>(item);
        }

        public IViewModel GetPostById(Guid id)
        {
            var item = this.GetById<Post>(id);
            return Mapper.Map<Post, PostViewModel>(item);
        }

        #endregion

        #region Setup

        public IEditModel SetupPostEditModel()
        {
            return new PostEditModel();
        }

        public IEditModel SetupPostEditModel(int id)
        {
            var item = this.GetById<Post>(id, "PostCategories.Category");
            var editModel = Mapper.Map<Post, PostEditModel>(item);
            editModel.Categories = this.PopulateCheckboxList<Category>(editModel.SelectedCategories);

            return editModel;
        }

        public IEditModel SetupPostEditModel(Guid id)
        {
            var item = this.GetById<Post>(id, "PostCategories.Category");
            var editModel = Mapper.Map<Post, PostEditModel>(item);
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
                var tempItems = Mapper.Map<IEnumerable<PostViewModel>, IEnumerable<Post>>(items);
                items = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(tempItems.Where(lambda));
            }

            return items;
        }

        public IEnumerable<IViewModel> OrderPost(int orderColumn, string orderDirection, IEnumerable<IViewModel> listItems)
        {
            try
            {
                var listPosts = Mapper.Map<IEnumerable<IViewModel>, IEnumerable<PostViewModel>>(listItems);

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

            if (editModel.IsNew)
            {
                post = Mapper.Map<PostEditModel, Post>((PostEditModel)editModel);
                _unitOfWork.Posts.Insert(post);
            }
            else
            {
                post = this.GetById<Post>(editModel.VanityId);
                Mapper.Map((PostEditModel)editModel, post);

                _unitOfWork.Posts.Update(post);
            }
        }

        #endregion
    }
}
