namespace CmsEngine.Application.Extensions.Mapper;

public static class ApplicationUserExtensions
{
    extension(ApplicationUser item)
    {
        /// <summary>
        /// Maps ApplicationUser model into a UserEditModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public UserEditModel MapToEditModel()
        {
            return new UserEditModel
            {
                VanityId = Guid.Parse(item.Id),
                Name = item.Name,
                Surname = item.Surname,
                Email = item.Email ?? string.Empty,
                UserName = item.UserName ?? string.Empty
            };
        }

        /// <summary>
        /// Maps ApplicationUser model into a UserViewModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public UserViewModel MapToViewModel()
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
    }

    extension(IEnumerable<ApplicationUser> users)
    {
        /// <summary>
        /// Maps Name, Surname, Email and UserName
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IEnumerable<UserViewModel> MapToViewModelSimple()
        {
            return users.Select(item => new UserViewModel
            {
                Name = item.Name,
                Surname = item.Surname,
                Email = item.Email,
                UserName = item.UserName
            }).ToList();
        }
    }

    extension(UserEditModel item)
    {
        /// <summary>
        /// Maps a UserEditModel into a ApplicationUser
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ApplicationUser MapToModel()
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
        public ApplicationUser MapToModel(ApplicationUser user)
        {
            user.Id = item.VanityId.ToString();
            user.Name = item.Name;
            user.Surname = item.Surname;
            user.Email = item.Email;
            user.UserName = item.UserName;

            return user;
        }
    }
}