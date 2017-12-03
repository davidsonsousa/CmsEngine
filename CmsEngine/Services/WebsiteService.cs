using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CmsEngine.Attributes;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Utils;
using Microsoft.AspNetCore.Http;

namespace CmsEngine.Services
{
    public sealed class WebsiteService : BaseService<Website>
    {
        public WebsiteService(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca) : base(uow, mapper, hca)
        {
        }

        public override IEnumerable<IViewModel> GetAllReadOnly()
        {
            IEnumerable<Website> listItems;

            try
            {
                listItems = Repository.GetReadOnly(q => q.IsDeleted == false);
            }
            catch
            {
                throw;
            }

            return Mapper.Map<IEnumerable<Website>, IEnumerable<WebsiteViewModel>>(listItems);
        }

        public override IViewModel GetById(int id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Website, WebsiteViewModel>(item);
        }

        public override IViewModel GetById(Guid id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Website, WebsiteViewModel>(item);
        }

        public override ReturnValue Delete(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var website = this.GetAll().Where(q => q.VanityId == id).FirstOrDefault();
                returnValue = this.Delete(website);
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the website";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Delete(int id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var website = this.GetAll().Where(q => q.Id == id).FirstOrDefault();
                returnValue = this.Delete(website);
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the website";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Save(IEditModel editModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Website '{((WebsiteEditModel)editModel).Name}' saved at {DateTime.Now.ToString("T")}"
            };

            try
            {
                PrepareForSaving(editModel);

                UnitOfWork.Save();
            }
            catch (Exception ex)
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while saving the website";
                returnValue.Exception = ex.Message;
                throw;
            }

            return returnValue;
        }

        public override IEditModel SetupEditModel()
        {
            return new WebsiteEditModel();
        }

        public override IEditModel SetupEditModel(int id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Website, WebsiteEditModel>(item);
        }

        public override IEditModel SetupEditModel(Guid id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Website, WebsiteEditModel>(item);
        }

        public override IEnumerable<IViewModel> Filter(string searchTerm, IEnumerable<IViewModel> listItems)
        {
            var items = (IEnumerable<WebsiteViewModel>)listItems;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(WebsiteViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = this.PrepareFilter(searchTerm, searchableProperties);

                // TODO: There must be a way to improve this
                var tempItems = Mapper.Map<IEnumerable<WebsiteViewModel>, IEnumerable<Website>>(items);
                items = Mapper.Map<IEnumerable<Website>, IEnumerable<WebsiteViewModel>>(tempItems.Where(lambda));
            }

            return items;
        }

        public override IEnumerable<IViewModel> Order(int orderColumn, string orderDirection, IEnumerable<IViewModel> listItems)
        {
            try
            {
                var listWebsites = Mapper.Map<IEnumerable<IViewModel>, IEnumerable<WebsiteViewModel>>(listItems);

                switch (orderColumn)
                {
                    case 3:
                        listItems = orderDirection == "asc" ? listWebsites.OrderBy(o => o.Culture) : listWebsites.OrderByDescending(o => o.Culture);
                        break;
                    case 2:
                        listItems = orderDirection == "asc" ? listWebsites.OrderBy(o => o.Description) : listWebsites.OrderByDescending(o => o.Description);
                        break;
                    case 1:
                    case 0:
                    default:
                        listItems = orderDirection == "asc" ? listWebsites.OrderBy(o => o.Name) : listWebsites.OrderByDescending(o => o.Name);
                        break;
                }
            }
            catch
            {
                throw;
            }

            return listItems;
        }

        protected override ReturnValue Delete(Website item)
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
                returnValue.Message = $"Website '{item.Name}' deleted at {DateTime.Now.ToString("T")}.";
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the website";
                throw;
            }

            return returnValue;
        }

        protected override void PrepareForSaving(IEditModel editModel)
        {
            Website website;

            if (editModel.IsNew)
            {
                website = Mapper.Map<WebsiteEditModel, Website>((WebsiteEditModel)editModel);
                Repository.Insert(website);
            }
            else
            {
                website = GetItemById(editModel.VanityId);
                Mapper.Map((WebsiteEditModel)editModel, website);

                Repository.Update(website);
            }
        }
    }
}
