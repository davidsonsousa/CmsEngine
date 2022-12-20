namespace CmsEngine.Data.Repositories;

public class Repository<TEntity> : IReadRepository<TEntity>,
                                   IDataModificationRepository<TEntity>,
                                   IDataModificationRangeRepository<TEntity>
                                   where TEntity : BaseEntity
{
    protected readonly CmsEngineContext dbContext;

    public Repository(CmsEngineContext context)
    {
        dbContext = context ?? throw new ArgumentNullException("Repository - Context");
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await GetValidRecords().ToListAsync();
    }

    public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null, int count = 0)
    {
        var recods = GetValidRecords();

        if (filter != null)
        {
            recods = recods.Where(filter);
        }

        if (count > 0)
        {
            recods = recods.Take(count);
        }

        return recods;
    }

    public async Task<IEnumerable<TEntity>> GetReadOnlyAsync(Expression<Func<TEntity, bool>> filter = null)
    {
        var records = GetValidRecords();

        if (filter != null)
        {
            records = records.Where(filter);
        }

        return await records.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await Get(q => q.Id == id).SingleOrDefaultAsync();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await Get(q => q.VanityId == id).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<TEntity>> GetByMultipleIdsAsync(int[] ids)
    {
        return await Get(q => ids.Contains(q.Id)).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetByMultipleIdsAsync(Guid[] ids)
    {
        return await Get(q => ids.Contains(q.VanityId)).ToListAsync();
    }

    public async Task<IEnumerable<int>> GetIdsByMultipleGuidsAsync(IEnumerable<Guid> ids)
    {
        return await Get(q => ids.Contains(q.VanityId)).Select(x => x.Id).ToListAsync();
    }

    public async Task Insert(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await dbContext.AddAsync(entity);
    }

    public async Task InsertRange(IEnumerable<TEntity> entities)
    {
        if (entities is null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        await dbContext.AddRangeAsync(entities);
    }

    public void Update(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        dbContext.Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        if (entities is null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        dbContext.UpdateRange(entities);
    }

    public void Delete(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        // We never delete anything, only update the IsDelete flag
        entity.IsDeleted = true;
        Update(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        if (entities is null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        for (var i = 0; i < entities.Count(); i++)
        {
            ((List<TEntity>)entities)[i].IsDeleted = true;
        }

        // We never delete anything
        UpdateRange(entities);
    }

    public void Attach(TEntity entity)
    {
        EntityEntry dbEntityEntry = dbContext.Entry(entity);
        if (dbEntityEntry.State == EntityState.Detached)
        {
            dbContext.Attach(entity);
        }
    }

    private IQueryable<TEntity> GetValidRecords()
    {
        return dbContext.Set<TEntity>().Where(q => q.IsDeleted == false);
    }
}
