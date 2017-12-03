using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CmsEngine.Attributes;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Extensions;
using CmsEngine.Utils;
using Microsoft.AspNetCore.Http;

namespace CmsEngine.Services
{
    public sealed class CategoryService : BaseService<Category>
    {
        public CategoryService(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca) : base(uow, mapper, hca)
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
                Message = $"Category '{((CategoryEditModel)editModel).Name}' saved at {DateTime.Now.ToString("T")}."
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

        public override IEnumerable<IViewModel> Filter(string searchTerm, IEnumerable<IViewModel> listItems)
        {
            var items = (IEnumerable<CategoryViewModel>)listItems;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(CategoryViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = this.PrepareFilter(searchTerm, searchableProperties);

                // TODO: There must be a way to improve this
                var tempItems = Mapper.Map<IEnumerable<CategoryViewModel>, IEnumerable<Category>>(items);
                items = Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(tempItems.Where(lambda));
            }

            return items;
        }

        public override IEnumerable<IViewModel> Order(int orderColumn, string orderDirection, IEnumerable<IViewModel> listItems)
        {
            try
            {
                var listCategories = Mapper.Map<IEnumerable<IViewModel>, IEnumerable<CategoryViewModel>>(listItems);

                switch (orderColumn)
                {
                    case 1:
                    case 0:
                    default:
                        listItems = orderDirection == "asc" ? listCategories.OrderBy(o => o.Name) : listCategories.OrderByDescending(o => o.Name);
                        break;
                }
            }
            catch
            {
                throw;
            }

            return listItems;
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
                returnValue.Message = string.Format("Category '{0}' deleted at {1}.", item.Name, DateTime.Now.ToString("T"));
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

            category.WebsiteId = WebsiteInstance.Id;

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
