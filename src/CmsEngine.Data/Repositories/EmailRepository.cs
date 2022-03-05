namespace CmsEngine.Data.Repositories;

public class EmailRepository : Repository<Email>, IEmailRepository
{
    public EmailRepository(CmsEngineContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Email>> GetOrderedByDate()
    {
        return await Get().OrderByDescending(o => o.DateReceived)
                          .ToListAsync();
    }
}
