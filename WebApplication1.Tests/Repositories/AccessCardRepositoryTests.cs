using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using System.Collections.Generic;
using WebApplication1.Data;
using WebApplication1.Models.Entities;
using WebApplication1.Repositories;
using Xunit;

namespace WebApplication1.Tests.Repositories;

public class AccessCardRepositoryTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private IConfiguration GetMockConfiguration()
    {
        var inMemorySettings = new Dictionary<string, string?> {
            {"ConnectionStrings:DefaultConnection", "Host=localhost;Database=test;Username=test;Password=test"}
        };
        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateAccessCard()
    {
        // Arrange
        using var context = GetDbContext();
        var mockConfig = GetMockConfiguration();
        var repository = new AccessCardRepository(context, mockConfig);
        var accessCard = new AccessCard
        {
            CardNumber = "CARD001",
            CardType = "RFID"
        };

        // Act
        var result = await repository.CreateAsync(accessCard);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("CARD001", result.CardNumber);
        Assert.Equal("RFID", result.CardType);
        Assert.True(result.CreatedAt.Kind == DateTimeKind.Utc);
        Assert.True(result.UpdatedAt.Kind == DateTimeKind.Utc);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAccessCard()
    {
        // Arrange
        using var context = GetDbContext();
        var mockConfig = GetMockConfiguration();
        var repository = new AccessCardRepository(context, mockConfig);
        var accessCard = new AccessCard
        {
            CardNumber = "CARD001",
            CardType = "RFID"
        };
        var created = await repository.CreateAsync(accessCard);

        // Act
        var result = await repository.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("CARD001", result.CardNumber);
    }

    [Fact]
    public async Task GetByCardNumberAsync_ShouldReturnAccessCard()
    {
        // Arrange
        using var context = GetDbContext();
        var mockConfig = GetMockConfiguration();
        var repository = new AccessCardRepository(context, mockConfig);
        var accessCard = new AccessCard
        {
            CardNumber = "CARD001",
            CardType = "RFID"
        };
        await repository.CreateAsync(accessCard);

        // Act
        var result = await repository.GetByCardNumberAsync("CARD001");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("CARD001", result.CardNumber);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAccessCards()
    {
        // Arrange
        using var context = GetDbContext();
        var mockConfig = GetMockConfiguration();
        var repository = new AccessCardRepository(context, mockConfig);
        
        await repository.CreateAsync(new AccessCard { CardNumber = "CARD001", CardType = "RFID" });
        await repository.CreateAsync(new AccessCard { CardNumber = "CARD002", CardType = "Magnetic" });

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAccessCard()
    {
        // Arrange
        using var context = GetDbContext();
        var mockConfig = GetMockConfiguration();
        var repository = new AccessCardRepository(context, mockConfig);
        var accessCard = new AccessCard
        {
            CardNumber = "CARD001",
            CardType = "RFID"
        };
        var created = await repository.CreateAsync(accessCard);
        created.CardType = "Magnetic";
        created.IsActive = false;

        // Act
        var result = await repository.UpdateAsync(created);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Magnetic", result.CardType);
        Assert.False(result.IsActive);
        Assert.True(result.UpdatedAt.Kind == DateTimeKind.Utc);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteAccessCard()
    {
        // Arrange
        using var context = GetDbContext();
        var mockConfig = GetMockConfiguration();
        var repository = new AccessCardRepository(context, mockConfig);
        var accessCard = new AccessCard
        {
            CardNumber = "CARD001",
            CardType = "RFID"
        };
        var created = await repository.CreateAsync(accessCard);

        // Act
        await repository.DeleteAsync(created.Id);

        // Assert
        var result = await repository.GetByIdAsync(created.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenAccessCardExists()
    {
        // Arrange
        using var context = GetDbContext();
        var mockConfig = GetMockConfiguration();
        var repository = new AccessCardRepository(context, mockConfig);
        var accessCard = new AccessCard
        {
            CardNumber = "CARD001",
            CardType = "RFID"
        };
        var created = await repository.CreateAsync(accessCard);

        // Act
        var result = await repository.ExistsAsync(created.Id);

        // Assert
        Assert.True(result);
    }


}

