using System.Collections.Generic;

namespace CmsEngine.Data.ViewModels
{
    public class DataTableViewModel<T> where T: IViewModel
    {
        public IEnumerable<T> Items { get; set; }
    }
}
