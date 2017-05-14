using CmsEngine.Attributes;

namespace CmsEngine.Data.ViewModels
{
    public class TagViewModel: IViewModel
    {
        [Searchable, Orderable, ShowOnDataTable(0)]
        public string Name { get; set; }

        [Searchable, Orderable, ShowOnDataTable(1)]
        public string Slug { get; set; }
    }
}
