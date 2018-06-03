using System;
using System.Collections.Generic;
using System.Linq;
using CmsEngine.Attributes;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Utils;

namespace CmsEngine
{
    public sealed partial class CmsService
    {
        #region Get

        public IEnumerable<IViewModel> GetAllTagsReadOnly(int count = 0)
        {
            IEnumerable<Tag> listItems;

            try
            {
                listItems = _unitOfWork.Tags.GetReadOnly(q => q.IsDeleted == false, count);
            }
            catch
            {
                throw;
            }

            return _mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(listItems);
        }

        public IViewModel GetTagById(int id)
        {
            var item = this.GetById<Tag>(id);
            return _mapper.Map<Tag, TagViewModel>(item);
        }

        public IViewModel GetTagById(Guid id)
        {
            var item = this.GetById<Tag>(id);
            return _mapper.Map<Tag, TagViewModel>(item);
        }

        #endregion

        #region Setup

        public IEditModel SetupTagEditModel()
        {
            return new TagEditModel();
        }

        public IEditModel SetupTagEditModel(int id)
        {
            var item = this.GetById<Tag>(id);
            return _mapper.Map<Tag, TagEditModel>(item);
        }

        public IEditModel SetupTagEditModel(Guid id)
        {
            var item = this.GetById<Tag>(id);
            return _mapper.Map<Tag, TagEditModel>(item);
        }

        #endregion

        #region Save

        public ReturnValue SaveTag(IEditModel editModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Tag '{((TagEditModel)editModel).Name}' saved at {DateTime.Now.ToString("T")}"
            };

            try
            {
                PrepareTagForSaving(editModel);

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

        #endregion

        #region Delete

        public ReturnValue DeleteTag(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var tag = this.GetAll<Tag>().Where(q => q.VanityId == id).FirstOrDefault();
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

        public ReturnValue DeleteTag(int id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var tag = this.GetAll<Tag>().Where(q => q.Id == id).FirstOrDefault();
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

        #endregion

        #region DataTable

        public IEnumerable<IViewModel> FilterTag(string searchTerm, IEnumerable<IViewModel> listItems)
        {
            var items = (IEnumerable<TagViewModel>)listItems;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(TagViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = this.PrepareFilter<Tag>(searchTerm, searchableProperties);

                // TODO: There must be a way to improve this
                var tempItems = _mapper.Map<IEnumerable<TagViewModel>, IEnumerable<Tag>>(items);
                items = _mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(tempItems.Where(lambda));
            }

            return items;
        }

        public IEnumerable<IViewModel> OrderTag(int orderColumn, string orderDirection, IEnumerable<IViewModel> listItems)
        {
            try
            {
                var listTags = _mapper.Map<IEnumerable<IViewModel>, IEnumerable<TagViewModel>>(listItems);

                switch (orderColumn)
                {
                    case 1:
                    case 0:
                    default:
                        listItems = orderDirection == "asc" ? listTags.OrderBy(o => o.Name) : listTags.OrderByDescending(o => o.Name);
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

        private void PrepareTagForSaving(IEditModel editModel)
        {
            Tag tag;

            if (editModel.IsNew)
            {
                tag = _mapper.Map<TagEditModel, Tag>((TagEditModel)editModel);
                tag.WebsiteId = _instanceId;

                _unitOfWork.Tags.Insert(tag);
            }
            else
            {
                tag = this.GetById<Tag>(editModel.VanityId);
                _mapper.Map((TagEditModel)editModel, tag);
                tag.WebsiteId = _instanceId;

                _unitOfWork.Tags.Update(tag);
            }
        }

        #endregion
    }
}
