using System.Linq;
using System.Threading.Tasks;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("[controller]")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;


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


    [Route("[action]")]
    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Title"] = "Users | Create User";
        return View();
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserViewModel viewModel)
    {

        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return View(viewModel);
        }

        var user = viewModel.ToUser();

        await _userService.CreateAsync(user);

        return RedirectToAction("List", "Users");
    }

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
        var updateUserViewModel = new UpdateUserViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive
        };


        return View(updateUserViewModel);
    }

    [Route("[action]/{id}")]
    [HttpPost]
    public async Task<IActionResult> Edit(UpdateUserViewModel updateUserViewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return View(updateUserViewModel);
        }
        var user = await _userService.GetByIdAsync(updateUserViewModel.Id);

        if (user == null)
        {
            return RedirectToAction("List", "Users");
        }

        user.Forename = updateUserViewModel.Forename ?? user.Forename;
        user.Surname = updateUserViewModel.Surname ?? user.Surname;
        user.Email = updateUserViewModel.Email ?? user.Email;
        user.DateOfBirth = updateUserViewModel.DateOfBirth ?? user.DateOfBirth;
        user.IsActive = updateUserViewModel.IsActive;

        var updatedUser = await _userService.UpdateAsync(user);

        return RedirectToAction("List", "Users");
    }


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

        var userViewModel = new UserViewModel
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

    [Route("[action]/{id}")]
    [HttpGet]
    public async Task<IActionResult> Delete(long id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return RedirectToAction("List");
        }

        var userViewModel = new UserViewModel
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


    [Route("[action]/{id}")]
    [HttpPost]
    public async Task<IActionResult> Delete(UserViewModel userViewModel)
    {
        var user = await _userService.GetByIdAsync(userViewModel.Id);

        if (user == null)
        {
            return RedirectToAction("List");
        }

        await _userService.DeleteAsync(user);


        return RedirectToAction("List");
    }
}
