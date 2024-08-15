namespace CmsEngine.Data.Repositories;

public class Repository<TEntity> : IReadRepository<TEntity>,
                                   IDataModificationRepository<TEntity>,
                                   IDataModificationRangeRepository<TEntity>,
                                   IDisposable
                                   where TEntity : BaseEntity
{
    protected readonly CmsEngineContext dbContext;
    private bool disposedValue;

    public Repository(CmsEngineContext context)
    {
        ArgumentNullException.ThrowIfNull(nameof(context));
        dbContext = context;
    }

    public async Task<int> CountAsync()
    {
        return await Get().CountAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await GetValidRecords().ToListAsync();
    }

    public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null, int count = 0)
    {
        if (filter is not null && count > 0)
        {
            return GetValidRecords().Where(filter).Take(count);
        }
        else if (filter is not null)
        {
            return GetValidRecords().Where(filter);
        }
        else if (count > 0)
        {
            return GetValidRecords().Take(count);
        }
        else
        {
            return GetValidRecords();
        }
    }

    public async Task<IEnumerable<TEntity>> GetReadOnlyAsync(Expression<Func<TEntity, bool>>? filter = null)
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

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
