using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Entities;
using WebApplication1.Repositories;
using Xunit;

namespace WebApplication1.Tests.Repositories;

public class AccessPointRepositoryTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateAccessPoint()
    {
        using var context = GetDbContext();
        var repository = new AccessPointRepository(context);
        var accessPoint = new AccessPoint
        {
            Name = "Main Entrance",
            Location = "Building A"
        };

        var result = await repository.CreateAsync(accessPoint);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("Main Entrance", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAccessPoint()
    {
        using var context = GetDbContext();
        var repository = new AccessPointRepository(context);
        var accessPoint = new AccessPoint
        {
            Name = "Main Entrance",
            Location = "Building A"
        };
        var created = await repository.CreateAsync(accessPoint);

        var result = await repository.GetByIdAsync(created.Id);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("Main Entrance", result.Name);
    }

    [Fact]
    public async Task GetAllAsync_WithSearch_ShouldFilterResults()
    {
        using var context = GetDbContext();
        var repository = new AccessPointRepository(context);
        
        await repository.CreateAsync(new AccessPoint { Name = "Main Entrance", Location = "Building A" });
        await repository.CreateAsync(new AccessPoint { Name = "Side Door", Location = "Building B" });
        await repository.CreateAsync(new AccessPoint { Name = "Back Entrance", Location = "Building A" });

        var result = await repository.GetAllAsync("Main", 1, 10);

        Assert.Single(result);
        Assert.Equal("Main Entrance", result.First().Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAccessPoint()
    {
        using var context = GetDbContext();
        var repository = new AccessPointRepository(context);
        var accessPoint = new AccessPoint
        {
            Name = "Main Entrance",
            Location = "Building A"
        };
        var created = await repository.CreateAsync(accessPoint);
        created.Name = "Updated Entrance";

        var result = await repository.UpdateAsync(created);

        Assert.NotNull(result);
        Assert.Equal("Updated Entrance", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteAccessPoint()
    {
        using var context = GetDbContext();
        var repository = new AccessPointRepository(context);
        var accessPoint = new AccessPoint
        {
            Name = "Main Entrance",
            Location = "Building A"
        };
        var created = await repository.CreateAsync(accessPoint);

        await repository.DeleteAsync(created.Id);

        var result = await repository.GetByIdAsync(created.Id);
        Assert.Null(result);
    }
}


