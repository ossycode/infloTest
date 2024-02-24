using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    #region GetSameEntities
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers(("John", "User", "juser@example.com", true, "10/10/1991"), ("Jane", "User2", "juser2@example.com", false, "10/10/1991"));

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }
    #endregion

    #region ReturnActiveUsers
    [Fact]
    public void FilterByActive_WhenActiveParameterIsTrue_MustReturnOnlyActiveUsers()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(("John", "User", "juser@example.com", true, "10/10/1991"), ("Jane", "User2", "juser2@example.com", false, "10/10/1991"));

        //Act
        var result = service.FilterByActive(true);

        // Assert
        result.Should().BeEquivalentTo(users.Where(user => user.IsActive));

    }
    #endregion

    #region ReturnInActiveUsers
    [Fact]
    public void FilterByActive_WhenActiveParameterIsFalse_MustReturnOnlyInActiveUsers()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(("John", "User", "juser@example.com", true, "10/10/1991"), ("Jane", "User2", "juser2@example.com", false, "10/10/1992"));

        //Act
        var result = service.FilterByActive(false);

        // Assert
        result.Should().BeEquivalentTo(users.Where(user => !user.IsActive));

    }
    #endregion

    #region AddUserSuccess
    [Fact]
    public void Create_WithValidUser_ShouldAddUserSuccessfully()
    {
        // Arrange
        var service = CreateService();
        var newUser = new User { Forename = "New", Surname = "User", Email = "nuser@example.com", IsActive = true, DateOfBirth = DateTime.Parse("1/1/1990") };

        // Act
        service.Create(newUser);

        // Assert
        _dataContext.Verify(d => d.Create(It.IsAny<User>()), Times.Once);
    }
    #endregion

    #region AddNullUserException
    [Fact]
    public void Create_WithNullUser_ShouldThrowArgumentNullException()
    {
        // Arrange
        var service = CreateService();
        User? nullUser = null;


        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => service.Create(nullUser));
    }
    #endregion

    #region GetUserByValidId
    [Fact]
    public void GetById_WithExistingId_ShouldReturnCorrectUser()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(("Jane", "User2", "juser2@example.com", false, "10/10/1991"));
        var firstUser = users.FirstOrDefault();

        // Act
        Assert.NotNull(firstUser);
        var result = service.GetById(firstUser.Id);

        // Assert
        result.Should().BeEquivalentTo(firstUser);
    }
    #endregion

    #region GetUserByIdWithNonExistingId
    [Fact]
    public void GetById_WithNonExistingId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var service = CreateService();
        SetupUsers();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => service.GetById(999));
    }

    #endregion

    #region UpdateUserSuccess
    [Fact]
    public void Update_WithExistingUser_ShouldUpdateSuccessfully()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(("Jane", "User2", "juser2@example.com", false, "10/10/1991"));
        var firstUser = users.FirstOrDefault();

        // Act
        service.Update(firstUser);

        // Assert
        _dataContext.Verify(d => d.Update(It.IsAny<User>()), Times.Once);
    }
    #endregion

    #region UpdateNullUser
    [Fact]
    public void Update_WithNullUser_ToBeArgumentNullException()
    {
        // Arrange
        var service = CreateService();
        User? userToUpdate = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => service.Update(userToUpdate));
    }
    #endregion

    #region DeleteValidUser
    [Fact]
    public void Delete_WithExistingUser_ShouldDeleteSuccessfully()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(("Jane", "User2", "juser2@example.com", false, "10/10/1991"));
        var firstUser = users.FirstOrDefault();

        // Act
        service.Delete(firstUser);

        // Assert
        _dataContext.Verify(d => d.Delete(It.IsAny<User>()), Times.Once);
    }
    #endregion

    #region DeleteInvalidUser
    [Fact]
    public void Delete_WithNonExistingUser_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var service = CreateService();
        var nonExistingUser = new User { Id = 100, Forename = "forname", Surname = "User", Email = "neuser@example.com", IsActive = false, DateOfBirth = DateTime.Parse("1/1/1990") };

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => service.Delete(nonExistingUser));
    }
    #endregion

    #region SetupUsers
    private IQueryable<User> SetupUsers(params (string forename, string surname, string email, bool isActive, string dateOfBirth)[] userParams)
    {
        var users = userParams.Select(u => new User
        {
            Forename = u.forename,
            Surname = u.surname,
            Email = u.email,
            IsActive = u.isActive,
            DateOfBirth = DateTime.Parse(u.dateOfBirth)

        }).AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        return users;
    }

    #endregion


    private readonly Mock<IDataContext> _dataContext = new();
    private UserService CreateService() => new(_dataContext.Object);
}
