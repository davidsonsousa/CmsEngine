using System;
using System.Collections.Generic;
using System.Linq;
using CmsEngine.Attributes;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Data.ViewModels.DataTableViewModels;
using CmsEngine.Utils;

namespace CmsEngine
{
    public sealed partial class CmsService
    {
        #region Get

        public IEnumerable<T> GetAllCategoriesReadOnly<T>(int count = 0) where T : IViewModel
        {
            IEnumerable<Category> listItems = GetAllReadOnly<Category>(count);
            return _mapper.Map<IEnumerable<Category>, IEnumerable<T>>(listItems);
        }

        public IEnumerable<T> GetCategoriesWithPostCount<T>() where T : IViewModel
        {
            IEnumerable<Category> listItems = GetAll<Category>().OrderBy(o => o.Name).ToList();

            return _mapper.Map<IEnumerable<Category>, IEnumerable<T>>(listItems);
        }

        public IViewModel GetCategoryById(int id)
        {
            var item = this.GetById<Category>(id);
            return _mapper.Map<Category, CategoryViewModel>(item);
        }

        public IViewModel GetCategoryById(Guid id)
        {
            var item = this.GetById<Category>(id);
            return _mapper.Map<Category, CategoryViewModel>(item);
        }

        public IViewModel GetCategoryBySlug(string slug)
        {
            var item = this.GetAll<Category>().Where(q => q.Slug == slug).SingleOrDefault();
            return _mapper.Map<Category, CategoryViewModel>(item);
        }

        #endregion

        #region Setup

        public IEditModel SetupCategoryEditModel()
        {
            return new CategoryEditModel();
        }

        public IEditModel SetupCategoryEditModel(int id)
        {
            var item = this.GetById<Category>(id);
            return _mapper.Map<Category, CategoryEditModel>(item);
        }

        public IEditModel SetupCategoryEditModel(Guid id)
        {
            var item = this.GetById<Category>(id);
            return _mapper.Map<Category, CategoryEditModel>(item);
        }

        #endregion

        #region Save

        public ReturnValue SaveCategory(IEditModel editModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Category '{((CategoryEditModel)editModel).Name}' saved at {DateTime.Now.ToString("T")}"
            };

            try
            {
                PrepareCategoryForSaving(editModel);

                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while saving the category";
                returnValue.Exception = ex.Message;
                throw;
            }

            return returnValue;
        }

        #endregion

        #region Delete

        public ReturnValue DeleteCategory(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var category = this.GetAll<Category>().Where(q => q.VanityId == id).FirstOrDefault();
                returnValue = this.Delete(category);

                if (!returnValue.IsError)
                {
                    returnValue.Message = $"Category '{category.Name}' deleted at {DateTime.Now.ToString("T")}.";
                }
                else
                {
                    returnValue.Message = "An error has occurred while deleting the category";
                }
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the category";
                throw;
            }

            return returnValue;
        }

        public ReturnValue DeleteCategory(int id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var category = this.GetAll<Category>().Where(q => q.Id == id).FirstOrDefault();
                returnValue = this.Delete(category);

                if (!returnValue.IsError)
                {
                    returnValue.Message = $"Category '{category.Name}' deleted at {DateTime.Now.ToString("T")}.";
                }
                else
                {
                    returnValue.Message = "An error has occurred while deleting the category";
                }
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the category";
                throw;
            }

            return returnValue;
        }

        #endregion

        #region DataTable

        public IEnumerable<IViewModel> FilterCategory(string searchTerm, IEnumerable<IViewModel> listItems)
        {
            var items = (IEnumerable<CategoryTableViewModel>)listItems;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(CategoryTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = this.PrepareFilter<Category>(searchTerm, searchableProperties);

                // TODO: There must be a way to improve this
                var tempItems = _mapper.Map<IEnumerable<CategoryTableViewModel>, IEnumerable<Category>>(items);
                items = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryTableViewModel>>(tempItems.Where(lambda));
            }

            return items;
        }

        public IEnumerable<IViewModel> OrderCategory(int orderColumn, string orderDirection, IEnumerable<IViewModel> listItems)
        {
            try
            {
                var listCategories = _mapper.Map<IEnumerable<IViewModel>, IEnumerable<CategoryTableViewModel>>(listItems);

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

        #endregion

        #region Helpers

        private void PrepareCategoryForSaving(IEditModel editModel)
        {
            Category category;

            if (editModel.IsNew)
            {
                category = _mapper.Map<CategoryEditModel, Category>((CategoryEditModel)editModel);
                category.WebsiteId = Instance.Id;

                _unitOfWork.Categories.Insert(category);
            }
            else
            {
                category = this.GetById<Category>(editModel.VanityId);
                _mapper.Map((CategoryEditModel)editModel, category);
                category.WebsiteId = Instance.Id;

                _unitOfWork.Categories.Update(category);
            }
        }

        #endregion
    }
}
