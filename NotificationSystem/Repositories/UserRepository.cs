using NotificationSystem.Entities;

namespace NotificationSystem.Repositories;

public sealed class UserRepository(AppDbContext dbContext) : IUserRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<User?> GetUserById(int id)
    {
        return await _dbContext.Users.FindAsync(id);
    }
}
