using System;
using System.Linq;
using UserManagement.Models;

namespace UserManagement.Data.Tests;

public class DataContextTests
{
    [Fact]
    public void GetAll_WhenNewEntityAdded_MustIncludeNewEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        var entity = new User
        {
            Forename = "Brand New",
            Surname = "User",
            Email = "brandnewuser@example.com",
            DateOfBirth = DateTime.Parse("6/11/1980")

        };
        context.Create(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().Contain(s => s.Email == entity.Email)
            .Which.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public void GetAll_WhenDeleted_MustNotIncludeDeletedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var entity = context.GetAll<User>().First();
        context.Delete(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotContain(s => s.Email == entity.Email);
    }

    [Fact]
    public void Update_WhenEntityUpdated_MustReflectChanges()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var entityToUpdate = context.GetAll<User>().First();

        var newSurname = "UpdatedSurname";
        entityToUpdate.Surname = newSurname;

        // Act: Invokes the method under test with the arranged parameters.
        context.Update(entityToUpdate);

        // Assert: Verifies that the action of the method under test behaves as expected.
        var updatedEntity = context.GetAll<User>().FirstOrDefault(u => u.Id == entityToUpdate.Id);

        updatedEntity.Should().NotBeNull();
        updatedEntity!.Surname.Should().Be(newSurname);
    }

    private DataContext CreateContext() => new();
}
