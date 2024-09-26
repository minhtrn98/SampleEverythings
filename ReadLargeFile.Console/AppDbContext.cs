using Microsoft.EntityFrameworkCore;

namespace ReadLargeFile.Console;

internal sealed class AppDbContext : DbContext
{
    public const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=BookStore;";

    public DbSet<Book> Books { get; set; }
    public DbSet<RatingDistribution> RatingDistributions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }
}
