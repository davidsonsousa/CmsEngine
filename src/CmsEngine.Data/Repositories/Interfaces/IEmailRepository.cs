namespace CmsEngine.Data.Repositories.Interfaces;

public interface IEmailRepository : IReadRepository<Email>, IDataModificationRepository<Email>
{
    Task<IEnumerable<Email>> GetOrderedByDate();
}
