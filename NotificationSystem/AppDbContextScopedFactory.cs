using Microsoft.EntityFrameworkCore;

namespace NotificationSystem;

public sealed class AppDbContextScopedFactory : IDbContextFactory<AppDbContext>
{
    private readonly IDbContextFactory<AppDbContext> _pooledFactory;

    public AppDbContextScopedFactory(IDbContextFactory<AppDbContext> pooledFactory)
    {
        _pooledFactory = pooledFactory;
    }

    public AppDbContext CreateDbContext()
    {
        AppDbContext context = _pooledFactory.CreateDbContext();
        return context;
    }
}
