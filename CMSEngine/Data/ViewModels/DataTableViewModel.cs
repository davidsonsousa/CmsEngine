using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsEngine.Data.ViewModels
{
    public class DataTableViewModel<T> where T: IViewModel
    {
        public IEnumerable<T> Items { get; set; }
    }
}
