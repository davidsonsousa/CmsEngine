using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsEngine.Application.Attributes;
using CmsEngine.Application.EditModels;
using CmsEngine.Application.Extensions;
using CmsEngine.Application.Extensions.Mapper;
using CmsEngine.Application.ViewModels.DataTableViewModels;
using CmsEngine.Core;
using CmsEngine.Data;
using CmsEngine.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CmsEngine.Application.Services
{
    public class WebsiteService : Service, IWebsiteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WebsiteService(IUnitOfWork uow, IHttpContextAccessor hca, ILogger log) : base(uow, hca, log)
        {
            _unitOfWork = uow;
        }

        public async Task<ReturnValue> Delete(Guid id)
        {
            var item = await _unitOfWork.Websites.GetByIdAsync(id);

            var returnValue = new ReturnValue($"Website '{item.Name}' deleted at {DateTime.Now.ToString("T")}.");

            try
            {
                _unitOfWork.Websites.Delete(item);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                returnValue.SetErrorMessage("An error has occurred while deleting the website");
            }

            return returnValue;
        }

        public async Task<ReturnValue> DeleteRange(Guid[] ids)
        {
            var items = await _unitOfWork.Websites.GetByMultipleIdsAsync(ids);

            var returnValue = new ReturnValue($"Websites deleted at {DateTime.Now.ToString("T")}.");

            try
            {
                _unitOfWork.Websites.DeleteRange(items);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                returnValue.SetErrorMessage("An error has occurred while deleting the websites");
            }

            return returnValue;
        }

        public IEnumerable<Website> FilterForDataTable(string searchValue, IEnumerable<Website> items)
        {
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                var searchableProperties = typeof(WebsiteTableViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));
                items = items.Where(items.GetSearchExpression(searchValue, searchableProperties).Compile());
            }
            return items;
        }

        public async Task<(IEnumerable<WebsiteTableViewModel> Data, int RecordsTotal, int RecordsFiltered)> GetForDataTable(DataParameters parameters)
        {
            var items = await _unitOfWork.Websites.GetAllAsync();
            int recordsTotal = items.Count();
            if (!string.IsNullOrWhiteSpace(parameters.Search.Value))
            {
                items = FilterForDataTable(parameters.Search.Value, items);
            }
            items = OrderForDataTable(parameters.Order[0].Column, parameters.Order[0].Dir, items);
            return (items.MapToTableViewModel(), recordsTotal, items.Count());
        }

        public IEnumerable<Website> OrderForDataTable(int column, string direction, IEnumerable<Website> items)
        {
            try
            {
                switch (column)
                {
                    case 1:
                        items = direction == "asc" ? items.OrderBy(o => o.Name) : items.OrderByDescending(o => o.Name);
                        break;
                    case 2:
                        items = direction == "asc" ? items.OrderBy(o => o.Tagline) : items.OrderByDescending(o => o.Tagline);
                        break;
                    case 3:
                        items = direction == "asc" ? items.OrderBy(o => o.Culture) : items.OrderByDescending(o => o.Culture);
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

        public async Task<ReturnValue> Save(WebsiteEditModel websiteEditModel)
        {
            logger.LogInformation("CmsService > Save(WebsiteEditModel: {0})", websiteEditModel.ToString());

            var returnValue = new ReturnValue($"Website '{websiteEditModel.Name}' saved at {DateTime.Now.ToString("T")}.");

            try
            {
                if (websiteEditModel.IsNew)
                {
                    logger.LogInformation("New website");
                    var website = websiteEditModel.MapToModel();

                    await unitOfWork.Websites.Insert(website);
                }
                else
                {
                    logger.LogInformation("Update website");
                    var website = websiteEditModel.MapToModel(await unitOfWork.Websites.GetByIdAsync(websiteEditModel.VanityId));

                    _unitOfWork.Websites.Update(website);
                }

                await _unitOfWork.Save();
                logger.LogInformation("Website saved");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                returnValue.SetErrorMessage("An error has occurred while saving the website");
            }

            return returnValue;
        }

        public WebsiteEditModel SetupEditModel()
        {
            logger.LogInformation("CmsService > SetupEditModel()");
            return new WebsiteEditModel();
        }

        public async Task<WebsiteEditModel> SetupEditModel(Guid id)
        {
            var item = await _unitOfWork.Websites.GetByIdAsync(id);
            logger.LogInformation("CmsService > SetupWebsiteEditModel(id: {0})", id);
            logger.LogInformation("Website: {0}", item.ToString());
            return item.MapToEditModel();
        }
    }
}
