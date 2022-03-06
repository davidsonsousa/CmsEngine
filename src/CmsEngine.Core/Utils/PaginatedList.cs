namespace CmsEngine.Core.Utils;

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }

    public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    public bool HasPreviousPage {
        get {
            return PageIndex > 1;
        }
    }

    public bool HasNextPage {
        get {
            return PageIndex < TotalPages;
        }
    }
}
