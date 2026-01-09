using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.Entities;

namespace WebApplication1.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<AccessPoint> AccessPoints { get; set; }
    public DbSet<AccessCard> AccessCards { get; set; }
    public DbSet<UserAccessCard> UserAccessCards { get; set; }
    public DbSet<AccessCardAccessPoint> AccessCardAccessPoints { get; set; }
    public DbSet<AccessLog> AccessLogs { get; set; }
    public DbSet<ApiKey> ApiKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Role>().ToTable("roles");
        modelBuilder.Entity<Permission>().ToTable("permissions");
        modelBuilder.Entity<AccessPoint>().ToTable("access_points");
        modelBuilder.Entity<AccessCard>().ToTable("access_cards");
        modelBuilder.Entity<AccessLog>().ToTable("access_logs");
        modelBuilder.Entity<ApiKey>().ToTable("api_keys");

        modelBuilder.Entity<UserRole>()
            .ToTable("user_roles")
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<RolePermission>()
            .ToTable("role_permissions")
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<UserAccessCard>()
            .ToTable("user_access_cards")
            .HasKey(uac => new { uac.UserId, uac.AccessCardId });

        modelBuilder.Entity<AccessCardAccessPoint>()
            .ToTable("access_card_access_points")
            .HasKey(acap => new { acap.AccessCardId, acap.AccessPointId });
    }
}


