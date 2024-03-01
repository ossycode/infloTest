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

    /// <summary>
    /// Retrieves a single entity of type <typeparamref name="TEntity"/> based on the provided integer <paramref name="id"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to retrieve.</typeparam>
    /// <param name="id">The integer identifier of the entity.</param>
    /// <returns>A task representing the asynchronous operation, resolving to the entity if found, or <c>null</c> if not found.</returns>
    Task<TEntity?> GetEntityByIdAsync<TEntity, TId>(TId id) where TEntity : class;

}
