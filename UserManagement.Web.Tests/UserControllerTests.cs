using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    #region GetAllUsers
    [Fact]
    public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers(("John", "User", "juser@example.com", true, "10/10/1992"), ("Jane", "User2", "juser2@example.com", false, "10/10/1991"));

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List(isActive: null);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }
    #endregion 


    #region FilterUserByActive
    [Fact]
    public async Task List_WhenServiceReturnsUsers_OnlyActiveUsersAreReturned()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers(("John", "User", "juser@example.com", true, "10/10/1992"), ("Jane", "User2", "juser2@example.com", true, "10/10/1991"));

        _userService.Setup(s => s.FilterByActiveAsync(true)).Returns(Task.FromResult<IEnumerable<User>>(users));

        // Act
        var result = await controller.List(isActive: true);

        // Assert
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);

    }
    #endregion

    #region FilterUserByInActive
    [Fact]
    public async Task List_WhenServiceReturnsUsers_OnlyInActiveUsersAreReturned()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers(("John", "User", "juser@example.com", false, "10/10/1992"), ("Jane", "User2", "juser2@example.com", false, "10/10/1991"));

        _userService.Setup(s => s.FilterByActiveAsync(false)).Returns(Task.FromResult<IEnumerable<User>>(users));

        // Act
        var result = await controller.List(isActive: false);

        // Assert
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);

    }
    #endregion


    #region CreateUserAndRedirect
    [Fact]
    public async Task Create_WhenModelStateIsValid_CreatesUserAndRedirects()
    {
        // Arrange
        var controller = CreateController();
        var viewModel = new UserListItemViewModel();

        // Act
        IActionResult result = await controller.Create(viewModel);

        // Assert
        RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);

        redirectResult.ActionName.Should().Be("List");

    }
    #endregion

    #region EditValidUser
    [Fact]
    public async Task Edit_WithValidViewModel_ShouldUpdateUserSuccessfullyAndRedirects()
    {
        // Arrange
        var controller = CreateController();
        var user = SetupUsers(("John", "Doe", "john@example.com", true, "10/10/1991"))[0];
        var updateUserViewModel = new UserListItemViewModel { Id = user.Id, Forename = "Jane" };

        // Act
        IActionResult result = await controller.Edit(updateUserViewModel);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        redirectToActionResult.ActionName.Should().Be("List");

    }
    #endregion

    #region DeleteValidUser
    [Fact]
    public async Task Delete_WithValidId_ShouldDeleteUserSuccessfullyAndRedirects()
    {
        // Arrange
        var controller = CreateController();
        var user = new User { Id = 1, Forename = "John" };
        _userService.Setup(m => m.GetByIdAsync(user.Id)).Returns(Task.FromResult(user));

        // Act
        var result = await controller.Delete(new UserListItemViewModel { Id = user.Id });

        // Assert
        _userService.Verify(m => m.DeleteAsync(user), Times.Once);
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        redirectToActionResult.ActionName.Should().Be("List");
    }
    #endregion


    #region SetUpUsers
    private User[] SetupUsers(params (string forename, string surname, string email, bool isActive, string dateOfBirth)[] userParams)
    {
        var users = userParams.Select(u => new User
        {
            Forename = u.forename,
            Surname = u.surname,
            Email = u.email,
            IsActive = u.isActive,
            DateOfBirth = DateTime.Parse(u.dateOfBirth)

        }).ToArray();

        _userService
            .Setup(s => s.GetAllAsync())
            .Returns(Task.FromResult<IEnumerable<User>>(users));

        return users;
    }
    #endregion



    private readonly Mock<IUserService> _userService = new();
    private readonly Mock<ILoggedEntriesService> _logEntryService = new();

    private UsersController CreateController()
    {
        return new UsersController(_userService.Object, _logEntryService.Object);
    }
}
