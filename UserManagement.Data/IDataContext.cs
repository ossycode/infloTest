using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Data;

public interface IDataContext
{
    /// <summary>
    /// Get a queryable list of items asynchronously.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    Task<IQueryable<TEntity>> GetAllAsync<TEntity>() where TEntity : class;

    /// <summary>
    /// Create a new item asynchronously
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The entity to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateAsync<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Update an existing item asynchronously.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Delete an item asynchronously.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class;
}
