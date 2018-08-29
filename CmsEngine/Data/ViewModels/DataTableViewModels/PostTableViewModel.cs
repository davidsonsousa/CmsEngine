using System;
using CmsEngine.Attributes;

namespace CmsEngine.Data.ViewModels.DataTableViewModels
{
    public class PostTableViewModel : BaseViewModel
    {
        [Searchable, Orderable, ShowOnDataTable(0)]
        public string Title { get; set; }

        [Searchable, Orderable, ShowOnDataTable(2)]
        public string Slug { get; set; }

        [Searchable, Orderable, ShowOnDataTable(1)]
        public string Description { get; set; }

        public string DocumentContent { get; set; }

        [Searchable, Orderable, ShowOnDataTable(3)]
        public string Author { get; set; }

        [Orderable, ShowOnDataTable(5)]
        public DocumentStatus Status { get; set; }

        [Orderable, ShowOnDataTable(4)]
        public DateTime PublishedOn { get; set; }
    }
}
