using System;
using System.Collections.Generic;

namespace CmsEngine.Data.AccessLayer
{
    public interface IRepositoryMany<T> : IDisposable
    {
        /// <summary>
        /// Inserts multiple records in the database
        /// </summary>
        /// <param name="entities"></param>
        void InsertMany(IEnumerable<T> entities);

        /// <summary>
        /// Updates multiple records in the database
        /// </summary>
        /// <param name="entities"></param>
        void UpdateMany(IEnumerable<T> entities);

        /// <summary>
        /// Deletes multiple records in the database
        /// </summary>
        /// <param name="entities"></param>
        void DeleteMany(IEnumerable<T> entities);
    }
}
