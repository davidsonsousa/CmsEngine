using CmsEngine.Domain.Attributes;

namespace CmsEngine.Domain.ViewModels.DataTableViewModels
{
    public class CategoryTableViewModel : BaseViewModel, IViewModel
    {
        [Searchable, Orderable, ShowOnDataTable(0)]
        public string Name { get; set; }

        [Searchable, Orderable, ShowOnDataTable(1)]
        public string Slug { get; set; }

        [Searchable, Orderable, ShowOnDataTable(2)]
        public string Description { get; set; }
    }
}
