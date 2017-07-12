using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Extensions;
using CmsEngine.Utils;
using System;
using System.Linq;

namespace CmsEngine.Services
{
    public class TagService : BaseService<Tag>
    {
        public TagService(IUnitOfWork uow) : base(uow)
        {
        }

        public override ReturnValue BulkDelete(Guid[] id)
        {
            var returnValue = new ReturnValue();
            try
            {
                Repository.BulkUpdate(q => id.Contains(q.VanityId), u => new Tag { IsDeleted = true });

                returnValue.IsError = false;
                returnValue.Message = string.Format("Selected items deleted at {0}.", DateTime.Now.ToString("d"));
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the tag";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Delete(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var tag = this.GetAll().Where(q => q.VanityId == id).FirstOrDefault();
                returnValue = this.Delete(tag);
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the tag";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Delete(int id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var tag = this.GetAll().Where(q => q.Id == id).FirstOrDefault();
                returnValue = this.Delete(tag);
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the tag";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Save(IEditModel editModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Tag '{((TagEditModel)editModel).Name}' saved."
            };

            try
            {
                PrepareForSaving(editModel);

                UnitOfWork.Save();
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while saving the tag";
                throw;
            }

            return returnValue;
        }

        public override IEditModel SetupEditModel()
        {
            return new TagEditModel();
        }

        protected override IEditModel SetupEditModel(Tag item)
        {
            var editModel = new TagEditModel();
            item.MapTo(editModel);

            return editModel;
        }

        protected override IViewModel SetupViewModel(Tag item)
        {
            var viewModel = new TagViewModel();
            item.MapTo(viewModel);

            return viewModel;
        }

        protected override ReturnValue Delete(Tag item)
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
                returnValue.Message = string.Format("Tag '{0}' deleted at {1}.", item.Name, DateTime.Now.ToString("d"));
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the tag";
                throw;
            }

            return returnValue;
        }

        protected override void PrepareForSaving(IEditModel editModel)
        {
            var tag = new Tag();
            editModel.MapTo(tag);

            if (tag.IsNew)
            {
                Repository.Insert(tag);
            }
            else
            {
                Repository.Update(tag);
            }
        }
    }
}
