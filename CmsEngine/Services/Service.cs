using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using CmsEngine.Attributes;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Data.ViewModels.DataTableViewModels;
using CmsEngine.Extensions;
using CmsEngine.Helpers;
using CmsEngine.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CmsEngine
{
    public sealed partial class CmsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly int _instanceId;

        private InstanceViewModel _instance;

        #region Properties

        public UserViewModel CurrentUser
        {
            get
            {
                var task = Task.Run(() => GetUserByUsername(_httpContextAccessor.HttpContext.User.Identity.Name));
                return (UserViewModel)task.Result;
            }
        }

        public InstanceViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    try
                    {
                        var website = _unitOfWork.Websites.Get(q => q.SiteUrl == _httpContextAccessor.HttpContext.Request.Host.Host).SingleOrDefault();
                        if (website != null)
                        {
                            _instance = new InstanceViewModel
                            {
                                Name = website.Name,
                                Description = website.Description,
                                Culture = website.Culture,
                                UrlFormat = website.UrlFormat,
                                DateFormat = website.DateFormat,
                                SiteUrl = website.SiteUrl,
                                ArticleLimit = website.ArticleLimit,
                                ContactDetails = new ContactDetailsViewModel
                                {
                                    Address = website.Address,
                                    Phone = website.Phone,
                                    Email = website.Email,
                                    Facebook = website.Facebook,
                                    Twitter = website.Twitter,
                                    Instagram = website.Instagram,
                                    LinkedIn = website.LinkedIn
                                }
                            };
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }

                return _instance;
            }
        }

        #endregion

        public CmsService(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = uow;
            _mapper = mapper;
            _httpContextAccessor = hca;
            _userManager = userManager;

            try
            {
                var website = _unitOfWork.Websites.Get(q => q.SiteUrl == _httpContextAccessor.HttpContext.Request.Host.Host).SingleOrDefault();

                if (website != null)
                {
                    _instanceId = website.Id;
                }
            }
            catch
            {
                throw;
            }
        }

        private IQueryable<T> GetAll<T>(int count = 0) where T : BaseModel
        {
            try
            {
                var query = _unitOfWork.GetRepository<T>().Get(q => q.IsDeleted == false);

                if (count > 0)
                {
                    query = query.Take(count);
                }

                return query;
            }
            catch
            {
                throw;
            }
        }

        private IQueryable<TModel> GetDocumentsByStatus<TModel>(DocumentStatus documentStatus, int count = 0) where TModel : Document
        {
            try
            {
                var query = _unitOfWork.GetRepository<TModel>()
                                  .Get(q => q.IsDeleted == false && q.Status == documentStatus)
                                  .OrderByDescending(o => o.PublishedOn)
                                  .AsQueryable();

                if (count > 0)
                {
                    query = query.Take(count);
                }

                return query;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<IViewModel> GetAllReadOnly<TModel, TViewModel>() where TModel : BaseModel where TViewModel : BaseViewModel
        {
            IEnumerable<TModel> listItems;

            try
            {
                listItems = _unitOfWork.GetRepository<TModel>().GetReadOnly(q => q.IsDeleted == false);
            }
            catch
            {
                throw;
            }

            return _mapper.Map<IEnumerable<TModel>, IEnumerable<TViewModel>>(listItems);
        }

        public int CountRecords<T>() where T : BaseModel
        {
            try
            {
                return _unitOfWork.GetRepository<T>().GetReadOnly(q => q.IsDeleted == false).Count();
            }
            catch
            {
                throw;
            }
        }

        public ReturnValue BulkDelete<T>(Guid[] id) where T : BaseModel
        {
            var returnValue = new ReturnValue();
            try
            {
                var itemsToDelete = _unitOfWork.GetRepository<T>().GetReadOnly(q => id.Contains(q.VanityId));
                _unitOfWork.GetRepository<T>().UpdateMany(itemsToDelete.Select(x =>
                                                                            {
                                                                                x.IsDeleted = true;
                                                                                return x;
                                                                            }));

                _unitOfWork.Save();

                returnValue.IsError = false;
                returnValue.Message = $"Selected items deleted at {DateTime.Now.ToString("T")}.";
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the selected items";
                throw;
            }

            return returnValue;
        }

        public TableViewModel BuildDataTable<T>(IEnumerable<IViewModel> listItems) where T : BaseModel
        {
            var listString = new List<List<string>>();

            foreach (var item in listItems)
            {
                // Get the properties which should appear in the DataTable
                var itemProperties = item.GetType()
                                         .GetProperties()
                                         .Where(p => Attribute.IsDefined(p, typeof(ShowOnDataTable)))
                                         .OrderBy(o => o.GetCustomAttributes(false).OfType<ShowOnDataTable>().First().Order);

                // An empty value must *always* be the first property because of the checkboxes
                var listPropertes = new List<string> { string.Empty };

                // Loop through and add the properties found
                foreach (var property in itemProperties)
                {
                    listPropertes.Add(PrepareProperty(item, property));
                }

                // VanityId must *always* be the last property
                listPropertes.Add(item.VanityId.ToString());

                listString.Add(listPropertes);
            }

            return new TableViewModel
            {
                Data = listString,
                RecordsTotal = this.CountRecords<T>(),
                RecordsFiltered = listItems.Count(),
                Draw = 0
            };
        }

        #region Helpers

        private IEnumerable<CheckboxEditModel> PopulateCheckboxList<T>(IEnumerable<string> selectedItems = null) where T : BaseModel
        {
            var itemList = _unitOfWork.GetRepository<T>().GetReadOnly(q => q.IsDeleted == false);
            var checkBoxList = new List<CheckboxEditModel>();

            foreach (var item in itemList)
            {
                checkBoxList.Add(new CheckboxEditModel
                {
                    Label = item.GetType().GetProperty("Name").GetValue(item).ToString(),
                    Value = item.VanityId.ToString(),
                    Enabled = true,
                    Selected = (selectedItems == null ? false : selectedItems.Contains(item.VanityId.ToString()))
                });
            }

            return checkBoxList;
        }

        private IEnumerable<SelectListItem> PopulateSelectListItems<T>(IEnumerable<string> selectedItems = null) where T : BaseModel
        {
            var itemList = _unitOfWork.GetRepository<T>().GetReadOnly(q => q.IsDeleted == false);
            var selectListItems = new List<SelectListItem>();

            foreach (var item in itemList)
            {
                selectListItems.Add(new SelectListItem
                {
                    Text = item.GetType().GetProperty("Name").GetValue(item).ToString(),
                    Value = item.VanityId.ToString(),
                    Disabled = false,
                    Selected = (selectedItems == null ? false : selectedItems.Contains(item.VanityId.ToString()))
                });
            }

            return selectListItems;
        }

        private IEnumerable<T> GetAllReadOnly<T>(int count = 0) where T : BaseModel
        {
            IEnumerable<T> listItems;

            try
            {
                var query = _unitOfWork.GetRepository<T>().Get(q => q.IsDeleted == false);

                if (count > 0)
                {
                    query = query.Take(count);
                }

                listItems = query.ToList();
            }
            catch
            {
                throw;
            }

            return listItems;
        }

        private T GetById<T>(int id, int count = 0) where T : BaseModel
        {
            try
            {
                return this.GetAll<T>(count).Where(q => q.Id == id).SingleOrDefault();
            }
            catch
            {
                throw;
            }
        }

        private T GetById<T>(Guid id, int count = 0) where T : BaseModel
        {
            try
            {
                return this.GetAll<T>(count).Where(q => q.VanityId == id).SingleOrDefault();
            }
            catch
            {
                throw;
            }
        }

        private ReturnValue Delete<T>(T item) where T : BaseModel
        {
            var returnValue = new ReturnValue();
            try
            {
                if (item != null)
                {
                    item.IsDeleted = true;
                    _unitOfWork.GetRepository<T>().Update(item);
                }

                _unitOfWork.Save();
                returnValue.IsError = false;
            }
            catch
            {
                returnValue.IsError = true;
                throw;
            }

            return returnValue;
        }

        private string PrepareProperty(IViewModel item, PropertyInfo property)
        {
            var propertyValue = item.GetType().GetProperty(property.Name).GetValue(item)?.ToString() ?? "";

            if (property.PropertyType.Name == "DocumentStatus")
            {
                GeneralStatus generalStatus;
                switch (propertyValue)
                {
                    case "Published":
                        generalStatus = GeneralStatus.Success;
                        break;
                    case "PendingApproval":
                        generalStatus = GeneralStatus.Warning;
                        break;
                    case "Draft":
                    default:
                        generalStatus = GeneralStatus.Info;
                        break;
                }

                propertyValue = $"<span class=\"label label-{generalStatus.ToString().ToLowerInvariant()}\">{propertyValue.ToEnum<DocumentStatus>().GetName()}</status-label>" ?? "";
            }

            return propertyValue;
        }

        private Func<T, bool> PrepareFilter<T>(string searchTerm, IEnumerable<PropertyInfo> searchableProperties)
        {
            var expressionFilter = new List<ExpressionFilter>();

            foreach (var property in searchableProperties)
            {
                expressionFilter.Add(new ExpressionFilter
                {
                    PropertyName = property.Name,
                    Operation = Operations.Contains,
                    Value = searchTerm
                });
            }

            return ExpressionBuilder.GetExpression<T>(expressionFilter, LogicalOperators.Or).Compile();
        }

        #endregion
    }
}
