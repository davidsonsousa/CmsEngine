using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            //var retValue = _mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserViewModel>>(listItems);
            var retValue = _mapper.Map<IEnumerable<UserViewModel>>(listItems);
            var test = _mapper.Map<UserViewModel>(listItems.FirstOrDefault());

            return retValue;
        }

        public async Task<IViewModel> GetUserById(Guid id)
        {
            var item = await _userManager.FindByIdAsync(id.ToString());
            return _mapper.Map<ApplicationUser, UserViewModel>(item);
        }

        public async Task<IViewModel> GetUserByUsername(string userName)
        {
            var item = await _userManager.FindByNameAsync(userName);
            return _mapper.Map<ApplicationUser, UserViewModel>(item);
        }

        #endregion

        #region Setup

        public IEditModel SetupUserEditModel()
        {
            return new UserEditModel();
        }

        public async Task<IEditModel> SetupUserEditModel(Guid id)
        {
            var item = await _userManager.FindByIdAsync(id.ToString());
            return _mapper.Map<ApplicationUser, UserEditModel>(item);
        }

        public async Task<IEditModel> SetupUserEditModel(string userName)
        {
            var item = await _userManager.FindByNameAsync(userName);
            return _mapper.Map<ApplicationUser, UserEditModel>(item);
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
                PrepareUserForSaving(editModel).GetAwaiter();

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

        public async Task<ReturnValue> DeleteUser(Guid id)
        {
            var returnValue = new ReturnValue();
            try
            {
                var user = _userManager.Users.Where(q => q.Id == id.ToString()).FirstOrDefault();
                var identityResult = await _userManager.DeleteAsync(user);
                returnValue.IsError = !identityResult.Succeeded;

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

        public async Task<ReturnValue> DeleteUser(string userName)
        {
            var returnValue = new ReturnValue();
            try
            {
                var user = _userManager.Users.Where(q => q.UserName == userName).FirstOrDefault();
                var identityResult = await _userManager.DeleteAsync(user);
                returnValue.IsError = !identityResult.Succeeded;

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
                var tempItems = _mapper.Map<IEnumerable<UserViewModel>, IEnumerable<ApplicationUser>>(items);
                items = _mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserViewModel>>(tempItems.Where(lambda));
            }

            return items;
        }

        public IEnumerable<IViewModel> OrderUser(int orderColumn, string orderDirection, IEnumerable<IViewModel> listItems)
        {
            try
            {
                var listUsers = _mapper.Map<IEnumerable<IViewModel>, IEnumerable<UserViewModel>>(listItems);

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

        private async Task PrepareUserForSaving(IEditModel editModel)
        {
            ApplicationUser user;

            if (editModel.IsNew)
            {
                user = _mapper.Map<UserEditModel, ApplicationUser>((UserEditModel)editModel);
                await _userManager.CreateAsync(user);
            }
            else
            {
                user = await _userManager.FindByIdAsync(editModel.VanityId.ToString());
                _mapper.Map((UserEditModel)editModel, user);
                await _userManager.UpdateAsync(user);
            }
        }

        #endregion
    }
}
