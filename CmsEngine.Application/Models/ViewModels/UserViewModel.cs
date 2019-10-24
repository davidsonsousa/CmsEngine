using CmsEngine.Application.Attributes;

namespace CmsEngine.Application.ViewModels
{
    public class UserViewModel : BaseViewModel, IViewModel
    {
        [Searchable, Orderable, ShowOnDataTable(0)]
        public string Name { get; set; }

        [Searchable, Orderable, ShowOnDataTable(2)]
        public string Surname { get; set; }

        [Searchable, Orderable, ShowOnDataTable(1)]
        public string Email { get; set; }

        public string FullName
        {
            get
            {
                return $"{Name} {Surname}";
            }
        }

        public string UserName { get; set; }
    }
}
