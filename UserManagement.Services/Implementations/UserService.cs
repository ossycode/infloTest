using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Helpers;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive)
    {
        var allUsers = GetAll();
        return allUsers.Where(user => user.IsActive == isActive);
    }

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

    public void Create(User? user)
    {
        //check if PersonAddRequest is not null
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        //Model validation
        ValidationHelper.ModelValidation(user);

        // Add a user Id
        if (user.Id <= 0)
        {
            var maxId = _dataAccess.GetAll<User>().Max(u => (long?)u.Id) ?? 0;
            user.Id = maxId + 1;
        }

        _dataAccess.Create(user);
    }

    public User GetById(long id)
    {
        var user = _dataAccess.GetAll<User>().FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} was not found.");
        }
        return user;
    }

    public User Update(User? user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        //GetById will throw if the user is not found.
        GetById(user.Id);

        ValidationHelper.ModelValidation(user);
        _dataAccess.Update(user);

        return user;
    }

    public void Delete(User? user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        //GetById will throw error if the user is not found.
        GetById(user.Id);

        _dataAccess.Delete(user);
    }
}
