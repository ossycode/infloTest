using System;
using System.Linq;
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

    [Fact]
    public void GetAll_WhenNewEntityAdded_MustIncludeNewEntity()
    {
        // Arrange: 
        using (var context = new DataContext(_options))
        {
            var entity = new User
            {
                Id = 1,
                Forename = "Jane",
                Surname = "User",
                Email = "jane1@example.com",
                DateOfBirth = DateTime.Parse("6/11/1980")
            };

            // Act: Invokes the method under test with the arranged parameters.
            context.Create(entity);

            // Assert: Verifies that the action of the method under test behaves as expected.
            var result = context.GetAll<User>();

            result
                .Should().Contain(s => s.Email == entity.Email)
                .Which.Should().BeEquivalentTo(entity);
        }
    }


    [Fact]
    public void GetAll_WhenDeleted_MustNotIncludeDeletedEntity()
    {
        // Arrange
        using (var context = new DataContext(_options))
        {
            var user1 = new User { Forename = "John", Surname = "User", Email = "john@example.com" };
            var user2 = new User { Forename = "Jane", Surname = "User", Email = "jane@example.com" };
            context.Create(user1);
            context.Create(user2);
            var entityToDelete = context.GetAll<User>().First();

            // Act
            context.Delete(entityToDelete);

            var result = context.GetAll<User>();

            // Assert
            result.Should().NotContain(entityToDelete);
            //result.Should().NotContain(s => s.Email == entityToDelete.Email);
        }

    }

    [Fact]
    public void Update_WhenEntityUpdated_MustReflectChanges()
    {
        using (var context = new DataContext(_options))
        {
            // Arrange
            var user1 = new User { Forename = "John", Surname = "User", Email = "john@example.com" };
            context.Create(user1);
            var entityToUpdate = context.GetAll<User>().First();


            var newSurname = "User1";
            entityToUpdate.Surname = newSurname;

            // Act: Invokes the method under test with the arranged parameters.
            context.Update(entityToUpdate);

            // Assert: Verifies that the action of the method under test behaves as expected.
            var updatedEntity = context.GetAll<User>().FirstOrDefault(u => u.Id == entityToUpdate.Id);

            Assert.NotNull(updatedEntity);
            updatedEntity.Surname.Should().Be(newSurname);


        }
    }

}
