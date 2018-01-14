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

        public IEnumerable<IViewModel> GetAllUsersReadOnly()
        {
            IEnumerable<ApplicationUser> listItems;

            try
            {
                listItems = _userManager.Users.ToList();
            }
            catch
            {
                throw;
            }

            //var retValue = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserViewModel>>(listItems);
            var retValue = Mapper.Map<IEnumerable<UserViewModel>>(listItems);
            var test = Mapper.Map<UserViewModel>(listItems.FirstOrDefault());

            return retValue;
        }

        public IViewModel GetUserById(Guid id)
        {
            var item = _userManager.FindByIdAsync(id.ToString());
            return Mapper.Map<ApplicationUser, UserViewModel>(item.Result);
        }

        public IViewModel GetUserByUsername(string userName)
        {
            var item = _userManager.FindByNameAsync(userName);
            return Mapper.Map<ApplicationUser, UserViewModel>(item.Result);
        }

        #endregion

        #region Setup

        public IEditModel SetupUserEditModel()
        {
            return new UserEditModel();
        }

        public IEditModel SetupUserEditModel(Guid id)
        {
            var item = _userManager.FindByIdAsync(id.ToString());
            return Mapper.Map<ApplicationUser, UserEditModel>(item.Result);
        }

        public IEditModel SetupUserEditModel(string userName)
        {
            var item = _userManager.FindByNameAsync(userName);
            return Mapper.Map<ApplicationUser, UserEditModel>(item.Result);
        }

        #endregion

        #region Save

        public ReturnValue SaveUser(IEditModel editModel)
        {
            var returnValue = new ReturnValue
            {
                IsError = false,
                Message = $"User '{((UserEditModel)editModel).Name}' saved at {DateTime.Now.ToString("T")}"
            };

            try
            {
                PrepareUserForSaving(editModel);

                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while saving the user";
                returnValue.Exception = ex.Message;
                throw;
            }

            return returnValue;
        }

        #endregion

        #region Delete

        public ReturnValue DeleteUser(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var user = _userManager.Users.Where(q => q.Id == id.ToString()).FirstOrDefault();
                var identityResult = _userManager.DeleteAsync(user);
                returnValue.IsError = !identityResult.Result.Succeeded;

                if (!returnValue.IsError)
                {
                    returnValue.Message = $"User '{user.UserName}' deleted at {DateTime.Now.ToString("T")}.";
                }
                else
                {
                    returnValue.Message = "An error has occurred while deleting the user";
                }
            }
            catch (Exception ex)
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the user";
                throw;
            }

            return returnValue;
        }

        public ReturnValue DeleteUser(string userName)
        {
            var returnValue = new ReturnValue();
            try
            {
                var user = _userManager.Users.Where(q => q.UserName == userName).FirstOrDefault();
                var identityResult = _userManager.DeleteAsync(user);
                returnValue.IsError = !identityResult.Result.Succeeded;

                if (!returnValue.IsError)
                {
                    returnValue.Message = $"User '{user.UserName}' deleted at {DateTime.Now.ToString("T")}.";
                }
                else
                {
                    returnValue.Message = "An error has occurred while deleting the user";
                }
            }
            catch
            {
                returnValue.IsError = true;
                returnValue.Message = "An error has occurred while deleting the user";
                throw;
            }

            return returnValue;
        }

        #endregion

        #region DataTable

        public IEnumerable<IViewModel> FilterUser(string searchTerm, IEnumerable<IViewModel> listItems)
        {
            var items = (IEnumerable<UserViewModel>)listItems;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchableProperties = typeof(UserViewModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(Searchable)));

                var lambda = this.PrepareFilter<ApplicationUser>(searchTerm, searchableProperties);

                // TODO: There must be a way to improve this
                var tempItems = Mapper.Map<IEnumerable<UserViewModel>, IEnumerable<ApplicationUser>>(items);
                items = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserViewModel>>(tempItems.Where(lambda));
            }

            return items;
        }

        public IEnumerable<IViewModel> OrderUser(int orderColumn, string orderDirection, IEnumerable<IViewModel> listItems)
        {
            try
            {
                var listUsers = Mapper.Map<IEnumerable<IViewModel>, IEnumerable<UserViewModel>>(listItems);

                switch (orderColumn)
                {
                    case 1:
                    case 0:
                    default:
                        listItems = orderDirection == "asc" ? listUsers.OrderBy(o => o.Name) : listUsers.OrderByDescending(o => o.Name);
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

        private void PrepareUserForSaving(IEditModel editModel)
        {
            ApplicationUser user;

            if (editModel.IsNew)
            {
                user = Mapper.Map<UserEditModel, ApplicationUser>((UserEditModel)editModel);
                _userManager.CreateAsync(user);
            }
            else
            {
                user = _userManager.FindByIdAsync(editModel.VanityId.ToString()).Result;
                Mapper.Map((UserEditModel)editModel, user);
                _userManager.UpdateAsync(user);
            }
        }

        #endregion
    }
}
