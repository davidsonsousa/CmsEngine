using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Utils;
using CmsEngine.ViewModels;
using System;
using System.Linq;

namespace CmsEngine.Services
{
    public class WebsiteService : BaseService<Website>
    {
        public WebsiteService(IUnitOfWork uow) : base(uow)
        {
        }

        public override ReturnValue BulkDelete(Guid[] vanityId)
        {
            var returnValue = new ReturnValue();
            try
            {
                Repository.BulkUpdate(q => vanityId.Contains(q.VanityId), u => new Website { IsDeleted = true });

                returnValue.IsError = false;
                returnValue.Message = string.Format("Selected items deleted at {0}.", DateTime.Now.ToShortTimeString());
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the website";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Delete(Guid vanityId)
        {
            var returnValue = new ReturnValue();
            try
            {
                var website = this.GetAll().Where(q => q.VanityId == vanityId).FirstOrDefault();
                returnValue = this.Delete(website);
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the website";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Delete(int id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var website = this.GetAll().Where(q => q.Id == id).FirstOrDefault();
                returnValue = this.Delete(website);
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the website";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Save(IViewModel viewModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = string.Format("Website <strong>'{0}'</strong> saved at {1}.", ((BaseViewModel<Website>)viewModel).Item.Name, DateTime.Now.ToShortTimeString())
            };

            try
            {
                PrepareForSaving(viewModel);

                UnitOfWork.Save();
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while saving the website";
                throw;
            }

            return returnValue;
        }

        public override IViewModel SetupViewModel()
        {
            var itemViewModel = new BaseViewModel<Website>
            {
                Item = new Website(),
                Items = this.GetAllReadOnly()
            };

            return itemViewModel;
        }

        protected override ReturnValue Delete(Website item)
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
                returnValue.Message = string.Format("Website '{0}' deleted at {1}.", item.Name, DateTime.Now.ToShortTimeString());
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the website";
                throw;
            }

            return returnValue;
        }

        protected override void PrepareForSaving(IViewModel viewModel)
        {
            var website = ((BaseViewModel<Website>)viewModel).Item;

            if (website.IsNew)
            {
                Repository.Insert(website);
            }
        }
    }
}
