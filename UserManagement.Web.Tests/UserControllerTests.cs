using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    #region GetAllUsers
    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers(("John", "User", "juser@example.com", true, "10/10/1992"), ("Jane", "User2", "juser2@example.com", false, "10/10/1991"));

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.List(isActive: null);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }
    #endregion 


    #region FilterUserByActive
    [Fact]
    public void List_WhenServiceReturnsUsers_OnlyActiveUsersAreReturned()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers(("John", "User", "juser@example.com", true, "10/10/1992"), ("Jane", "User2", "juser2@example.com", true, "10/10/1991"));

        _userService.Setup(s => s.FilterByActive(true)).Returns(users);

        // Act
        var result = controller.List(isActive: true);

        // Assert
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);

    }
    #endregion

    #region FilterUserByInActive
    [Fact]
    public void List_WhenServiceReturnsUsers_OnlyInActiveUsersAreReturned()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers(("John", "User", "juser@example.com", false, "10/10/1992"), ("Jane", "User2", "juser2@example.com", false, "10/10/1991"));

        _userService.Setup(s => s.FilterByActive(false)).Returns(users);

        // Act
        var result = controller.List(isActive: false);

        // Assert
        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(users);

    }
    #endregion

    #region CreateUserIsInValid
    [Fact]
    public void Create_WhenModelStateIsInvalid_ReturnsViewWithErrors()
    {
        // Arrange
        var controller = CreateController();
        var viewModel = new CreateUserViewModel();

        // Manually add model state error for testing
        controller.ModelState.AddModelError("Forename", "Forename can't be blank");

        // Act
        IActionResult result = controller.Create(viewModel);

        // Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        viewResult.ViewData.Model.Should().BeAssignableTo<CreateUserViewModel>();
        viewResult.ViewData.Model.Should().Be(viewModel);

    }
    #endregion

    #region CreateUserAndRedirect
    [Fact]
    public void Create_WhenModelStateIsValid_CreatesUserAndRedirects()
    {
        // Arrange
        var controller = CreateController();
        var viewModel = new CreateUserViewModel();

        // Act
        IActionResult result = controller.Create(viewModel);

        // Assert
        RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);

        redirectResult.ActionName.Should().Be("List");

    }
    #endregion

    #region EditValidUser
    [Fact]
    public void Edit_WithValidViewModel_ShouldUpdateUserSuccessfully()
    {
        // Arrange
        var controller = CreateController();
        var user = SetupUsers(("John", "Doe", "john@example.com", true, "10/10/1991"))[0];
        var updateUserViewModel = new UpdateUserViewModel { Id = user.Id, Forename = "Jane" };
        _userService.Setup(m => m.GetById(user.Id)).Returns(user);

        // Act
        IActionResult result = controller.Edit(updateUserViewModel);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        redirectToActionResult.ActionName.Should().Be("List");
        _userService.Verify(m => m.Update(user), Times.Once);
    }
    #endregion

    #region DeleteValidUser
    [Fact]
    public void Delete_WithValidId_ShouldDeleteUserSuccessfully()
    {
        // Arrange
        var controller = CreateController();
        var user = new User { Id = 1, Forename = "John" };
        _userService.Setup(m => m.GetById(user.Id)).Returns(user);

        // Act
        var result = controller.Delete(new UserViewModel { Id = user.Id });

        // Assert
        _userService.Verify(m => m.Delete(user), Times.Once);
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
            .Setup(s => s.GetAll())
            .Returns(users);

        return users;
    }
    #endregion



    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController() => new(_userService.Object);
}
