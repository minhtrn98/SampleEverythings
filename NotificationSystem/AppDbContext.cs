using Microsoft.EntityFrameworkCore;
using NotificationSystem.Entities;

namespace NotificationSystem;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<FailedNotification> FailedNotifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Database.Migrate();
        base.OnModelCreating(modelBuilder);
    }
}
