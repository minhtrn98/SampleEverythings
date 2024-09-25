using Microsoft.EntityFrameworkCore;

namespace UploadLargeFile.Api;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<RatingDistribution> RatingDistributions { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    Database.Migrate();
    //    base.OnModelCreating(modelBuilder);
    //}
}
