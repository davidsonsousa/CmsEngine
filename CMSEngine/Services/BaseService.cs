using CMSEngine.Data.AccessLayer;
using CMSEngine.Data.Models;
using CMSEngine.Extensions;
using CMSEngine.Utils;
using CMSEngine.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMSEngine.Services
{
    public abstract class BaseService<T> where T: BaseModel
    {
        private readonly IRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        #region Properties

        //public IPrincipal CurrentUser
        //{
        //    get
        //    {
        //        return _httpContextAccessor.HttpContext.User;
        //    }
        //}

        #endregion

        protected internal BaseService(IUnitOfWork uow/*, IHttpContextAccessor hca*/)
        {
            _repository = uow.GetRepository<T>();
            _unitOfWork = uow;
            //_httpContextAccessor = hca;
        }

        protected IRepository<T> Repository
        {
            get { return _repository; }
        }

        protected IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        //public IEnumerable<T> Filter(string searchTerm, IEnumerable<T> listItems)
        //{
        //    if (!string.IsNullOrWhiteSpace(searchTerm))
        //    {
        //        var searchableProperties = typeof(T).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));
        //        List<ExpressionFilter> expressionFilter = new List<ExpressionFilter>();

        //        foreach (var property in searchableProperties)
        //        {
        //            expressionFilter.Add(new ExpressionFilter
        //            {
        //                PropertyName = property.Name,
        //                Operation = Operations.Contains,
        //                Value = searchTerm
        //            });
        //        }

        //        var lambda = ExpressionBuilder.GetExpression<T>(expressionFilter, LogicalOperators.Or).Compile();
        //        listItems = listItems.Where(lambda);
        //    }

        //    return listItems;
        //}

        //public abstract IEnumerable<T> Order(int orderColumn, string orderDirection, IEnumerable<T> listItems);

        //public DataTableViewModel BuildDataTable(IEnumerable<T> listItems)
        //{
        //    List<List<string>> listString = new List<List<string>>();

        //    foreach (var item in listItems)
        //    {
        //        // Get the properties which should appear in the DataTable
        //        var itemProperties = item.GetType()
        //                                 .GetProperties()
        //                                 .Where(p => Attribute.IsDefined(p, typeof(ShowOnDataTable)))
        //                                 .OrderBy(o => o.GetCustomAttributes(false).OfType<ShowOnDataTable>().First().Order);

        //        var listPropertes = new List<string>();

        //        // An empty value must *always* be the first property because of the checkboxes
        //        listPropertes.Add(string.Empty);

        //        // Loop through and add the properties found
        //        foreach (var property in itemProperties)
        //        {
        //            listPropertes.Add(PrepareProperty(item, property));
        //        }

        //        // VanityId must *always* be the last property
        //        listPropertes.Add(item.VanityId.ToString());

        //        listString.Add(listPropertes);
        //    }

        //    DataTableViewModel dataTableViewModel;

        //    dataTableViewModel = new DataTableViewModel
        //    {
        //        Data = listString,
        //        RecordsTotal = this.GetAllReadOnly().Count(),
        //        RecordsFiltered = listItems.Count(),
        //        Draw = 0
        //    };

        //    return dataTableViewModel;
        //}

        #region Get

        public virtual IQueryable<T> GetAll()
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

        public virtual IEnumerable<T> GetAllReadOnly()
        {
            IEnumerable<T> listItems;

            try
            {
                listItems = Repository.GetReadOnly(q => q.IsDeleted == false);
            }
            catch
            {
                throw;
            }

            return listItems;
        }

        public virtual T GetById(int id)
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

        public virtual T GetByVanityId(Guid vanitId)
        {
            T item;

            try
            {
                item = this.GetAll().Where(q => q.VanityId == vanitId).FirstOrDefault();
            }
            catch
            {
                throw;
            }

            return item;
        }

        #endregion

        #region Setup

        public abstract IViewModel SetupViewModel();

        public virtual IViewModel SetupViewModel(int id)
        {
            var item = this.GetById(id);
            return this.SetupViewModel(item);
        }

        public virtual IViewModel SetupViewModel(Guid vanityId)
        {
            var item = this.GetByVanityId(vanityId);
            return this.SetupViewModel(item);
        }

        #endregion

        public abstract ReturnValue Save(IViewModel viewModel);
        public abstract ReturnValue Delete(Guid vanityId);
        public abstract ReturnValue BulkDelete(Guid[] vanityId);
        public abstract ReturnValue Delete(int id);

        #region Helpers

        private IViewModel SetupViewModel(T item)
        {
            var itemViewModel = this.SetupViewModel() as BaseViewModel<T>;

            try
            {
                itemViewModel.Item = item;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return itemViewModel;
        }

        private string PrepareProperty(T item, System.Reflection.PropertyInfo property)
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

        protected abstract ReturnValue Delete(T item);

        protected abstract void PrepareForSaving(IViewModel viewModel);

        #endregion
    }
}
