using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Interfaces;
using UserManagement.WebAPI.Controllers;
using UserManagement.WebAPI.DTO;

namespace UserManagement.WebAPI.Test;
public class UserControllerTests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<ILoggedEntriesService> _loggedEntriesServiceMock = new();

        private UsersController CreateController()
        {
            return new UsersController(_userServiceMock.Object, _loggedEntriesServiceMock.Object);
        }
        #region GetAllUsers_WhenServiceReturnsUsers

        [Fact]
        public async Task GetAllUsers_WhenServiceReturnsUsers_ModelMustContainUsers()
        {
            // Arrange
            var controller = CreateController();
            var users = SetupUsers(("John", "User", "juser@example.com", true, "10/10/1992"), ("Jane", "User2", "juser2@example.com", false, "10/10/1991"));
            _userServiceMock.Setup(s => s.GetAllAsync()).Returns(Task.FromResult<IEnumerable<User>>(users));

            // Act
            var result = await controller.GetUsers(null);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<PaginatedResult<UserDTO>>()
                .Which.Items.Should().BeEquivalentTo(users.Select(u => new UserDTO
                {
                    Id = u.Id,
                    Forename = u.Forename,
                    Surname = u.Surname,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    DateOfBirth = u.DateOfBirth
                }));
        }

        #endregion

        #region GetUser_WithValidId

        [Fact]
        public async Task GetUser_WithValidId_ReturnsUserDTO()
        {
            // Arrange
            var controller = CreateController();
            var user = SetupUsers(("John", "Doe", "john@example.com", true, "10/10/1991"), ("Jane", "User2", "juser2@example.com", false, "10/10/1991"))[0];

            // Act
            var result = await controller.GetUser(user.Id);

            // Assert
            var userDTO = (result.Result as OkObjectResult)?.Value as UserDTO;
            userDTO?.Forename.Should().Be("John");
        }

        #endregion

        #region CreateUser_WithValidUser

        [Fact]
        public async Task CreateUser_WithValidUser_ReturnsCreatedUser()
        {
            // Arrange
            var controller = CreateController();
            var user = new User { Id = 1, Forename = "John", Surname = "Doe", Email = "john@example.com", IsActive = true, DateOfBirth = DateTime.Parse("10/10/1991") };

            // Act
            var result = await controller.CreateUser(user);

            // Assert
            result.Should().BeOfType<ActionResult<User>>();
            var userDTO = (result.Result as OkObjectResult)?.Value as UserDTO;
            userDTO?.Forename.Should().Be("John");
        }

        #endregion

        #region UpdateUser_WithValidIdAndUserDTO

        [Fact]
        public async Task UpdateUser_WithValidIdAndUserDTO_RetursUpdatedDetails()
        {
            // Arrange
            var controller = CreateController();
            var user = SetupUsers(("John", "user", "john@example.com", true, "10/10/1991"), ("Jane", "User2", "juser2@example.com", false, "10/10/1991"))[0];
            var userDTO = new UserDTO { Id = user.Id, Forename = user.Forename, Surname = "NewName", IsActive = false, Email = user.Email };

            // Act
            await controller.UpdateUser(userDTO.Id, userDTO);
            var result = await controller.GetUser(user.Id);

            // Assert
            var updatedUserDTO = (result.Result as OkObjectResult)?.Value as UserDTO;
            updatedUserDTO?.Surname.Should().Be("NewName");
        }

        #endregion

        #region DeleteUser_WithValidId

        [Fact]
        public async Task DeleteUser_WithValidId_Returns404()
        {
            // Arrange
            var controller = CreateController();
            var user = SetupUsers(("John", "user", "john@example.com", true, "10/10/1991"), ("Jane", "User2", "juser2@example.com", false, "10/10/1991"))[0];

            // Act
            var result = await controller.DeleteUser(user.Id);

            // Assert
            var problemResult = result.Should().BeOfType<ObjectResult>().Subject;
            problemResult.Should().NotBeNull();
            problemResult.StatusCode.Should().Be(404);
        }

        #endregion




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

            _userServiceMock
                .Setup(s => s.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<User>>(users));

            return users;
        }
    }
}



