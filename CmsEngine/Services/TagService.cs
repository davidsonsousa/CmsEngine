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
    public class TagService : BaseService<Tag>
    {
        public TagService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
        {
        }

        public override IEnumerable<IViewModel> GetAllReadOnly()
        {
            IEnumerable<Tag> listItems;

            try
            {
                listItems = Repository.GetReadOnly(q => q.IsDeleted == false);
            }
            catch
            {
                throw;
            }

            return Mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(listItems);
        }

        public override IViewModel GetById(int id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Tag, TagViewModel>(item);
        }

        public override IViewModel GetById(Guid id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Tag, TagViewModel>(item);
        }

        public override ReturnValue BulkDelete(Guid[] id)
        {
            var returnValue = new ReturnValue();
            try
            {
                Repository.BulkUpdate(q => id.Contains(q.VanityId), u => new Tag { IsDeleted = true });

                returnValue.IsError = false;
                returnValue.Message = $"Selected items deleted at {DateTime.Now.ToString("T")}.";
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the tag";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Delete(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var tag = this.GetAll().Where(q => q.VanityId == id).FirstOrDefault();
                returnValue = this.Delete(tag);
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the tag";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Delete(int id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var tag = this.GetAll().Where(q => q.Id == id).FirstOrDefault();
                returnValue = this.Delete(tag);
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the tag";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Save(IEditModel editModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Tag '{((TagEditModel)editModel).Name}' saved at ${DateTime.Now.ToString("T")}."
            };

            try
            {
                PrepareForSaving(editModel);

                UnitOfWork.Save();
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while saving the tag";
                throw;
            }

            return returnValue;
        }

        public override IEditModel SetupEditModel()
        {
            return new TagEditModel();
        }

        public override IEditModel SetupEditModel(int id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Tag, TagEditModel>(item);
        }

        public override IEditModel SetupEditModel(Guid id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Tag, TagEditModel>(item);
        }

        protected override ReturnValue Delete(Tag item)
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
                returnValue.Message = $"Tag '{item.Name}' deleted at {DateTime.Now.ToString("T")}.";
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the tag";
                throw;
            }

            return returnValue;
        }

        protected override void PrepareForSaving(IEditModel editModel)
        {
            var tag = new Tag();
            editModel.MapTo(tag);

            if (tag.IsNew)
            {
                Repository.Insert(tag);
            }
            else
            {
                Repository.Update(tag);
            }
        }
    }
}
