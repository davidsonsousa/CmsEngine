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
        public IEnumerable<T> GetAllCategoriesReadOnly<T>(int count = 0) where T : IViewModel
        {
            IEnumerable<Category> listItems = this.GetAllReadOnly<Category>(count);

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

        public (IEnumerable<IViewModel> Data, int RecordsCount) GetCategoriesForDataTable(DataParameters parameters)
        {
            var items = _unitOfWork.Categories.Get();

            if (!string.IsNullOrWhiteSpace(parameters.Search.Value))
            {
                items = this.FilterCategory(parameters.Search.Value, items);
            }

            items = this.OrderCategory(parameters.Order[0].Column, parameters.Order[0].Dir, items);

            int recordsCount = items.Count();

            return (_mapper.Map<IEnumerable<Category>, IEnumerable<CategoryTableViewModel>>(items.Skip(parameters.Start).Take(parameters.Length).ToList()), recordsCount);
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
                this.PrepareCategoryForSaving(editModel);

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

        private IQueryable<Category> FilterCategory(string searchTerm, IQueryable<Category> items)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(CategoryTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = this.PrepareFilter<Category>(searchTerm, searchableProperties);
                items = items.Where(lambda);
            }

            return items;
        }

        private IQueryable<Category> OrderCategory(int orderColumn, string orderDirection, IQueryable<Category> items)
        {
            try
            {
                switch (orderColumn)
                {
                    case 1:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.Name) : items.OrderByDescending(o => o.Name);
                        break;
                    case 2:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.Slug) : items.OrderByDescending(o => o.Slug);
                        break;
                    case 3:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.Description) : items.OrderByDescending(o => o.Description);
                        break;
                    default:
                        items = items.OrderBy(o => o.Name);
                        break;
                }
            }
            catch
            {
                throw;
            }

            return items;
        }

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
    }
}
