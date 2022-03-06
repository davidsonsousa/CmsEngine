namespace CmsEngine.Data.Repositories.Interfaces;

public interface IReadRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Get all records which were not marked as deleted (IsDeleted == false)
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Gets item based on condition and includes extra table
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, int count = 0);

    /// <summary>
    /// Gets items based on condition for read-only
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetReadOnlyAsync(Expression<Func<TEntity, bool>> filter = null);

    /// <summary>
    /// Get record by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity> GetByIdAsync(int id);

    /// <summary>
    /// Gets record by vanity id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity> GetByIdAsync(Guid id);

    /// <summary>
    /// Get multiple records by an array of ids
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetByMultipleIdsAsync(int[] ids);

    /// <summary>
    /// Get multiple records by an array of vanity ids
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetByMultipleIdsAsync(Guid[] ids);

    /// <summary>
    /// Get multiple records by an IEnumerable of vanity ids
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<IEnumerable<int>> GetIdsByMultipleGuidsAsync(IEnumerable<Guid> ids);
}
