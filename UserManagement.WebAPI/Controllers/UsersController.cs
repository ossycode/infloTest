using System.Data;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Interfaces;
using UserManagement.WebAPI.DTO;

namespace UserManagement.WebAPI.Controllers;

/// <summary>
/// API controller for managing user-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILoggedEntriesService _loggedEntriesService;


    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="userService">The user service.</param>
    /// <param name="loggedEntriesService">The logged entries service.</param>
    public UsersController(IUserService userService, ILoggedEntriesService loggedEntriesService)
    {
        _userService = userService;
        _loggedEntriesService = loggedEntriesService;

    }

    /// <summary>
    /// Retrieves a paginated list of users optionally filtered by their active status.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers(bool? isActive, int pageNo = 1, int pageSize = 10)
    {
        try
        {
            IEnumerable<User> users;

            if (isActive.HasValue)
            {
                users = await _userService.FilterByActiveAsync(isActive.Value);

            }
            else
            {
                users = await _userService.GetAllAsync();
            }
            var paginatedUsers = Paginate(users, pageNo, pageSize);
            var totalRecords = users.Count();
            var totalPage = (int)Math.Ceiling((double)totalRecords / pageSize);


            var data = new PaginatedResult<UserDTO>
            {

                Items = paginatedUsers.Select(u => new UserDTO
                {
                    Id = u.Id,
                    Forename = u.Forename,
                    Surname = u.Surname,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    DateOfBirth = u.DateOfBirth,
                }).ToList(),
                TotalPages = totalPage,
                TotalRecords = totalRecords,

            };
            return Ok(data);
        }
        catch (Exception ex)
        {

            return Problem(detail: $"An error occurred while fetching the users: {ex.Message}", statusCode: 500, title: "Get Users");
        }

    }

    /// <summary>
    /// Retrieves the user with the specified ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUser(long id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return Problem(detail: "User not found with given ID", statusCode: 400, title: "Get User"); ;

        }
        var userDTO = new UserDTO
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive,
        };
        var logEntries = await _loggedEntriesService.GetAllLogEntriesForUserAsync(user.Id.ToString());

        userDTO.LogEntries = logEntries.ToList();


        return userDTO;

    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        try
        {

            await _userService.CreateAsync(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            return Problem(detail: $"An error occurred while creating the user: {ex.Message}", statusCode: 500, title: "Create User");
        }
    }

    /// <summary>
    /// Updates an existing user with the specified ID.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(long id, UserDTO user)
    {

        if (id != user.Id)
        {
            return Problem(detail: "ID Not valid", statusCode: 400, title: "Update User");
        }

        var existingUser = await _userService.GetByIdAsync(id);

        if (existingUser == null)
        {
            return Problem(detail: $"User with ID {id} not found.", statusCode: 404, title: "Update User");
        }

        existingUser.Email = user.Email;
        existingUser.Surname = user.Surname;
        existingUser.Forename = user.Forename;
        existingUser.DateOfBirth = user.DateOfBirth;
        existingUser.IsActive = user.IsActive;

        try
        {

            await _userService.UpdateAsync(existingUser);

        }
        catch (DBConcurrencyException ex)
        {
            if (existingUser != null)
            {

                return Problem(detail: $"User with ID {id} not found.", statusCode: 404, title: "Update User");
            }
            else
            {
                return Problem(detail: $"An error occurred while updating the user: {ex.Message}", statusCode: 500, title: "Update User");
            }
        }
        return NoContent();
    }


    /// <summary>
    /// Deletes the user with the specified ID.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return Problem(detail: $"User with ID {id} not found.", statusCode: 404, title: "Delete User");
            }
            await _userService.DeleteAsync(user);

            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(detail: $"An error occurred while deleting the user: {ex.Message}", statusCode: 500, title: "Delete User");
        }
    }


    /// <summary>
    /// Paginates a collection of users based on the specified page number and page size.
    /// </summary>
    /// <param name="users">The collection of users to paginate.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The paginated collection of users.</returns>
    private IEnumerable<User> Paginate(IEnumerable<User> users, int pageNumber, int pageSize)
    {
        return users.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

}
