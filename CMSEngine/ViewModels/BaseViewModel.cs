using System.Collections.Generic;

namespace CmsEngine.ViewModels
{
    public class BaseViewModel<T> : IViewModel where T : class
    {
        public bool IsDialog { get; set; }

        public IEnumerable<T> Items { get; set; }
        public T Item { get; set; }
    }
}
