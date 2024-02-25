using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;


namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    #region GetSameEntities
    [Fact]
    public async Task GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers(("John", "User", "juser@example.com", true, "10/10/1991"), ("Jane", "User2", "juser2@example.com", false, "10/10/1991"));

        // Act: Invokes the method under test with the arranged parameters.
        var result = await service.GetAllAsync();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(users);

    }
    #endregion

    #region ReturnActiveUsers
    [Fact]
    public async Task FilterByActive_WhenActiveParameterIsTrue_MustReturnOnlyActiveUsers()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(("John", "User", "juser@example.com", true, "10/10/1991"), ("Jane", "User2", "juser2@example.com", false, "10/10/1991"));

        //Act
        var result = await service.FilterByActiveAsync(true);

        // Assert
        result.Should().BeEquivalentTo(users.Where(user => user.IsActive));

    }
    #endregion

    #region ReturnInActiveUsers
    [Fact]
    public async Task FilterByActive_WhenActiveParameterIsFalse_MustReturnOnlyInActiveUsers()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(("John", "User", "juser@example.com", true, "10/10/1991"), ("Jane", "User2", "juser2@example.com", false, "10/10/1992"));

        //Act
        var result = await service.FilterByActiveAsync(false);

        // Assert
        result.Should().BeEquivalentTo(users.Where(user => !user.IsActive));

    }
    #endregion

    #region AddUserSuccess
    [Fact]
    public async Task Create_WithValidUser_ShouldAddUserSuccessfully()
    {
        // Arrange
        var service = CreateService();
        var newUser = new User { Forename = "New", Surname = "User", Email = "nuser@example.com", IsActive = true, DateOfBirth = DateTime.Parse("1/1/1990") };

        // Act
        await service.CreateAsync(newUser);

        // Assert
        _dataContext.Verify(d => d.CreateAsync(It.IsAny<User>()), Times.Once);
    }
    #endregion

    #region AddNullUserException
    [Fact]
    public async Task Create_WithNullUser_ShouldThrowArgumentNullException()
    {
        // Arrange
        var service = CreateService();
        User? nullUser = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.CreateAsync(nullUser));
    }
    #endregion

    #region GetUserByValidId
    [Fact]
    public async Task GetById_WithExistingId_ShouldReturnCorrectUser()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(("Jane", "User2", "juser2@example.com", false, "10/10/1991"));
        var firstUser = users.FirstOrDefault();

        // Act
        Assert.NotNull(firstUser);
        var result = await service.GetByIdAsync(firstUser.Id);

        // Assert
        result.Should().BeEquivalentTo(firstUser);
    }
    #endregion

    #region GetUserByIdWithNonExistingId
    [Fact]
    public async Task GetById_WithNonExistingId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var service = CreateService();
        SetupUsers();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.GetByIdAsync(999));
    }

    #endregion

    #region UpdateUserSuccess
    [Fact]
    public async Task Update_WithExistingUser_ShouldUpdateSuccessfully()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(("Jane", "User2", "juser2@example.com", false, "10/10/1991"));
        var firstUser = users.FirstOrDefault();

        // Act
        await service.UpdateAsync(firstUser);

        // Assert
        _dataContext.Verify(d => d.UpdateAsync(It.IsAny<User>()), Times.Once);
    }
    #endregion

    #region UpdateNullUser
    [Fact]
    public async Task Update_WithNullUser_ToBeArgumentNullException()
    {
        // Arrange
        var service = CreateService();
        User? userToUpdate = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateAsync(userToUpdate));
    }
    #endregion

    #region DeleteValidUser
    [Fact]
    public async Task Delete_WithExistingUser_ShouldDeleteSuccessfully()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(("Jane", "User2", "juser2@example.com", false, "10/10/1991"));
        var firstUser = users.FirstOrDefault();

        // Act
        await service.DeleteAsync(firstUser);

        // Assert
        _dataContext.Verify(d => d.DeleteAsync(It.IsAny<User>()), Times.Once);
    }
    #endregion

    #region DeleteInvalidUser
    [Fact]
    public async Task Delete_WithNonExistingUser_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var service = CreateService();
        var nonExistingUser = new User { Id = 100, Forename = "forname", Surname = "User", Email = "neuser@example.com", IsActive = false, DateOfBirth = DateTime.Parse("1/1/1990") };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.DeleteAsync(nonExistingUser));
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
            .Setup(s => s.GetAllAsync<User>())
            .Returns(Task.FromResult(users));

        return users;
    }

    #endregion


    private readonly Mock<IDataContext> _dataContext = new();
    private UserService CreateService() => new(_dataContext.Object);
}
