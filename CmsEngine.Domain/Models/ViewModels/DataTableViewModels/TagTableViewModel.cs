using CmsEngine.Domain.Attributes;

namespace CmsEngine.Domain.ViewModels.DataTableViewModels
{
    public class TagTableViewModel : BaseViewModel, IViewModel
    {
        [Searchable, Orderable, ShowOnDataTable(0)]
        public string Name { get; set; }

        [Searchable, Orderable, ShowOnDataTable(1)]
        public string Slug { get; set; }
    }
}
