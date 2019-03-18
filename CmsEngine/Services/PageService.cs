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
        #region Get

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

        #endregion

        #region Setup

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

        #endregion

        #region Save

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

        #endregion

        #region Delete

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

        #endregion

        #region DataTable

        public IEnumerable<IViewModel> FilterPage(string searchTerm, IEnumerable<IViewModel> listItems)
        {
            var items = (IEnumerable<PageTableViewModel>)listItems;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(PageTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = this.PrepareFilter<Page>(searchTerm, searchableProperties);

                // TODO: There must be a way to improve this
                var tempItems = _mapper.Map<IEnumerable<PageTableViewModel>, IEnumerable<Page>>(items);
                items = _mapper.Map<IEnumerable<Page>, IEnumerable<PageTableViewModel>>(tempItems.Where(lambda));
            }

            return items;
        }

        public IEnumerable<IViewModel> OrderPage(int orderColumn, string orderDirection, IEnumerable<IViewModel> listItems)
        {
            try
            {
                var listPages = _mapper.Map<IEnumerable<IViewModel>, IEnumerable<PageTableViewModel>>(listItems);

                switch (orderColumn)
                {
                    case 1:
                        listItems = orderDirection == "asc" ? listPages.OrderBy(o => o.Title) : listPages.OrderByDescending(o => o.Title);
                        break;
                    case 2:
                        listItems = orderDirection == "asc" ? listPages.OrderBy(o => o.Description) : listPages.OrderByDescending(o => o.Description);
                        break;
                    case 3:
                        listItems = orderDirection == "asc" ? listPages.OrderBy(o => o.Slug) : listPages.OrderByDescending(o => o.Slug);
                        break;
                    case 4:
                        listItems = orderDirection == "asc" ? listPages.OrderBy(o => o.Author.FullName) : listPages.OrderByDescending(o => o.Author.FullName);
                        break;
                    case 5:
                        listItems = orderDirection == "asc" ? listPages.OrderBy(o => o.PublishedOn) : listPages.OrderByDescending(o => o.PublishedOn);
                        break;
                    case 6:
                        listItems = orderDirection == "asc" ? listPages.OrderBy(o => o.Status) : listPages.OrderByDescending(o => o.Status);
                        break;
                    default:
                        listItems = listPages.OrderByDescending(o => o.PublishedOn);
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

            if (editModel.IsNew)
            {
                _logger.LogInformation("New page");

                page = _mapper.Map<PageEditModel, Page>((PageEditModel)editModel);
                page.WebsiteId = Instance.Id;

                _unitOfWork.Pages.Insert(page);
            }
            else
            {
                _logger.LogInformation("Update page");

                page = _unitOfWork.Pages.GetById(editModel.VanityId);
                _mapper.Map((PageEditModel)editModel, page);
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

        #endregion
    }
}
