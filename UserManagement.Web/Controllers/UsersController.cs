using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("[controller]")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;


    [HttpGet]
    public ViewResult List(bool? isActive)
    {
        ViewData["Title"] = "Users List";

        IEnumerable<UserListItemViewModel> items;

        if (isActive.HasValue)
        {
            var filteredUsers = _userService.FilterByActive(isActive.Value);

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
            var allUsers = _userService.GetAll();

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
    public IActionResult Create(CreateUserViewModel viewModel)
    {

        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return View(viewModel);
        }

        var user = viewModel.ToUser();

        _userService.Create(user);

        return RedirectToAction("List", "Users");
    }

    [Route("[action]/{id}")]
    [HttpGet]
    public IActionResult Edit(long id)
    {
        ViewData["Title"] = "Users | Edit User";
        var user = _userService.GetById(id);
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
    public IActionResult Edit(UpdateUserViewModel updateUserViewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return View(updateUserViewModel);
        }
        var user = _userService.GetById(updateUserViewModel.Id);

        if (user == null)
        {
            return RedirectToAction("List", "Users");
        }

        user.Forename = updateUserViewModel.Forename ?? user.Forename;
        user.Surname = updateUserViewModel.Surname ?? user.Surname;
        user.Email = updateUserViewModel.Email ?? user.Email;
        user.DateOfBirth = updateUserViewModel.DateOfBirth ?? user.DateOfBirth;
        user.IsActive = updateUserViewModel.IsActive;

        var updatedUser = _userService.Update(user);

        return RedirectToAction("List", "Users");
    }


    [Route("[action]/{id}")]
    [HttpGet]
    public IActionResult Details(long id)
    {
        ViewData["Title"] = "Users | User Details";

        var user = _userService.GetById(id);

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
    public IActionResult Delete(long id)
    {
        var user = _userService.GetById(id);

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
    public IActionResult Delete(UserViewModel userViewModel)
    {
        var user = _userService.GetById(userViewModel.Id);

        if (user == null)
        {
            return RedirectToAction("List");
        }

        _userService.Delete(user);


        return RedirectToAction("List");
    }
}
