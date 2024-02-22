using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers(("John", "User", "juser@example.com", true), ("Jane", "User2", "juser2@example.com", false));

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }

    [Fact]
    public void FilterByActive_WhenActiveParameterIsTrue_MustReturnOnlyActiveUsers()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(("John", "User", "juser@example.com", true), ("Jane", "User2", "juser2@example.com", false));

        //Act
        var result = service.FilterByActive(true);

        // Assert
        result.Should().BeEquivalentTo(users.Where(user => user.IsActive));

    }

    private IQueryable<User> SetupUsers(params (string forename, string surname, string email, bool isActive)[] userParams)
    {
        var users = userParams.Select(user => new User
        {
            Forename = user.forename,
            Surname = user.surname,
            Email = user.email,
            IsActive = user.isActive

        }).AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        return users;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private UserService CreateService() => new(_dataContext.Object);
}
