using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Helpers;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    private readonly ICRUDActionsLoggerService _crudActionsLogger;

    /// <summary>
    /// Initializes a new instance of the UserService class.
    /// </summary>
    /// <param name="dataAccess">The data access object.</param>
    public UserService(IDataContext dataAccess, ICRUDActionsLoggerService crudActionsLogger)
    {
        _dataAccess = dataAccess;
        _crudActionsLogger = crudActionsLogger;
    }

    /// <summary>
    /// Filters users by active state asynchronously.
    /// </summary>
    /// <param name="isActive">A boolean indicating whether to filter by active state.</param>
    /// <returns>A task representing the asynchronous operation, yielding a collection of filtered users.</returns>
    public async Task<IEnumerable<User>> FilterByActiveAsync(bool isActive)
    {
        var allUsers = await _dataAccess.GetAllAsync<User>();
        return allUsers.Where(user => user.IsActive == isActive);
    }

    /// <summary>
    /// Retrieves all users asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding a collection of all users.</returns>
    public async Task<IEnumerable<User>> GetAllAsync()
    {

        return await _dataAccess.GetAllAsync<User>();

    }

    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CreateAsync(User? user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        ValidationHelper.ModelValidation(user);

        //if (user.Id <= 0)
        //{
        //    var maxId = (await _dataAccess.GetAllAsync<User>()).Max(u => (long?)u.Id) ?? 0;
        //    user.Id = maxId + 1;
        //}
        await _dataAccess.CreateAsync(user);
        _crudActionsLogger.logCrudActions("Create", user.Id.ToString());
    }

    /// <summary>
    /// Retrieves a user by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, yielding the user with the specified ID.</returns>
    public async Task<User> GetByIdAsync(long id)
    {
        var user = (await _dataAccess.GetAllAsync<User>()).FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} was not found.");
        }

        _crudActionsLogger.logCrudActions("Read", user.Id.ToString());

        return user;
    }

    /// <summary>
    /// Updates an existing user asynchronously.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <returns>A task representing the asynchronous operation, yielding the updated user.</returns>
    public async Task<User> UpdateAsync(User? user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        ValidationHelper.ModelValidation(user);
        await _dataAccess.UpdateAsync(user);

        _crudActionsLogger.logCrudActions("Update", user.Id.ToString());

        return user;
    }

    /// <summary>
    /// Deletes a user asynchronously.
    /// </summary>
    /// <param name="user">The user to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteAsync(User? user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        await GetByIdAsync(user.Id);
        await _dataAccess.DeleteAsync(user);

        _crudActionsLogger.logCrudActions("Delete", user.Id.ToString());


    }
}
