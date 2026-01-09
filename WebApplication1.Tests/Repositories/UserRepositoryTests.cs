using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Entities;
using WebApplication1.Repositories;
using Xunit;

namespace WebApplication1.Tests.Repositories;

public class UserRepositoryTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateUser()
    {
        // Arrange
        using var context = GetDbContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash"
        };

        // Act
        var result = await repository.CreateAsync(user);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("testuser", result.Username);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser()
    {
        // Arrange
        using var context = GetDbContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        var created = await repository.CreateAsync(user);

        // Act
        var result = await repository.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("testuser", result.Username);
    }

    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnUser()
    {
        // Arrange
        using var context = GetDbContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        await repository.CreateAsync(user);

        // Act
        var result = await repository.GetByUsernameAsync("testuser");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser()
    {
        // Arrange
        using var context = GetDbContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        var created = await repository.CreateAsync(user);
        created.FirstName = "Updated";

        // Act
        var result = await repository.UpdateAsync(created);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated", result.FirstName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteUser()
    {
        // Arrange
        using var context = GetDbContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        var created = await repository.CreateAsync(user);

        // Act
        await repository.DeleteAsync(created.Id);

        // Assert
        var result = await repository.GetByIdAsync(created.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        using var context = GetDbContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        var created = await repository.CreateAsync(user);

        // Act
        var result = await repository.ExistsAsync(created.Id);

        // Assert
        Assert.True(result);
    }
}


