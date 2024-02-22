using System;
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
