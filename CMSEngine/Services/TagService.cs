using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Utils;
using CmsEngine.ViewModels;
using System;
using System.Linq;

namespace CmsEngine.Services
{
    public class TagService : BaseService<Tag>
    {
        public TagService(IUnitOfWork uow) : base(uow)
        {
        }

        public override ReturnValue BulkDelete(Guid[] vanityId)
        {
            var returnValue = new ReturnValue();
            try
            {
                Repository.BulkUpdate(q => vanityId.Contains(q.VanityId), u => new Tag { IsDeleted = true });

                returnValue.IsError = false;
                returnValue.Message = string.Format("Selected items deleted at {0}.", DateTime.Now.ToShortTimeString());
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the tag";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Delete(Guid vanityId)
        {
            var returnValue = new ReturnValue();
            try
            {
                var tag = this.GetAll().Where(q => q.VanityId == vanityId).FirstOrDefault();
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

        public override ReturnValue Save(IViewModel viewModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = string.Format("Tag <strong>'{0}'</strong> saved at {1}.", ((BaseViewModel<Tag>)viewModel).Item.Name, DateTime.Now.ToShortTimeString())
            };

            try
            {
                PrepareForSaving(viewModel);

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

        public override IViewModel SetupViewModel()
        {
            var itemViewModel = new BaseViewModel<Tag>
            {
                Item = new Tag(),
                Items = this.GetAllReadOnly()
            };

            return itemViewModel;
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
                returnValue.Message = string.Format("Tag '{0}' deleted at {1}.", item.Name, DateTime.Now.ToShortTimeString());
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the tag";
                throw;
            }

            return returnValue;
        }

        protected override void PrepareForSaving(IViewModel viewModel)
        {
            var tag = ((BaseViewModel<Tag>)viewModel).Item;

            if (tag.IsNew)
            {
                Repository.Insert(tag);
            }
        }
    }
}
