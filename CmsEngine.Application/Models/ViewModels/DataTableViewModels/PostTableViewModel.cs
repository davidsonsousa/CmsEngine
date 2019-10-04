using System;
using CmsEngine.Core;
using CmsEngine.Domain.Attributes;

namespace CmsEngine.Domain.ViewModels.DataTableViewModels
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

        [Orderable, ShowOnDataTable(3)]
        public UserViewModel Author { get; set; }

        [Orderable, ShowOnDataTable(5)]
        public DocumentStatus Status { get; set; }

        [Orderable, ShowOnDataTable(4)]
        public DateTime PublishedOn { get; set; }
    }
}
