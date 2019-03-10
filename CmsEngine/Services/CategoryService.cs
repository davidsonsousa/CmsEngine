using System;
using System.Collections.Generic;
using System.Linq;
using CmsEngine.Attributes;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Data.ViewModels.DataTableViewModels;
using CmsEngine.Utils;
using Microsoft.Extensions.Logging;

namespace CmsEngine
{
    public sealed partial class CmsService
    {
        #region Get

        public IEnumerable<T> GetAllCategoriesReadOnly<T>(int count = 0) where T : IViewModel
        {
            IEnumerable<Category> listItems = GetAllReadOnly<Category>(count);

            _logger.LogInformation("CmsService > GetAllCategoriesReadOnly(count: {0})", count);
            _logger.LogInformation("Categories loaded: {0}", listItems.Count());

            return _mapper.Map<IEnumerable<Category>, IEnumerable<T>>(listItems);
        }

        public IEnumerable<T> GetCategoriesWithPosts<T>() where T : IViewModel
        {
            IEnumerable<Category> listItems = _unitOfWork.Categories
                                                            .Get(q => q.PostCategories.Any(pc => pc.Post.Status == DocumentStatus.Published
                                                                                              && pc.Post.IsDeleted == false))
                                                            .OrderBy(o => o.Name)
                                                            .ToList();

            _logger.LogInformation("CmsService > GetCategoriesWithPosts()");
            _logger.LogInformation("Categories loaded: {0}", listItems.Count());

            return _mapper.Map<IEnumerable<Category>, IEnumerable<T>>(listItems);
        }

        public IViewModel GetCategoryById(int id)
        {
            var item = _unitOfWork.Categories.GetById(id);

            _logger.LogInformation("CmsService > GetCategoryById(id: {0})", id);
            _logger.LogInformation("Category: {0}", item.ToString());

            return _mapper.Map<Category, CategoryViewModel>(item);
        }

        public IViewModel GetCategoryById(Guid id)
        {
            var item = _unitOfWork.Categories.GetById(id);

            _logger.LogInformation("CmsService > GetCategoryById(id: {0})", id);
            _logger.LogInformation("Category: {0}", item.ToString());

            return _mapper.Map<Category, CategoryViewModel>(item);
        }

        public IViewModel GetCategoryBySlug(string slug)
        {
            var item = _unitOfWork.Categories.Get(q => q.Slug == slug).SingleOrDefault();

            _logger.LogInformation("CmsService > GetCategoryBySlug(slug: {0})", slug);
            _logger.LogInformation("Category: {0}", item.ToString());

            return _mapper.Map<Category, CategoryViewModel>(item);
        }

        #endregion

        #region Setup

        public IEditModel SetupCategoryEditModel()
        {
            _logger.LogInformation("CmsService > SetupCategoryEditModel()");
            return new CategoryEditModel();
        }

        public IEditModel SetupCategoryEditModel(int id)
        {
            var item = _unitOfWork.Categories.GetById(id);

            _logger.LogInformation("CmsService > SetupCategoryEditModel(id: {0})", id);
            _logger.LogInformation("Category: {0}", item.ToString());

            return _mapper.Map<Category, CategoryEditModel>(item);
        }

        public IEditModel SetupCategoryEditModel(Guid id)
        {
            var item = _unitOfWork.Categories.GetById(id);

            _logger.LogInformation("CmsService > SetupCategoryEditModel(id: {0})", id);
            _logger.LogInformation("Category: {0}", item.ToString());

            return _mapper.Map<Category, CategoryEditModel>(item);
        }

        #endregion

        #region Save

        public ReturnValue SaveCategory(IEditModel editModel)
        {
            _logger.LogInformation("CmsService > SaveCategory(editModel: {0})", editModel.ToString());

            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Category '{((CategoryEditModel)editModel).Name}' saved at {DateTime.Now.ToString("T")}"
            };

            try
            {
                PrepareCategoryForSaving(editModel);

                _unitOfWork.Save();
                _logger.LogInformation("Category saved");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when saving category {0}", editModel.ToString());

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
            return this.DeleteCategory(_unitOfWork.Categories.GetById(id));
        }

        public ReturnValue DeleteCategory(int id)
        {
            return this.DeleteCategory(_unitOfWork.Categories.GetById(id));
        }

        private ReturnValue DeleteCategory(Category category)
        {
            var returnValue = new ReturnValue();
            try
            {
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
                        listItems = orderDirection == "asc" ? listCategories.OrderBy(o => o.Name) : listCategories.OrderByDescending(o => o.Name);
                        break;
                    case 2:
                        listItems = orderDirection == "asc" ? listCategories.OrderBy(o => o.Slug) : listCategories.OrderByDescending(o => o.Slug);
                        break;
                    case 3:
                        listItems = orderDirection == "asc" ? listCategories.OrderBy(o => o.Description) : listCategories.OrderByDescending(o => o.Description);
                        break;
                    default:
                        listItems = listCategories.OrderBy(o => o.Name);
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
                _logger.LogInformation("New category");

                category = _mapper.Map<CategoryEditModel, Category>((CategoryEditModel)editModel);
                category.WebsiteId = Instance.Id;

                _unitOfWork.Categories.Insert(category);
            }
            else
            {
                _logger.LogInformation("Update category");

                category = _unitOfWork.Categories.GetById(editModel.VanityId);
                _mapper.Map((CategoryEditModel)editModel, category);
                category.WebsiteId = Instance.Id;

                _unitOfWork.Categories.Update(category);
            }
        }

        #endregion
    }
}
