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
    public class CategoryService : BaseService<Category>
    {
        public CategoryService(IUnitOfWork uow) : base(uow)
        {
        }

        public override ReturnValue BulkDelete(Guid[] id)
        {
            var returnValue = new ReturnValue();
            try
            {
                Repository.BulkUpdate(q => id.Contains(q.VanityId), u => new Category { IsDeleted = true });

                returnValue.IsError = false;
                returnValue.Message = string.Format("Selected items deleted at {0}.", DateTime.Now.ToShortTimeString());
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the category";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Delete(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var category = this.GetAll().Where(q => q.VanityId == id).FirstOrDefault();
                returnValue = this.Delete(category);
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the category";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Delete(int id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var category = this.GetAll().Where(q => q.Id == id).FirstOrDefault();
                returnValue = this.Delete(category);
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the category";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Save(IEditModel editModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Category '{((CategoryEditModel)editModel).Name}' saved."
            };

            try
            {
                PrepareForSaving(editModel);

                UnitOfWork.Save();
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while saving the category";
                throw;
            }

            return returnValue;
        }

        public override IEditModel SetupEditModel()
        {
            return new CategoryEditModel();
        }

        protected override IEditModel SetupEditModel(Category item)
        {
            var editModel = new CategoryEditModel();
            item.MapTo(editModel);

            return editModel;
        }

        protected override IViewModel SetupViewModel(Category item)
        {
            var viewModel = new CategoryViewModel();
            item.MapTo(viewModel);

            return viewModel;
        }


        protected override ReturnValue Delete(Category item)
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
                returnValue.Message = string.Format("Category '{0}' deleted at {1}.", item.Name, DateTime.Now.ToShortTimeString());
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the category";
                throw;
            }

            return returnValue;
        }

        protected override void PrepareForSaving(IEditModel editModel)
        {
            var category = new Category();
            editModel.MapTo(category);

            if (category.IsNew)
            {
                Repository.Insert(category);
            }
        }
    }
}
