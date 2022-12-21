namespace CmsEngine.Data.Repositories;

public class PageRepository : Repository<Page>, IPageRepository
{
    public PageRepository(CmsEngineContext context) : base(context)
    {

    }

    public async Task<IEnumerable<Page>> GetOrderByDescending(Expression<Func<Page, DateTime>> orderBy)
    {
        return await Get(q => q.Status == DocumentStatus.Published && q.PublishedOn < DateTime.Now).OrderByDescending(orderBy).ToListAsync();
    }

    public async Task<IEnumerable<Page>> GetByStatusOrderByDescending(DocumentStatus documentStatus)
    {
        return await Get(q => q.Status == documentStatus && q.PublishedOn < DateTime.Now).OrderByDescending(o => o.PublishedOn).ToListAsync();
    }

    public async Task<Page?> GetBySlug(string slug)
    {
        return await Get(q => q.Slug == slug && q.PublishedOn < DateTime.Now)
                        .Select(p => new Page
                        {
                            VanityId = p.VanityId,
                            Title = p.Title,
                            Slug = p.Slug,
                            Description = p.Description,
                            DocumentContent = p.DocumentContent,
                            HeaderImage = p.HeaderImage,
                            PublishedOn = p.PublishedOn,
                            ApplicationUsers = p.PageApplicationUsers.Select(pau => pau.ApplicationUser).Select(au => new ApplicationUser
                            {
                                Id = au.Id,
                                Name = au.Name,
                                Surname = au.Surname,
                                Email = au.Email
                            })
                        })
                        .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Page>> GetForDataTable()
    {
        return await Get().Select(p => new Page
        {
            VanityId = p.VanityId,
            Title = p.Title,
            Description = p.Description,
            Slug = p.Slug,
            PublishedOn = p.PublishedOn,
            Status = p.Status,
            ApplicationUsers = p.PageApplicationUsers.Select(pau => pau.ApplicationUser).Select(au => new ApplicationUser
            {
                Id = au.Id,
                Name = au.Name,
                Surname = au.Surname,
                Email = au.Email
            })
        }).ToListAsync();
    }

    public async Task<Page?> GetForSavingById(Guid id)
    {
        return await Get(q => q.VanityId == id).Include(p => p.PageApplicationUsers)
                                               .SingleOrDefaultAsync();
    }

    public void RemoveRelatedItems(Page page)
    {
        dbContext.RemoveRange(page.PageApplicationUsers);
    }
}
