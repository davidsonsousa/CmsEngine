using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Extensions;
using CmsEngine.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CmsEngine.Services
{
    public class CategoryService : BaseService<Category>
    {
        public CategoryService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
        {
        }

        public override IEnumerable<IViewModel> GetAllReadOnly()
        {
            IEnumerable<Category> listItems;

            try
            {
                listItems = Repository.GetReadOnly(q => q.IsDeleted == false);
            }
            catch
            {
                throw;
            }

            return Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(listItems);
        }

        public override IViewModel GetById(int id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Category, CategoryViewModel>(item);
        }

        public override IViewModel GetById(Guid id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Category, CategoryViewModel>(item);
        }

        public override ReturnValue BulkDelete(Guid[] id)
        {
            var returnValue = new ReturnValue();
            try
            {
                Repository.BulkUpdate(q => id.Contains(q.VanityId), u => new Category { IsDeleted = true });

                returnValue.IsError = false;
                returnValue.Message = string.Format("Selected items deleted at {0}.", DateTime.Now.ToString("d"));
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

        public override IEditModel SetupEditModel(int id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Category, CategoryEditModel>(item);
        }

        public override IEditModel SetupEditModel(Guid id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Category, CategoryEditModel>(item);
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
                returnValue.Message = string.Format("Category '{0}' deleted at {1}.", item.Name, DateTime.Now.ToString("d"));
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
            else
            {
                Repository.Update(category);
            }
        }
    }
}
