namespace CmsEngine.Data.Repositories.Interfaces;

public interface IPageRepository : IReadRepository<Page>, IDataModificationRepository<Page>, IDataModificationRangeRepository<Page>
{
    Task<IEnumerable<Page>> GetOrderByDescending(Expression<Func<Page, DateTime>> orderBy);
    Task<IEnumerable<Page>> GetByStatusOrderByDescending(DocumentStatus documentStatus);
    Task<IEnumerable<Page>> GetForDataTable();
    Task<Page> GetBySlug(string slug);
    Task<Page> GetForSavingById(Guid id);
    void RemoveRelatedItems(Page page);
}
