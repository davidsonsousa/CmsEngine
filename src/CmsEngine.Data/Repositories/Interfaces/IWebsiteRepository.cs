namespace CmsEngine.Data.Repositories.Interfaces;

public interface IWebsiteRepository : IReadRepository<Website>, IDataModificationRepository<Website>, IDataModificationRangeRepository<Website>, IDisposable
{
    Website? GetWebsiteInstanceByHost(string host);

    IQueryable<Website> GetForDataTable();
}
