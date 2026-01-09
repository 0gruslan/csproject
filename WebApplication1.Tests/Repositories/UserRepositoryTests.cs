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
        using var context = GetDbContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash"
        };

        var result = await repository.CreateAsync(user);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("testuser", result.Username);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser()
    {
        using var context = GetDbContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        var created = await repository.CreateAsync(user);

        var result = await repository.GetByIdAsync(created.Id);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("testuser", result.Username);
    }

    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnUser()
    {
        using var context = GetDbContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        await repository.CreateAsync(user);

        var result = await repository.GetByUsernameAsync("testuser");

        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser()
    {
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

        var result = await repository.UpdateAsync(created);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.FirstName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteUser()
    {
        using var context = GetDbContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        var created = await repository.CreateAsync(user);

        await repository.DeleteAsync(created.Id);

        var result = await repository.GetByIdAsync(created.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenUserExists()
    {
        using var context = GetDbContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        var created = await repository.CreateAsync(user);

        var result = await repository.ExistsAsync(created.Id);

        Assert.True(result);
    }
}


