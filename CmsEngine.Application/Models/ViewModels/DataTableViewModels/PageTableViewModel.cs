using CmsEngine.Application.Attributes;
using CmsEngine.Core;

namespace CmsEngine.Application.ViewModels.DataTableViewModels
{
    public class PageTableViewModel : BaseViewModel, IViewModel
    {
        [Searchable, Orderable, ShowOnDataTable(0)]
        public string Title { get; set; }

        [Searchable, Orderable, ShowOnDataTable(2)]
        public string Slug { get; set; }

        [Searchable, Orderable, ShowOnDataTable(1)]
        public string Description { get; set; }

        public string DocumentContent { get; set; }

        [Searchable, Orderable, ShowOnDataTable(3)]
        public UserViewModel Author { get; set; }

        [Orderable, ShowOnDataTable(5)]
        public DocumentStatus Status { get; set; }

        [Orderable, ShowOnDataTable(4)]
        public string PublishedOn { get; set; }
    }
}
