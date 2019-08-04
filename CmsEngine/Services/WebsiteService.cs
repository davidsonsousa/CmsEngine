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
        public IEnumerable<T> GetAllWebsitesReadOnly<T>(int count = 0) where T : IViewModel
        {
            IEnumerable<Website> listItems = this.GetAllReadOnly<Website>(count);

            return _mapper.Map<IEnumerable<Website>, IEnumerable<T>>(listItems);
        }

        public (IEnumerable<IViewModel> Data, int RecordsCount) GetWebsitesForDataTable(DataParameters parameters)
        {
            var items = _unitOfWork.Websites.Get();

            if (!string.IsNullOrWhiteSpace(parameters.Search.Value))
            {
                items = this.FilterWebsite(parameters.Search.Value, items);
            }

            items = this.OrderWebsite(parameters.Order[0].Column, parameters.Order[0].Dir, items);

            int recordsCount = items.Count();

            return (_mapper.Map<IEnumerable<Website>, IEnumerable<WebsiteTableViewModel>>(items.Skip(parameters.Start).Take(parameters.Length).ToList()), recordsCount);
        }

        public IViewModel GetWebsiteById(int id)
        {
            var item = _unitOfWork.Websites.GetById(id);
            return _mapper.Map<Website, WebsiteViewModel>(item);
        }

        public IViewModel GetWebsiteById(Guid id)
        {
            var item = _unitOfWork.Websites.GetById(id);
            return _mapper.Map<Website, WebsiteViewModel>(item);
        }

        public IEditModel SetupWebsiteEditModel()
        {
            return new WebsiteEditModel();
        }

        public IEditModel SetupWebsiteEditModel(int id)
        {
            var item = _unitOfWork.Websites.GetById(id);
            return _mapper.Map<Website, WebsiteEditModel>(item);
        }

        public IEditModel SetupWebsiteEditModel(Guid id)
        {
            var item = _unitOfWork.Websites.GetById(id);
            return _mapper.Map<Website, WebsiteEditModel>(item);
        }

        public ReturnValue SaveWebsite(IEditModel editModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Website '{((WebsiteEditModel)editModel).Name}' saved at {DateTime.Now.ToString("T")}"
            };

            try
            {
                this.PrepareWebsiteForSaving(editModel);

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

        public ReturnValue DeleteWebsite(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var website = _unitOfWork.Websites.GetById(id);
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
                var website = _unitOfWork.Websites.GetById(id);
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

        private IQueryable<Website> FilterWebsite(string searchTerm, IQueryable<Website> items)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(WebsiteTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = this.PrepareFilter<Website>(searchTerm, searchableProperties);
                items = items.Where(lambda);
            }

            return items;
        }

        private IQueryable<Website> OrderWebsite(int orderColumn, string orderDirection, IQueryable<Website> items)
        {
            try
            {
                switch (orderColumn)
                {
                    case 1:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.Name) : items.OrderByDescending(o => o.Name);
                        break;
                    case 2:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.Tagline) : items.OrderByDescending(o => o.Tagline);
                        break;
                    case 3:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.Culture) : items.OrderByDescending(o => o.Culture);
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
                website = _unitOfWork.Websites.GetById(editModel.VanityId);
                _mapper.Map((WebsiteEditModel)editModel, website);

                _unitOfWork.Websites.Update(website);
            }
        }
    }
}
