using System.Collections.Generic;

namespace CMSEngine.ViewModels
{
    public class BaseViewModel<T> : IViewModel where T : class
    {
        public bool IsDialog { get; set; }

        public IEnumerable<T> Items { get; set; }
        public T Item { get; set; }
    }
}
