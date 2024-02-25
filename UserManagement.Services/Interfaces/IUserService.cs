using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Return users by active state asynchronously.
    /// </summary>
    /// <param name="isActive">Boolean indicating whether to filter by active state.</param>
    /// <returns>A task representing the asynchronous operation, yielding a collection of User entities.</returns>
    Task<IEnumerable<User>> FilterByActiveAsync(bool isActive);

    /// <summary>
    /// Get all users asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding a collection of all User entities.</returns>
    Task<IEnumerable<User>> GetAllAsync();

    /// <summary>
    /// Get a user by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, yielding the User entity with the specified ID.</returns>
    Task<User> GetByIdAsync(long id);

    /// <summary>
    /// Create a new user asynchronously.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateAsync(User user);

    /// <summary>
    /// Update an existing user asynchronously.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <returns>A task representing the asynchronous operation, yielding the updated User entity.</returns>
    Task<User> UpdateAsync(User user);

    /// <summary>
    /// Delete a user asynchronously.
    /// </summary>
    /// <param name="user">The user to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(User user);
}
