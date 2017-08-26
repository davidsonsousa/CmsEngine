using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Extensions;
using CmsEngine.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CmsEngine.Services
{
    public class PostService : BaseService<Post>
    {
        public PostService(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca) : base(uow, mapper, hca)
        {
        }

        public override IEnumerable<IViewModel> GetAllReadOnly()
        {
            IEnumerable<Post> listItems;

            try
            {
                listItems = Repository.GetReadOnly(q => q.IsDeleted == false);
            }
            catch
            {
                throw;
            }

            return Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(listItems);
        }

        public override IViewModel GetById(int id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Post, PostViewModel>(item);
        }

        public override IViewModel GetById(Guid id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Post, PostViewModel>(item);
        }

        public override ReturnValue BulkDelete(Guid[] id)
        {
            var returnValue = new ReturnValue();
            try
            {
                Repository.BulkUpdate(q => id.Contains(q.VanityId), u => new Post { IsDeleted = true });

                returnValue.IsError = false;
                returnValue.Message = $"Selected items deleted at {DateTime.Now.ToString("T")}.";
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the post";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Delete(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var post = this.GetAll().Where(q => q.VanityId == id).FirstOrDefault();
                returnValue = this.Delete(post);
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the post";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Delete(int id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var post = this.GetAll().Where(q => q.Id == id).FirstOrDefault();
                returnValue = this.Delete(post);
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the post";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Save(IEditModel editModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Post '{((PostEditModel)editModel).Title}' saved at {DateTime.Now.ToString("T")}."
            };

            try
            {
                PrepareForSaving(editModel);

                UnitOfWork.Save();
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while saving the post";
                throw;
            }

            return returnValue;
        }

        public override IEditModel SetupEditModel()
        {
            return new PostEditModel();
        }

        public override IEditModel SetupEditModel(int id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Post, PostEditModel>(item);
        }

        public override IEditModel SetupEditModel(Guid id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Post, PostEditModel>(item);
        }

        protected override ReturnValue Delete(Post item)
        {
            var returnValue = new ReturnValue();
            try
            {
                if (item != null)
                {
                    item.IsDeleted = true;
                    Repository.Update(item);
                }

                UnitOfWork.Save();
                returnValue.IsError = false;
                returnValue.Message = $"Post '{item.Title}' deleted at {DateTime.Now.ToString("T")}.";
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the post";
                throw;
            }

            return returnValue;
        }

        protected override void PrepareForSaving(IEditModel editModel)
        {
            var post = new Post();
            editModel.MapTo(post);

            post.Website = WebsiteInstance;

            if (post.IsNew)
            {
                Repository.Insert(post);
            }
            else
            {
                Repository.Update(post);
            }
        }
    }
}
