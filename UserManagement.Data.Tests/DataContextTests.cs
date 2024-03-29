using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;


namespace UserManagement.Data.Tests;

public class DataContextTests
{
    private readonly DbContextOptions<DataContext> _options;

    public DataContextTests()
    {
        _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    #region Get All Entities Tests
    [Fact]
    public async Task GetAll_WhenNewEntityAdded_MustIncludeNewEntity()
    {

        using (var context = new DataContext(_options))
        {
            // Arrange: 
            var entity = new User
            {
                Forename = "Jane",
                Surname = "User",
                Email = "jane1@example.com",
                DateOfBirth = DateTime.Parse("6/11/1980")
            };

            // Act: Invokes the method under test with the arranged parameters.
            await context.CreateAsync(entity);

            // Assert: Verifies that the action of the method under test behaves as expected.
            var result = await context.GetAllAsync<User>();

            result
                .Should().Contain(s => s.Email == entity.Email)
                .Which.Should().BeEquivalentTo(entity);
        }
    }
    #endregion

    #region Delete Entity Tests
    [Fact]
    public async Task GetAll_WhenDeleted_MustNotIncludeDeletedEntity()
    {
        // Arrange
        using (var context = new DataContext(_options))
        {
            var user1 = new User { Forename = "John", Surname = "User", Email = "john@example.com", DateOfBirth = DateTime.Parse("1/1/1990") };
            var user2 = new User { Forename = "Jane", Surname = "User", Email = "jane@example.com", DateOfBirth = DateTime.Parse("1/1/1990") };
            await context.CreateAsync(user1);
            await context.CreateAsync(user2);
            var entityToDelete = (await context.GetAllAsync<User>()).First();

            // Act
            await context.DeleteAsync(entityToDelete);

            var result = await context.GetAllAsync<User>();

            // Assert
            result.Should().NotContain(entityToDelete);
        }

    }
    #endregion

    #region Update entity Tests
    [Fact]
    public async Task Update_WhenEntityUpdated_MustReflectChanges()
    {
        using (var context = new DataContext(_options))
        {
            // Arrange
            var user1 = new User { Forename = "John", Surname = "User", Email = "john@example.com", DateOfBirth = DateTime.Parse("1/1/1990") };
            await context.CreateAsync(user1);
            var entityToUpdate = (await context.GetAllAsync<User>()).First();


            var newSurname = "User1";
            entityToUpdate.Surname = newSurname;

            // Act: Invokes the method under test with the arranged parameters.
            await context.UpdateAsync(entityToUpdate);

            // Assert: Verifies that the action of the method under test behaves as expected.
            var updatedEntity = (await context.GetAllAsync<User>()).FirstOrDefault(u => u.Id == entityToUpdate.Id);

            Assert.NotNull(updatedEntity);
            updatedEntity.Surname.Should().Be(newSurname);


        }
    }
    #endregion

    #region GetEntityByIdAsync Tests

    [Fact]
    public async Task GetEntityByIdAsync_WhenEntityExists_ReturnsEntity()
    {
        // Arrange
        using (var context = new DataContext(_options))
        {
            var expectedEntity = new User { Forename = "John", Surname = "User", Email = "john@example.com", DateOfBirth = DateTime.Parse("1/1/1990") };
            await context.CreateAsync(expectedEntity);

            // Act
            var result = await context.GetEntityByIdAsync<User, long>(expectedEntity.Id);

            // Assert
            result.Should().BeEquivalentTo(expectedEntity);
        }
    }

    [Fact]
    public async Task GetEntityByIdAsync_WhenEntityDoesNotExist_ReturnsNull()
    {
        // Arrange
        using (var context = new DataContext(_options))
        {
            // Act
            var result = await context.GetEntityByIdAsync<User, long>(999);

            // Assert
            result.Should().BeNull();
        }
    }

    #endregion


}
