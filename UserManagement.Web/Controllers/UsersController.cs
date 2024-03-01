using System.Linq;
using System.Threading.Tasks;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Filters.ActionFilters;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;


/// <summary>
/// Controller for managing user-related actions such as listing users, creating, editing, viewing details, and deleting users.
/// </summary>
[Route("[controller]")]
public class UsersController : Controller
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
    /// Retrieves a list of users based on optional filter criteria and renders the corresponding view.
    /// </summary>
    /// <param name="isActive">Optional filter to retrieve active or inactive users.</param>
    /// <returns>The view containing the list of users.</returns>
    [HttpGet]
    public async Task<ViewResult> List(bool? isActive)
    {
        ViewData["Title"] = "Users List";

        IEnumerable<UserListItemViewModel> items;

        if (isActive.HasValue)
        {
            var filteredUsers = await _userService.FilterByActiveAsync(isActive.Value);

            items = filteredUsers.Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                IsActive = p.IsActive,
                DateOfBirth = p.DateOfBirth,
            });
        }
        else
        {
            var allUsers = await _userService.GetAllAsync();

            items = allUsers.Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                IsActive = p.IsActive,
                DateOfBirth = p.DateOfBirth,
            });
        }

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }


    /// <summary>
    /// Renders the view for creating a new user.
    /// </summary>
    /// <returns>The view for creating a new user.</returns>
    [Route("[action]")]
    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Title"] = "Users | Create User";
        return View();
    }

    /// <summary>
    /// Handles the creation of a new user based on the provided view model data.
    /// </summary>
    /// <param name="viewModel">The view model containing user data.</param>
    /// <returns>A redirection to the user list view upon successful creation.</returns>
    [Route("[action]")]
    [HttpPost]
    [TypeFilter(typeof(UserCreateAndEditActionFilter))]
    public async Task<IActionResult> Create(UserListItemViewModel viewModel)
    {
        var user = viewModel.ToUser();

        await _userService.CreateAsync(user);

        return RedirectToAction("List", "Users");
    }



    /// <summary>
    /// Renders the view for editing an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to edit.</param>
    /// <returns>The view for editing the specified user.</returns>
    [Route("[action]/{id}")]
    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        ViewData["Title"] = "Users | Edit User";
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return RedirectToAction("List");
        }
        var viewModel = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive
        };


        return View(viewModel);
    }


    /// <summary>
    /// Handles the update of an existing user based on the provided view model data.
    /// </summary>
    /// <param name="viewModel">The view model containing updated user data.</param>
    /// <returns>A redirection to the user list view upon successful update.</returns>
    [Route("[action]/{id}")]
    [HttpPost]
    [TypeFilter(typeof(UserCreateAndEditActionFilter))]
    public async Task<IActionResult> Edit(UserListItemViewModel viewModel)
    {
        if (viewModel == null)
        {
            return RedirectToAction("List");
        }

        var user = await _userService.GetByIdAsync(viewModel.Id);

        if (user == null)
        {
            return RedirectToAction("List", "Users");
        }

        user.Forename = viewModel.Forename ?? user.Forename;
        user.Surname = viewModel.Surname ?? user.Surname;
        user.Email = viewModel.Email ?? user.Email;
        user.DateOfBirth = viewModel.DateOfBirth ?? user.DateOfBirth;
        user.IsActive = viewModel.IsActive;

        await _userService.UpdateAsync(user);


        return RedirectToAction("List", "Users");
    }


    /// <summary>
    /// Renders the view for viewing details of a specific user.
    /// </summary>
    /// <param name="id">The ID of the user to view details for.</param>
    /// <returns>The view displaying details of the specified user.</returns>
    [Route("[action]/{id}")]
    [HttpGet]
    public async Task<IActionResult> Details(long id)
    {
        ViewData["Title"] = "Users | User Details";

        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return RedirectToAction("List");
        }

        var userViewModel = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };
        var logEntries = await _loggedEntriesService.GetAllLogEntriesForUserAsync(user.Id.ToString());

        userViewModel.LogEntries = logEntries;

        return View(userViewModel);
    }


    /// <summary>
    /// Renders the view for confirming the deletion of a specific user.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>The view for confirming the deletion of the specified user.</returns>
    [Route("[action]/{id}")]
    [HttpGet]
    public async Task<IActionResult> Delete(long id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return RedirectToAction("List");
        }

        var userViewModel = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        return View(userViewModel);
    }


    /// <summary>
    /// Handles the deletion of a user based on the provided view model data.
    /// </summary>
    /// <param name="viewModel">The view model containing user data.</param>
    /// <returns>A redirection to the user list view upon successful deletion.</returns>
    [Route("[action]/{id}")]
    [HttpDelete]
    public async Task<IActionResult> Delete(UserListItemViewModel viewModel)
    {
        var user = await _userService.GetByIdAsync(viewModel.Id);

        if (user == null)
        {
            return RedirectToAction("List");
        }

        await _userService.DeleteAsync(user);


        return RedirectToAction("List");
    }
}
