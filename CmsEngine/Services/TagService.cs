using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IEnumerable<T> GetAllTagsReadOnly<T>(int count = 0) where T : IViewModel
        {
            IEnumerable<Tag> listItems = this.GetAllReadOnly<Tag>(count);
            return _mapper.Map<IEnumerable<Tag>, IEnumerable<T>>(listItems);
        }

        public (IEnumerable<IViewModel> Data, int RecordsCount) GetTagsForDataTable(DataParameters parameters)
        {
            var items = _unitOfWork.Tags.Get();

            if (!string.IsNullOrWhiteSpace(parameters.Search.Value))
            {
                items = this.FilterTag(parameters.Search.Value, items);
            }

            items = this.OrderTag(parameters.Order[0].Column, parameters.Order[0].Dir, items);

            int recordsCount = items.Count();

            return (_mapper.Map<IEnumerable<Tag>, IEnumerable<TagTableViewModel>>(items.Skip(parameters.Start).Take(parameters.Length).ToList()), recordsCount);
        }

        public async Task<IViewModel> GetTagById(int id)
        {
            var item = await _unitOfWork.Tags.GetById(id);
            return _mapper.Map<Tag, TagViewModel>(item);
        }

        public async Task<IViewModel> GetTagById(Guid id)
        {
            var item = await _unitOfWork.Tags.GetById(id);
            return _mapper.Map<Tag, TagViewModel>(item);
        }

        public IEditModel SetupTagEditModel()
        {
            return new TagEditModel();
        }

        public async Task<IEditModel> SetupTagEditModel(int id)
        {
            var item = await _unitOfWork.Tags.GetById(id);
            return _mapper.Map<Tag, TagEditModel>(item);
        }

        public async Task<IEditModel> SetupTagEditModel(Guid id)
        {
            var item = await _unitOfWork.Tags.GetById(id);
            return _mapper.Map<Tag, TagEditModel>(item);
        }

        public async Task<ReturnValue> SaveTag(IEditModel editModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Tag '{((TagEditModel)editModel).Name}' saved at {DateTime.Now.ToString("T")}"
            };

            try
            {
                await PrepareTagForSaving(editModel);

                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while saving the tag";
                returnValue.Exception = ex.Message;
                throw;
            }

            return returnValue;
        }

        public async Task<ReturnValue> DeleteTag(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var tag = await _unitOfWork.Tags.GetById(id);
                returnValue = this.Delete(tag);

                if (!returnValue.IsError)
                {
                    returnValue.Message = $"Tag '{tag.Name}' deleted at {DateTime.Now.ToString("T")}.";
                }
                else
                {
                    returnValue.Message = "An error has occurred while deleting the tag";
                }
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the tag";
                throw;
            }

            return returnValue;
        }

        public async Task<ReturnValue> DeleteTag(int id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var tag = await _unitOfWork.Tags.GetById(id);
                returnValue = this.Delete(tag);

                if (!returnValue.IsError)
                {
                    returnValue.Message = $"Tag '{tag.Name}' deleted at {DateTime.Now.ToString("T")}.";
                }
                else
                {
                    returnValue.Message = "An error has occurred while deleting the tag";
                }
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the tag";
                throw;
            }

            return returnValue;
        }

        private IQueryable<Tag> FilterTag(string searchTerm, IQueryable<Tag> items)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(TagTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = this.PrepareFilter<Tag>(searchTerm, searchableProperties);
                items = items.Where(lambda);
            }

            return items;
        }

        private IQueryable<Tag> OrderTag(int orderColumn, string orderDirection, IQueryable<Tag> items)
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

        private async Task PrepareTagForSaving(IEditModel editModel)
        {
            Tag tag;

            if (editModel.IsNew)
            {
                tag = _mapper.Map<TagEditModel, Tag>((TagEditModel)editModel);
                tag.WebsiteId = Instance.Id;

                await _unitOfWork.Tags.Insert(tag);
            }
            else
            {
                tag = await _unitOfWork.Tags.GetById(editModel.VanityId);
                _mapper.Map((TagEditModel)editModel, tag);
                tag.WebsiteId = Instance.Id;

                _unitOfWork.Tags.Update(tag);
            }
        }
    }
}
