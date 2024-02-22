using System;
using System.Linq;
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
