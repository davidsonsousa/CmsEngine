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

        public IEnumerable<IViewModel> GetAllPagesReadOnly(int count = 0)
        {
            IEnumerable<Page> listItems;

            try
            {
                listItems = _unitOfWork.Pages.GetReadOnly(q => q.IsDeleted == false, count);
            }
            catch
            {
                throw;
            }

            return Mapper.Map<IEnumerable<Page>, IEnumerable<PageViewModel>>(listItems);
        }

        public IViewModel GetPageById(int id)
        {
            var item = this.GetById<Page>(id);
            return Mapper.Map<Page, PageViewModel>(item);
        }

        public IViewModel GetPageById(Guid id)
        {
            var item = this.GetById<Page>(id);
            return Mapper.Map<Page, PageViewModel>(item);
        }

        #endregion

        #region Setup

        public IEditModel SetupPageEditModel()
        {
            return new PageEditModel();
        }

        public IEditModel SetupPageEditModel(int id)
        {
            var item = this.GetById<Page>(id);
            return Mapper.Map<Page, PageEditModel>(item);
        }

        public IEditModel SetupPageEditModel(Guid id)
        {
            var item = this.GetById<Page>(id);
            return Mapper.Map<Page, PageEditModel>(item);
        }

        #endregion

        #region Save

        public ReturnValue SavePage(IEditModel editModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Page '{((PageEditModel)editModel).Title}' saved at {DateTime.Now.ToString("T")}"
            };

            try
            {
                PreparePageForSaving(editModel);

                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while saving the page";
                returnValue.Exception = ex.Message;
                throw;
            }

            return returnValue;
        }

        #endregion

        #region Delete

        public ReturnValue DeletePage(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var page = this.GetAll<Page>().Where(q => q.VanityId == id).FirstOrDefault();
                returnValue = this.Delete(page);

                if (!returnValue.IsError)
                {
                    returnValue.Message = $"Page '{page.Title}' deleted at {DateTime.Now.ToString("T")}.";
                }
                else
                {
                    returnValue.Message = "An error has occurred while deleting the page";
                }
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the page";
                throw;
            }

            return returnValue;
        }

        public ReturnValue DeletePage(int id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var page = this.GetAll<Page>().Where(q => q.Id == id).FirstOrDefault();
                returnValue = this.Delete(page);

                if (!returnValue.IsError)
                {
                    returnValue.Message = $"Page '{page.Title}' deleted at {DateTime.Now.ToString("T")}.";
                }
                else
                {
                    returnValue.Message = "An error has occurred while deleting the page";
                }
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the page";
                throw;
            }

            return returnValue;
        }

        #endregion

        #region DataTable

        public IEnumerable<IViewModel> FilterPage(string searchTerm, IEnumerable<IViewModel> listItems)
        {
            var items = (IEnumerable<PageViewModel>)listItems;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(PageViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = this.PrepareFilter<Page>(searchTerm, searchableProperties);

                // TODO: There must be a way to improve this
                var tempItems = Mapper.Map<IEnumerable<PageViewModel>, IEnumerable<Page>>(items);
                items = Mapper.Map<IEnumerable<Page>, IEnumerable<PageViewModel>>(tempItems.Where(lambda));
            }

            return items;
        }

        public IEnumerable<IViewModel> OrderPage(int orderColumn, string orderDirection, IEnumerable<IViewModel> listItems)
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

        #endregion

        #region Helpers

        private void PreparePageForSaving(IEditModel editModel)
        {
            Page page;

            var pageEditModel = (PageEditModel)editModel;

            // The field is varchar(20)
            pageEditModel.Author = CurrentUser.FullName.Length > 20
                                   ? CurrentUser.FullName.Substring(0, 20)
                                   : CurrentUser.FullName;
            pageEditModel.AuthorId = CurrentUser.VanityId.ToString();

            if (editModel.IsNew)
            {
                page = Mapper.Map<PageEditModel, Page>((PageEditModel)editModel);
                page.WebsiteId = WebsiteInstance.Id;

                _unitOfWork.Pages.Insert(page);
            }
            else
            {
                page = this.GetById<Page>(editModel.VanityId);
                Mapper.Map((PageEditModel)editModel, page);
                page.WebsiteId = WebsiteInstance.Id;

                _unitOfWork.Pages.Update(page);
            }
        }

        #endregion
    }
}
