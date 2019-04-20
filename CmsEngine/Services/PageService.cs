using System;
using System.Collections.Generic;
using System.Linq;
using CmsEngine.Attributes;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Data.ViewModels.DataTableViewModels;
using CmsEngine.Utils;
using Microsoft.Extensions.Logging;

namespace CmsEngine
{
    public sealed partial class CmsService
    {
        public IEnumerable<T> GetPagesByStatusReadOnly<T>(DocumentStatus documentStatus, int count = 0) where T : IViewModel
        {
            var items = this.GetDocumentsByStatus<Page>(documentStatus, count);

            _logger.LogInformation("CmsService > GetPagesByStatusReadOnly(documentStatus: {0}, count: {1})", documentStatus, count);
            _logger.LogInformation("Pages loaded: {0}", items.Count());

            return _mapper.Map<IEnumerable<Page>, IEnumerable<T>>(items);
        }

        public IEnumerable<T> GetAllPagesReadOnly<T>(int count = 0) where T : IViewModel
        {
            IEnumerable<Page> listItems = GetAllReadOnly<Page>(count);

            _logger.LogInformation("CmsService > GetAllPagesReadOnly(count: {0})", count);
            _logger.LogInformation("Pages loaded: {0}", listItems.Count());

            return _mapper.Map<IEnumerable<Page>, IEnumerable<T>>(listItems);
        }

        public (IEnumerable<IViewModel> Data, int RecordsCount) GetPagesForDataTable(DataParameters parameters)
        {
            var items = _unitOfWork.Pages.GetAll();

            if (!string.IsNullOrWhiteSpace(parameters.Search.Value))
            {
                items = FilterPage(parameters.Search.Value, items);
            }

            items = OrderPage(parameters.Order[0].Column, parameters.Order[0].Dir, items);

            int recordsCount = items.Count();

            return (_mapper.Map<IEnumerable<Page>, IEnumerable<PageTableViewModel>>(items.Skip(parameters.Start).Take(parameters.Length).ToList()), recordsCount);
        }

        public IViewModel GetPageById(int id)
        {
            var item = _unitOfWork.Pages.GetById(id);

            _logger.LogInformation("CmsService > GetPageById(id: {0})", id);
            _logger.LogInformation("Page: {0}", item.ToString());

            return _mapper.Map<Page, PageViewModel>(item);
        }

        public IViewModel GetPageById(Guid id)
        {
            var item = _unitOfWork.Pages.GetById(id);

            _logger.LogInformation("CmsService > GetPageById(id: {0})", id);
            _logger.LogInformation("Page: {0}", item.ToString());

            return _mapper.Map<Page, PageViewModel>(item);
        }

        public IViewModel GetPageBySlug(string slug)
        {
            var item = _unitOfWork.Pages.Get(q => q.Slug == slug).SingleOrDefault();

            _logger.LogInformation("CmsService > GetPageBySlug(slug: {0})", slug);
            _logger.LogInformation("Page: {0}", item.ToString());

            return _mapper.Map<Page, PageViewModel>(item);
        }

        public IEditModel SetupPageEditModel()
        {
            _logger.LogInformation("CmsService > SetupPageEditModel()");
            return new PageEditModel();
        }

        public IEditModel SetupPageEditModel(int id)
        {
            var item = _unitOfWork.Pages.GetById(id);

            _logger.LogInformation("CmsService > SetupCategoryEditModel(id: {0})", id);
            _logger.LogInformation("Page: {0}", item.ToString());

            return _mapper.Map<Page, PageEditModel>(item);
        }

        public IEditModel SetupPageEditModel(Guid id)
        {
            var item = _unitOfWork.Pages.GetById(id);

            _logger.LogInformation("CmsService > SetupCategoryEditModel(id: {0})", id);
            _logger.LogInformation("Page: {0}", item.ToString());

            return _mapper.Map<Page, PageEditModel>(item);
        }

        public ReturnValue SavePage(IEditModel editModel)
        {
            _logger.LogInformation("CmsService > SavePage(editModel: {0})", editModel.ToString());

            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"Page '{((PageEditModel)editModel).Title}' saved at {DateTime.Now.ToString("T")}"
            };

            try
            {
                PreparePageForSaving(editModel);

                _unitOfWork.Save();
                _logger.LogInformation("Page saved");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when saving category {0}", editModel.ToString());

                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while saving the page";
                returnValue.Exception = ex.Message;
                throw;
            }

            return returnValue;
        }

        public ReturnValue DeletePage(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var page = _unitOfWork.Pages.GetById(id);
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
                var page = _unitOfWork.Pages.GetById(id);
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

        private IQueryable<Page> FilterPage(string searchTerm, IQueryable<Page> items)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(PageTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = this.PrepareFilter<Page>(searchTerm, searchableProperties);
                items = items.Where(lambda);
            }

            return items;
        }

        private IQueryable<Page> OrderPage(int orderColumn, string orderDirection, IQueryable<Page> items)
        {
            try
            {
                switch (orderColumn)
                {
                    case 1:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.Title) : items.OrderByDescending(o => o.Title);
                        break;
                    case 2:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.Description) : items.OrderByDescending(o => o.Description);
                        break;
                    case 3:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.Slug) : items.OrderByDescending(o => o.Slug);
                        break;
                    //case 4:
                    //    items = orderDirection == "asc" ? items.OrderBy(o => o.Author.FullName) : items.OrderByDescending(o => o.Author.FullName);
                    //    break;
                    case 5:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.PublishedOn) : items.OrderByDescending(o => o.PublishedOn);
                        break;
                    case 6:
                        items = orderDirection == "asc" ? items.OrderBy(o => o.Status) : items.OrderByDescending(o => o.Status);
                        break;
                    default:
                        items = items.OrderByDescending(o => o.PublishedOn);
                        break;
                }
            }
            catch
            {
                throw;
            }

            return items;
        }

        private void PreparePageForSaving(IEditModel editModel)
        {
            Page page;

            var pageEditModel = (PageEditModel)editModel;

            if (editModel.IsNew)
            {
                _logger.LogInformation("New page");

                page = _mapper.Map<PageEditModel, Page>(pageEditModel);
                page.WebsiteId = Instance.Id;

                _unitOfWork.Pages.Insert(page);
            }
            else
            {
                _logger.LogInformation("Update page");

                page = _unitOfWork.Pages.GetById(pageEditModel.VanityId);
                _mapper.Map(pageEditModel, page);
                page.WebsiteId = Instance.Id;

                _unitOfWork.Pages.Update(page);
            }

            PrepareRelatedAuthorsForPage(page);
        }

        private void PrepareRelatedAuthorsForPage(Page page)
        {
            // TODO: Improve the logic of this method

            _logger.LogInformation("Prepare related authors for page");

            if (page.PageApplicationUsers == null || page.PageApplicationUsers.Count == 0)
            {
                _unitOfWork.GetRepositoryMany<PageApplicationUser>()
                           .InsertMany(new List<PageApplicationUser>
                           {
                               new PageApplicationUser {
                                   ApplicationUserId = CurrentUser.VanityId.ToString(),
                                   Page = page
                               }
                           });
            }
        }
    }
}
