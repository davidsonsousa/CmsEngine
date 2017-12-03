using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using CmsEngine.Attributes;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Extensions;
using CmsEngine.Helpers;
using CmsEngine.Utils;
using Microsoft.AspNetCore.Http;

namespace CmsEngine.Services
{
    public abstract class BaseService<T> where T : BaseModel
    {
        private readonly IRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Website _websiteInstance;

        #region Properties

        //public IPrincipal CurrentUser
        //{
        //    get
        //    {
        //        return _httpContextAccessor.HttpContext.User;
        //    }
        //}

        public Website WebsiteInstance
        {
            get
            {
                if (_websiteInstance == null)
                {
                    _websiteInstance = _unitOfWork.GetRepository<Website>().Get(q => q.SiteUrl == _httpContextAccessor.HttpContext.Request.Host.Host).FirstOrDefault();
                }

                return _websiteInstance;
            }
        }

        /// <summary>
        /// Repository used by the Service
        /// </summary>
        protected IRepository<T> Repository
        {
            get { return _repository; }
        }

        protected IMapper Mapper
        {
            get { return _mapper; }
        }

        /// <summary>
        /// Unit of work used by the Service
        /// </summary>
        protected IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        #endregion

        protected internal BaseService(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor hca)
        {
            _repository = uow.GetRepository<T>();
            _unitOfWork = uow;
            _mapper = mapper;
            _httpContextAccessor = hca;
        }

        public abstract IEnumerable<IViewModel> Filter(string searchTerm, IEnumerable<IViewModel> listItems);

        public abstract IEnumerable<IViewModel> Order(int orderColumn, string orderDirection, IEnumerable<IViewModel> listItems);

        public DataTableViewModel BuildDataTable(IEnumerable<IViewModel> listItems)
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

            DataTableViewModel dataTableViewModel;

            dataTableViewModel = new DataTableViewModel
            {
                Data = listString,
                RecordsTotal = this.GetAllReadOnly().Count(),
                RecordsFiltered = listItems.Count(),
                Draw = 0
            };

            return dataTableViewModel;
        }

        #region Get

        /// <summary>
        /// Get all items
        /// </summary>
        /// <returns></returns>
        protected IQueryable<T> GetAll()
        {
            IQueryable<T> listItems;

            try
            {
                listItems = Repository.Get(q => q.IsDeleted == false);
            }
            catch
            {
                throw;
            }

            return listItems;
        }

        public abstract IEnumerable<IViewModel> GetAllReadOnly();

        public abstract IViewModel GetById(int id);

        public abstract IViewModel GetById(Guid id);

        #endregion

        #region Setup View and Edit models

        public abstract IEditModel SetupEditModel();

        public abstract IEditModel SetupEditModel(int id);

        public abstract IEditModel SetupEditModel(Guid id);

        #endregion

        public abstract ReturnValue Save(IEditModel editModel);

        public abstract ReturnValue Delete(Guid id);

        public virtual ReturnValue BulkDelete(Guid[] id)
        {
            var returnValue = new ReturnValue();
            try
            {
                Repository.BulkUpdate(q => id.Contains(q.VanityId), u => u.IsDeleted = true);

                UnitOfWork.Save();

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

        public abstract ReturnValue Delete(int id);

        #region Helpers

        private string PrepareProperty(IViewModel item, System.Reflection.PropertyInfo property)
        {
            string propertyValue = item.GetType().GetProperty(property.Name).GetValue(item)?.ToString() ?? "";

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

                propertyValue = $"<span class=\"label label-{generalStatus.ToString().ToLowerInvariant()}\">{propertyValue.ToEnum<DocumentStatus>().GetDescription()}</status-label>" ?? "";
            }

            return propertyValue;
        }

        protected virtual T GetItemById(int id)
        {
            T item;

            try
            {
                item = this.GetAll().Where(q => q.Id == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }

            return item;
        }

        protected virtual T GetItemById(Guid id)
        {
            T item;

            try
            {
                item = this.GetAll().Where(q => q.VanityId == id).FirstOrDefault();
            }
            catch
            {
                throw;
            }

            return item;
        }

        protected virtual Func<T, bool> PrepareFilter(string searchTerm, IEnumerable<PropertyInfo> searchableProperties)
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

        protected abstract ReturnValue Delete(T item);

        protected abstract void PrepareForSaving(IEditModel editModel);

        protected IEnumerable<CheckboxEditModel> GetCheckboxListCategory()
        {
            var categoryRepository = UnitOfWork.GetRepository<Category>();
            var categoryList = categoryRepository.GetReadOnly(q => q.IsDeleted == false);
            var checkBoxList = new List<CheckboxEditModel>();

            foreach (var category in categoryList)
            {
                checkBoxList.Add(new CheckboxEditModel
                {
                    Label = category.Name,
                    Value = category.VanityId.ToString(),
                    Enabled = true
                });
            }

            return checkBoxList;
        }

        #endregion
    }
}
