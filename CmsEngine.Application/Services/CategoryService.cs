using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsEngine.Application.Attributes;
using CmsEngine.Application.EditModels;
using CmsEngine.Application.Extensions;
using CmsEngine.Application.Extensions.Mapper;
using CmsEngine.Application.ViewModels.DataTableViewModels;
using CmsEngine.Core;
using CmsEngine.Data;
using CmsEngine.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Application.Services
{
    public class CategoryService : Service, ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork uow, IHttpContextAccessor hca, ILogger log) : base(uow, hca, log)
        {
            _unitOfWork = uow;
        }

        public async Task<ReturnValue> Delete(Guid id)
        {
            var item = await _unitOfWork.Categories.GetByIdAsync(id);

            var returnValue = new ReturnValue($"Category '{item.Name}' deleted at {DateTime.Now.ToString("T")}.");

            try
            {
                _unitOfWork.Categories.Delete(item);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                returnValue.SetErrorMessage("An error has occurred while deleting the category");
            }

            return returnValue;
        }

        public async Task<ReturnValue> DeleteRange(Guid[] ids)
        {
            var items = await _unitOfWork.Categories.GetCategoriesById(ids);

            var returnValue = new ReturnValue($"Categories deleted at {DateTime.Now.ToString("T")}.");

            try
            {
                _unitOfWork.Categories.DeleteRange(items);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                returnValue.SetErrorMessage("An error has occurred while deleting the categories");
            }

            return returnValue;
        }

        public IEnumerable<Category> FilterForDataTable(string searchValue, IEnumerable<Category> items)
        {
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                var searchableProperties = typeof(CategoryTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));
                items = items.Where(items.GetSearchExpression(searchValue, searchableProperties).Compile());
            }

            return items;
        }

        public async Task<(IEnumerable<CategoryTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters)
        {
            var items = await _unitOfWork.Categories.GetAllAsync();
            int recordsTotal = items.Count();

            if (!string.IsNullOrWhiteSpace(parameters.Search.Value))
            {
                items = FilterForDataTable(parameters.Search.Value, items);
            }

            items = OrderForDataTable(parameters.Order[0].Column, parameters.Order[0].Dir, items);

            return (items.MapToTableViewModel(), recordsTotal, items.Count());
        }

        public IEnumerable<Category> OrderForDataTable(int column, string direction, IEnumerable<Category> items)
        {
            try
            {
                switch (column)
                {
                    case 1:
                        items = direction == "asc" ? items.OrderBy(o => o.Name) : items.OrderByDescending(o => o.Name);
                        break;
                    case 2:
                        items = direction == "asc" ? items.OrderBy(o => o.Slug) : items.OrderByDescending(o => o.Slug);
                        break;
                    case 3:
                        items = direction == "asc" ? items.OrderBy(o => o.Description) : items.OrderByDescending(o => o.Description);
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

        public async Task<ReturnValue> Save(CategoryEditModel categoryEditModel)
        {
            logger.LogInformation("CmsService > Save(CategoryEditModel: {0})", categoryEditModel.ToString());

            var returnValue = new ReturnValue($"Category '{categoryEditModel.Name}' saved at {DateTime.Now.ToString("T")}.");

            try
            {
                if (categoryEditModel.IsNew)
                {
                    logger.LogInformation("New category");
                    var category = categoryEditModel.MapToModel();
                    category.WebsiteId = Instance.Id;

                    await unitOfWork.Categories.Insert(category);
                }
                else
                {
                    logger.LogInformation("Update category");
                    var category = categoryEditModel.MapToModel(await unitOfWork.Categories.GetByIdAsync(categoryEditModel.VanityId));
                    category.WebsiteId = Instance.Id;

                    _unitOfWork.Categories.Update(category);
                }

                await _unitOfWork.Save();
                logger.LogInformation("Category saved");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                returnValue.SetErrorMessage("An error has occurred while saving the category");
            }

            return returnValue;
        }

        public CategoryEditModel SetupEditModel()
        {
            logger.LogInformation("CmsService > SetupEditModel()");
            return new CategoryEditModel();
        }

        public async Task<CategoryEditModel> SetupEditModel(Guid id)
        {
            var item = await _unitOfWork.Categories.GetByIdAsync(id);

            logger.LogInformation("CmsService > SetupCategoryEditModel(id: {0})", id);
            logger.LogInformation("Category: {0}", item.ToString());

            return item.MapToEditModel();
        }
    }
}
