using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsEngine.Application.Attributes;
using CmsEngine.Application.EditModels;
using CmsEngine.Application.Extensions;
using CmsEngine.Application.Extensions.Mapper;
using CmsEngine.Application.ViewModels;
using CmsEngine.Application.ViewModels.DataTableViewModels;
using CmsEngine.Core;
using CmsEngine.Data;
using CmsEngine.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Application.Services
{
    public class TagService : Service, ITagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IUnitOfWork uow, IHttpContextAccessor hca, ILoggerFactory loggerFactory, IMemoryCache memoryCache)
                         : base(uow, hca, loggerFactory, memoryCache)
        {
            _unitOfWork = uow;
        }

        public async Task<ReturnValue> Delete(Guid id)
        {
            var item = await _unitOfWork.Tags.GetByIdAsync(id);

            var returnValue = new ReturnValue($"Tag '{item.Name}' deleted at {DateTime.Now.ToString("T")}.");

            try
            {
                _unitOfWork.Tags.Delete(item);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                returnValue.SetErrorMessage("An error has occurred while deleting the tag");
            }

            return returnValue;
        }

        public async Task<ReturnValue> DeleteRange(Guid[] ids)
        {
            var items = await _unitOfWork.Tags.GetByMultipleIdsAsync(ids);

            var returnValue = new ReturnValue($"Tags deleted at {DateTime.Now.ToString("T")}.");

            try
            {
                _unitOfWork.Tags.DeleteRange(items);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                returnValue.SetErrorMessage("An error has occurred while deleting the tags");
            }

            return returnValue;
        }

        public IEnumerable<Tag> FilterForDataTable(string searchValue, IEnumerable<Tag> items)
        {
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                var searchableProperties = typeof(TagTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));
                items = items.Where(items.GetSearchExpression(searchValue, searchableProperties).Compile());
            }
            return items;
        }

        public async Task<(IEnumerable<TagTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters)
        {
            var items = await _unitOfWork.Tags.GetAllAsync();
            int recordsTotal = items.Count();
            if (!string.IsNullOrWhiteSpace(parameters.Search.Value))
            {
                items = FilterForDataTable(parameters.Search.Value, items);
            }
            items = OrderForDataTable(parameters.Order[0].Column, parameters.Order[0].Dir, items);
            return (items.MapToTableViewModel(), recordsTotal, items.Count());
        }

        public async Task<IEnumerable<TagViewModel>> GetAllTags()
        {
            logger.LogInformation("TagService > GetAllTags()");
            var items = await _unitOfWork.Tags.GetAllAsync();
            logger.LogInformation("Tags loaded: {0}", items.Count());
            return items.MapToViewModel();
        }

        public IEnumerable<Tag> OrderForDataTable(int column, string direction, IEnumerable<Tag> items)
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

        public async Task<ReturnValue> Save(TagEditModel tagEditModel)
        {
            logger.LogInformation("CmsService > Save(TagEditModel: {0})", tagEditModel.ToString());

            var returnValue = new ReturnValue($"Tag '{tagEditModel.Name}' saved.");

            try
            {
                if (tagEditModel.IsNew)
                {
                    logger.LogInformation("New tag");
                    var tag = tagEditModel.MapToModel();
                    tag.WebsiteId = Instance.Id;

                    await unitOfWork.Tags.Insert(tag);
                }
                else
                {
                    logger.LogInformation("Update tag");
                    var tag = tagEditModel.MapToModel(await unitOfWork.Tags.GetByIdAsync(tagEditModel.VanityId));
                    tag.WebsiteId = Instance.Id;

                    _unitOfWork.Tags.Update(tag);
                }

                await _unitOfWork.Save();
                logger.LogInformation("Tag saved");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                returnValue.SetErrorMessage("An error has occurred while saving the tag");
            }

            return returnValue;
        }

        public TagEditModel SetupEditModel()
        {
            logger.LogInformation("CmsService > SetupEditModel()");
            return new TagEditModel();
        }

        public async Task<TagEditModel> SetupEditModel(Guid id)
        {
            logger.LogInformation("CmsService > SetupTagEditModel(id: {0})", id);
            var item = await _unitOfWork.Tags.GetByIdAsync(id);
            logger.LogInformation("Tag: {0}", item.ToString());
            return item.MapToEditModel();
        }
    }
}
