using System;
using System.Collections.Generic;
using System.Linq;
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
        #region Get

        public IEnumerable<T> GetAllWebsitesReadOnly<T>(int count = 0) where T : IViewModel
        {
            IEnumerable<Website> listItems = GetAllReadOnly<Website>(count);

            return _mapper.Map<IEnumerable<Website>, IEnumerable<T>>(listItems);
        }

        public IViewModel GetWebsiteById(int id)
        {
            var item = this.GetById<Website>(id);
            return _mapper.Map<Website, WebsiteViewModel>(item);
        }

        public IViewModel GetWebsiteById(Guid id)
        {
            var item = this.GetById<Website>(id);
            return _mapper.Map<Website, WebsiteViewModel>(item);
        }

        #endregion

        #region Setup

        public IEditModel SetupWebsiteEditModel()
        {
            return new WebsiteEditModel();
        }

        public IEditModel SetupWebsiteEditModel(int id)
        {
            var item = this.GetById<Website>(id);
            return _mapper.Map<Website, WebsiteEditModel>(item);
        }

        public IEditModel SetupWebsiteEditModel(Guid id)
        {
            var item = this.GetById<Website>(id);
            return _mapper.Map<Website, WebsiteEditModel>(item);
        }

        #endregion

        #region Save

        public ReturnValue SaveWebsite(IEditModel editModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Website '{((WebsiteEditModel)editModel).Name}' saved at {DateTime.Now.ToString("T")}"
            };

            try
            {
                PrepareWebsiteForSaving(editModel);

                _unitOfWork.Save();
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

        #endregion

        #region Delete

        public ReturnValue DeleteWebsite(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var website = this.GetAll<Website>().Where(q => q.VanityId == id).FirstOrDefault();
                returnValue = this.Delete(website);

                if (!returnValue.IsError)
                {
                    returnValue.Message = $"Website '{website.Name}' deleted at {DateTime.Now.ToString("T")}.";
                }
                else
                {
                    returnValue.Message = "An error has occurred while deleting the website";
                }
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the website";
                throw;
            }

            return returnValue;
        }

        public ReturnValue DeleteWebsite(int id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var website = this.GetAll<Website>().Where(q => q.Id == id).FirstOrDefault();
                returnValue = this.Delete(website);

                if (!returnValue.IsError)
                {
                    returnValue.Message = $"Website '{website.Name}' deleted at {DateTime.Now.ToString("T")}.";
                }
                else
                {
                    returnValue.Message = "An error has occurred while deleting the website";
                }
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the website";
                throw;
            }

            return returnValue;
        }

        #endregion

        #region DataTable

        public IEnumerable<IViewModel> FilterWebsite(string searchTerm, IEnumerable<IViewModel> listItems)
        {
            var items = (IEnumerable<WebsiteTableViewModel>)listItems;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(WebsiteTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = this.PrepareFilter<Website>(searchTerm, searchableProperties);

                // TODO: There must be a way to improve this
                var tempItems = _mapper.Map<IEnumerable<WebsiteTableViewModel>, IEnumerable<Website>>(items);
                items = _mapper.Map<IEnumerable<Website>, IEnumerable<WebsiteTableViewModel>>(tempItems.Where(lambda));
            }

            return items;
        }

        public IEnumerable<IViewModel> OrderWebsite(int orderColumn, string orderDirection, IEnumerable<IViewModel> listItems)
        {
            try
            {
                var listWebsites = _mapper.Map<IEnumerable<IViewModel>, IEnumerable<WebsiteTableViewModel>>(listItems);

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

        #endregion

        #region Helpers

        private void PrepareWebsiteForSaving(IEditModel editModel)
        {
            Website website;

            if (editModel.IsNew)
            {
                website = _mapper.Map<WebsiteEditModel, Website>((WebsiteEditModel)editModel);
                _unitOfWork.Websites.Insert(website);
            }
            else
            {
                website = this.GetById<Website>(editModel.VanityId);
                _mapper.Map((WebsiteEditModel)editModel, website);

                _unitOfWork.Websites.Update(website);
            }
        }

        #endregion
    }
}
