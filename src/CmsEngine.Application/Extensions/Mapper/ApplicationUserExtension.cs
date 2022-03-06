namespace CmsEngine.Application.Extensions.Mapper;

public static class ApplicationUserExtensions
{
    /// <summary>
    /// Maps ApplicationUser model into a UserEditModel
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static UserEditModel MapToEditModel(this ApplicationUser item)
    {
        return new UserEditModel
        {
            VanityId = Guid.Parse(item.Id),
            Name = item.Name,
            Surname = item.Surname,
            Email = item.Email,
            UserName = item.UserName
        };
    }

    /// <summary>
    /// Maps ApplicationUser model into a UserViewModel
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static UserViewModel MapToViewModel(this ApplicationUser item)
    {
        return new UserViewModel
        {
            VanityId = Guid.Parse(item.Id),
            Name = item.Name,
            Surname = item.Surname,
            Email = item.Email,
            UserName = item.UserName
        };
    }

    /// <summary>
    /// Maps Name, Surname, Email and UserName
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static IEnumerable<UserViewModel> MapToViewModelSimple(this IEnumerable<ApplicationUser> users)
    {
        var viewModels = new List<UserViewModel>();

        foreach (var item in users)
        {
            viewModels.Add(new UserViewModel
            {
                Name = item.Name,
                Surname = item.Surname,
                Email = item.Email,
                UserName = item.UserName
            });
        }

        return viewModels;
    }

    /// <summary>
    /// Maps a UserEditModel into a ApplicationUser
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static ApplicationUser MapToModel(this UserEditModel item)
    {
        return new ApplicationUser
        {
            Id = item.VanityId.ToString(),
            Name = item.Name,
            Surname = item.Surname,
            Email = item.Email,
            UserName = item.UserName
        };
    }

    /// <summary>
    /// Maps a UserEditModel into a specific ApplicationUser
    /// </summary>
    /// <param name="item"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static ApplicationUser MapToModel(this UserEditModel item, ApplicationUser user)
    {
        user.Id = item.VanityId.ToString();
        user.Name = item.Name;
        user.Surname = item.Surname;
        user.Email = item.Email;
        user.UserName = item.UserName;

        return user;
    }

    ///// <summary>
    ///// Maps an IEnumerable<ApplicationUser> into an IEnumerable<ApplicationUserTableViewModel>
    ///// </summary>
    ///// <param name="tags"></param>
    ///// <returns></returns>
    //public static IEnumerable<ApplicationUserTableViewModel> MapToTableViewModel(this IEnumerable<ApplicationUser> tags)
    //{
    //    var tableViewModel = new List<ApplicationUserTableViewModel>();

    //    foreach (var item in tags)
    //    {
    //        tableViewModel.Add(new ApplicationUserTableViewModel
    //        {
    //            VanityId = Guid.Parse(item.Id),
    //            Name = item.Name,
    //            Surname = item.Surname,
    //            Email = item.Email,
    //            UserName = item.UserName
    //        });
    //    }

    //    return tableViewModel;
    //}
}
