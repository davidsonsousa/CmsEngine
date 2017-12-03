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
    public sealed class PageService : BaseService<Page>
    {
        public PageService(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca) : base(uow, mapper, hca)
        {
        }

        public override IEnumerable<IViewModel> GetAllReadOnly()
        {
            IEnumerable<Page> listItems;

            try
            {
                listItems = Repository.GetReadOnly(q => q.IsDeleted == false);
            }
            catch
            {
                throw;
            }

            return Mapper.Map<IEnumerable<Page>, IEnumerable<PageViewModel>>(listItems);
        }

        public override IViewModel GetById(int id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Page, PageViewModel>(item);
        }

        public override IViewModel GetById(Guid id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Page, PageViewModel>(item);
        }

        public override ReturnValue Delete(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var page = this.GetAll().Where(q => q.VanityId == id).FirstOrDefault();
                returnValue = this.Delete(page);
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the page";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Delete(int id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var page = this.GetAll().Where(q => q.Id == id).FirstOrDefault();
                returnValue = this.Delete(page);
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the page";
                throw;
            }

            return returnValue;
        }

        public override ReturnValue Save(IEditModel editModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Page '{((PageEditModel)editModel).Title}' saved at {DateTime.Now.ToString("T")}."
            };

            try
            {
                PrepareForSaving(editModel);

                UnitOfWork.Save();
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while saving the page";
                throw;
            }

            return returnValue;
        }

        public override IEditModel SetupEditModel()
        {
            return new PageEditModel();
        }

        public override IEditModel SetupEditModel(int id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Page, PageEditModel>(item);
        }

        public override IEditModel SetupEditModel(Guid id)
        {
            var item = this.GetItemById(id);
            return Mapper.Map<Page, PageEditModel>(item);
        }

        public override IEnumerable<IViewModel> Filter(string searchTerm, IEnumerable<IViewModel> listItems)
        {
            var items = (IEnumerable<PageViewModel>)listItems;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(PageViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = this.PrepareFilter(searchTerm, searchableProperties);

                // TODO: There must be a way to improve this
                var tempItems = Mapper.Map<IEnumerable<PageViewModel>, IEnumerable<Page>>(items);
                items = Mapper.Map<IEnumerable<Page>, IEnumerable<PageViewModel>>(tempItems.Where(lambda));
            }

            return items;
        }

        public override IEnumerable<IViewModel> Order(int orderColumn, string orderDirection, IEnumerable<IViewModel> listItems)
        {
            try
            {
                var listPages = Mapper.Map<IEnumerable<IViewModel>, IEnumerable<PageViewModel>>(listItems);

                switch (orderColumn)
                {
                    case 1:
                    case 0:
                    default:
                        listItems = orderDirection == "asc" ? listPages.OrderBy(o => o.Title) : listPages.OrderByDescending(o => o.Title);
                        break;
                }
            }
            catch
            {
                throw;
            }

            return listItems;
        }

        protected override ReturnValue Delete(Page item)
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
                returnValue.Message = $"Page '{item.Title}' deleted at {DateTime.Now.ToString("T")}.";
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the page";
                throw;
            }

            return returnValue;
        }

        protected override void PrepareForSaving(IEditModel editModel)
        {
            var page = new Page();
            editModel.MapTo(page);

            page.WebsiteId = WebsiteInstance.Id;

            if (page.IsNew)
            {
                Repository.Insert(page);
            }
            else
            {
                Repository.Update(page);
            }
        }
    }
}
